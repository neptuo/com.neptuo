This post is about registering startup script in the ASP.NET WebForms inside `UpdatePanel`. The method we used `ScriptManager.RegisterStartupScript` takes 5 arguments, four of them are quite obvious. From the documentation:

```C#
//   page:
//     The page object that is registering the client script block.
//
//   type:
//     The type of the client script block. This parameter is usually specified by using
//     the typeof operator (C#) or the GetType operator (Visual Basic) to retrieve the
//     type of the control that is registering the script.
//
//   key:
//     A unique identifier for the script block.
//
//   script:
//     The script to register.
```

The last one entirely changes the behavior and leads to some unexpected behavior and eventually exceptions.

```C#
//   addScriptTags:
//     true to enclose the script block with <script> and </script> tags; otherwise,
//     false.
```

## What happens?

When the parameter `addScriptTags` is `true`, everything works as expected, at least to me. The script is encoded and registered inside `<script>` tag created by the ASP.NET.

But when we register startup script with`false` value for the parameter `addScriptTags`. Without changing anything in the script (except adding `<script>` tag) the page stops working. The end user doesn't see anything, except the missing update of the UpdatePanel. 

When you dig a little and open the browser developer console, you can see javascript error

```
0x800a139e - JavaScript runtime error: Sys.WebForms.PageRequestManagerServerErrorException: Error during serialization or deserialization using the JSON JavaScriptSerializer. The length of the string exceeds the value set on the maxJsonLength property.
```

Also, when logging unhandled exceptions in the web application (`HttpApplication.Error`), you got following

```
System.Web.HttpUnhandledException (0x80004005): ... Error during serialization or deserialization using the JSON JavaScriptSerializer. The length of the string exceeds the value set on the maxJsonLength property.
   at System.Web.Script.Serialization.JavaScriptSerializer.Serialize(Object obj, StringBuilder output, SerializationFormat serializationFormat)
   at System.Web.Script.Serialization.JavaScriptSerializer.Serialize(Object obj, SerializationFormat serializationFormat)
   at System.Web.Script.Serialization.JavaScriptSerializer.Serialize(Object obj)
   at System.Web.UI.ScriptRegistrationManager.WriteScriptWithTags(HtmlTextWriter writer, String token, RegisteredScript activeRegistration)
   at System.Web.UI.ScriptRegistrationManager.RenderActiveScriptBlocks(List`1 updatePanels, HtmlTextWriter writer, String token, List`1 scriptRegistrations)
   at System.Web.UI.ScriptRegistrationManager.RenderActiveScripts(List`1 updatePanels, HtmlTextWriter writer)
   ...
```

So, the ASP.NET starts using `JavascriptSerializer` when `addScriptTags` is changed to `false`. I'm not sure why this happens, maybe some kind of encoding? But why they use `JavascriptSerializer` only when script tags are used from our code? After some digging I have found what is happening under the hood.

## The implementation

> The source is taken from the reference source of the [ScriptRegistrationManager](https://referencesource.microsoft.com/#System.Web.Extensions/UI/ScriptRegistrationManager.cs,646 "ScriptRegistrationManager.WriteScriptWithTags") and shortened for clarity.

```C#
private static void WriteScriptWithTags(HtmlTextWriter writer, string token, RegisteredScript activeRegistration)
{
    // If the content already has script tags, we need to parse out the contents
    // so that the client doesn't have to. The contents may include more than one
    // script tag, but no other content (such as arbitrary HTML).
    string scriptContent = activeRegistration.Script;

    // ... walk through all script tags in the scriptContent and for each of them do:
    for (...)
    {
        OrderedDictionary attrs = new OrderedDictionary();

        // ... get content between script tags.
        string scriptBlockContents = scriptContent.Substring(indexOfEndOfScriptBeginTag, (indexOfScriptEndTag - indexOfEndOfScriptBeginTag));
        attrs.Add("text", scriptBlockContents);
        
        // ... add all attributes defined on the script tag to the attrs dictionary.
        ...
        
        JavaScriptSerializer serializer = new JavaScriptSerializer();

        // Dev10# 877767 - Allow configurable UpdatePanel script block length
        // The default is JavaScriptSerializer.DefaultMaxJsonLength
        if (AppSettings.UpdatePanelMaxScriptLength > 0)
        {
            serializer.MaxJsonLength = AppSettings.UpdatePanelMaxScriptLength;
        }

        string attrText = serializer.Serialize(attrs);
        PageRequestManager.EncodeString(writer, token, "ScriptContentWithTags", attrText);
    }

    // ...
}
```

After registering a script (including script tags) the ASP.NET parses out all these tags. For each of the found script tag, it creates a dictionary with the script content (`text`=`your script`), eventually adds attributes from the tag and uses `JavascriptSerializer` to serialize this dictionary. At this point, the previously mentioned exception can raise. `JavascriptSerializer` is created with the default `maxJsonLength`, which can be overriden by the appSettings with key `aspnet:UpdatePanelMaxScriptLength`. 

> This appSetting is parsed as `Int32`, which can be found at reference source of the [AppSettings](http://referencesource.microsoft.com/#System.Web/Util/AppSettings.cs "System.Web.Util.AppSettings"). There are also other 'hidden' ASP.NET appSettings.

The serialized dictionary is then placed in the ajax response, deserialized at the client and executed by the browser.

## Conclusion

There is no limit for the length of the script registered with the `addScriptTags` as `true`. When this is changed, the script is serialized using `JavascriptSerializer` and some length limits apply.

It is always better to register scripts without tags and let the infrastructure to generate them for you. I even don't known any use-case where I would want to generate them by myself and if you do, please leave a comment. Nevertheless I have found this usage on many places in the customers code base.
