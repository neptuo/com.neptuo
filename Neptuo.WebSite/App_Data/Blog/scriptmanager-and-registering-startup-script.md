## ScriptManager.RegisterStartupScript and behavior of the last argument addScriptTags

This blog post is about registering startup script in the ASP.NET WebForms. The method `ScriptManager.RegisterStartupScript` takes 5 arguments, four of them are quite obvious.
(TODO: Describe the parameters). The last one dramatically changes the behavior.

> The behavior described in the post is about registering startup script placed in the UpdatePanel.

When parameter `addScriptTags` is `true`, everything works as expected, at least to me. The passed script is encoded and registered inside `<script>` tag created by the ASP.NET. There is also no limit for the length of the script.

But strange things happen when to register startup script with the `false` value for parameter `addScriptTags`. Without changing anything in the script (except adding `<script>` tag) the page stop working. The end user doesn't see anything, except the missing update of the UpdatePanel. When you dig a little and open the browser developer console, you can see javascript error

```
TODO: The exception from the browser's developer console.
```

also, if you are logging unhandled exceptions in the web application, you get

```
TODO: The exception from the HttpApplication.
```

So, why does register startup script use JavascriptSerializer?

I did not find an answer for this question, but I do have found what is happening. 

```C#
TODO: Part of the source code from the reference source.
```

After registering script (including script tags) the ASP.NET parses out these tags. Than it creates dictionary with single item `text`=`your script` and uses JavascriptSerializer to serialize the collection. At this point, the above mentioned exception can raise. JavascriptSerializer is created with the default maxJsonLength, which can be overriden by the appSettings with key `aspnet:UpdatePanelMaxScriptLength`.

> This appSetting is parsed as `Int32`, which can be found at http://referencesource.microsoft.com/#System.Web/Util/AppSettings.cs. There are also other 'hidden' ASP.NET appSettings.

The serialized dictionary is then placed in the ajax response, deserialized at the client and run. As if it was registered without own script tags.
