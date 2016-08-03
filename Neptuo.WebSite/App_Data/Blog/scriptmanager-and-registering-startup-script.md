## ScriptManager.RegisterStartupScript and behavior of the last argument addScriptTags

This blog post is about registering startup script in the ASP.NET WebForms. The method `ScriptManager.RegisterStartupScript` takes 5 arguments, four of them are quite obvious.
XXX. The last one dramatically changes the behavior.

> The behavior described in the post is about registering startup script placed in the UpdatePanel.

When parameter `addScriptTags` is `true`, everything works as expected, at least to me. The passed script is encoded and registered inside `<script>` tag created by the ASP.NET. There is also no limit for the length of the script.

But strange things happen when to register startup script with the `false` value for parameter `addScriptTags`. Without changing anything in the script (except adding `<script>` tag) the page stop working. The end user doesn't see anything, except the missing update of the UpdatePanel. When you dig a little and open the browser developer console, you can see javascript error

```

```

also, if you are logging unhandled exceptions in the web application, you get

```

```

So, why does register startup script use JavascriptSerializer?
