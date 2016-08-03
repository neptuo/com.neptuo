## ScriptManager.RegisterStartupScript and behavior of the last argument addScriptTags

This blog post is about registering startup script in the ASP.NET WebForms. The method `ScriptManager.RegisterStartupScript` takes 5 arguments, four of them are quite obvious.
XXX. The last one dramatically changes the behavior.

> The behavior described in the post is about registering startup script placed in the UpdatePanel.

When parameter `addScriptTags` is `true`, everything works as expected, at least to me. The passed script is encoded and registered inside `&lt;script&gt;` tag created by the ASP.NET. There is also no limit for the length of the script.
