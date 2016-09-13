## ScriptManager.RegisterStartupScript and behavior of the last argument addScriptTags

This blog post is about registering startup script in the ASP.NET WebForms with `UpdatePanel`. The method `ScriptManager.RegisterStartupScript` takes 5 arguments, four of them are quite obvious. From the documentation:

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

The last one entirely changes the behavior and leads to some unexpected behavior.

```C#
//   addScriptTags:
//     true to enclose the script block with <script> and </script> tags; otherwise,
//     false.
```

When the parameter `addScriptTags` is `true`, everything works as expected, at least to me. The passed script is encoded and registered inside `<script>` tag created by the ASP.NET.

Strange things happen when we register startup script with`false` value for the parameter `addScriptTags`. Without changing anything in the script (except adding `<script>` tag) the page stops working. The end user doesn't see anything, except the missing update of the UpdatePanel. 

When you dig a little and open the browser developer console, you can see javascript error

```
TODO: The exception from the browser's developer console.
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

So, the ASP.NET starts using `JavascriptSerializer` when `addScriptTags` is changed to `false`. Why?

I did not find any answer for this question, but I do have found what is happening. 

```C#
TODO: Part of the source code from the reference source.
```

After registering script (including script tags) the ASP.NET parses out these tags. Than it creates dictionary with single item (`text`=`your script`) and uses `JavascriptSerializer` (need for some encoding?) to serialize the collection. At this point, the previously mentioned exception can raise. `JavascriptSerializer` is created with the default `maxJsonLength`, which can be overriden by the appSettings with key `aspnet:UpdatePanelMaxScriptLength`. 

> This appSetting is parsed as `Int32`, which can be found at http://referencesource.microsoft.com/#System.Web/Util/AppSettings.cs. There are also other 'hidden' ASP.NET appSettings.

The serialized dictionary is then placed in the ajax response, deserialized at the client and run.

#### Conclusion

There is also no limit for the length of the script registered with the `addScriptTags` as `true`. When this is changed, the script is serialized using `JavascriptSerializer` and some length limits apply.
