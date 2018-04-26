> [Blazor](https://github.com/aspnet/Blazor) is an experimental Web UI framework from Microsoft. There are plenty of posts about so I'm not going to describe it and I'm directly heading to my topic.

When you create an empty Blazor application from template and run it, it simply shows "Loading..." while downloading whole the application to the browser. As it could be a (very) few megabytes, so it can be shown for a little while.

![Basic loading screen](/Content/Images/Blog/blazor-boot-screen/basic.png)

It doesn't look great and it can be very easily fixed/enhanced. This "Loading..." text comes from `index.html` in your project. The pre-generated one looks like this:

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title>BlazorHosted.CSharp</title>
    <base href="/" />
    <link href="css/bootstrap/bootstrap.min.css" rel="stylesheet" />
    <link href="css/site.css" rel="stylesheet" />
</head>
<body>
    <app>Loading...</app>

    <script src="css/bootstrap/bootstrap-native.min.js"></script>
    <script type="blazor-boot"></script>
</body>
</html>
```

As you can see, the text is placed in `<app>` tags. This element is replaced once Blazor is bootstrapped. Anything you place between these tags will be shown between first page load and Blazor bootstrap finish.

In our [Money - experimental outcome manager](http://money.neptuo.com) - we have replaced the default text with this one.

![Money loading screen](/Content/Images/Blog/blazor-boot-screen/money.png)

Once bootstrap is finished and an application is loaded, it smoothly transitions to full layout.

![Money loaded screen](/Content/Images/Blog/blazor-boot-screen/money-loaded.png)

The source code ([taken from github](https://github.com/maraf/Money/blob/master/src/Money.UI.Blazor/wwwroot/index.html)) looks like this:

```html
...
<body>
    <app>
        <nav class="navbar navbar-inverse navbar-fixed-top">
            <div class="container">
                <div class="navbar-header">
                    <a class="navbar-brand">Money</a>
                </div>
            </div>
        </nav>
        <div class="container body-content">
            <p>
                <h1 class="center">Loading...</h1>
            </p>
            <hr />
            <footer>
                <p>&copy; 2018 - Money</p>
            </footer>
        </div>
    </app>
...
```

It is a simplified copy of Blazor main layout from [_layout.cshtml](https://github.com/maraf/Money/blob/master/src/Money.UI.Blazor/Shared/_Layout.cshtml) and also server side [_layout.cshtml](https://github.com/maraf/Money/blob/master/src/Money.UI.Backend/Views/Shared/_Layout.cshtml).

### A side note on replacing &lt;app&gt; tag

This tag is not replaced magically. The replacement is defined in `Program.cs` in method `Main` at the last line from template.

```C#
public class Program
{
    static void Main(string[] args)
    {
        var serviceProvider = new BrowserServiceProvider(configure =>
        {
            // Add any custom services here
        });

        new BrowserRenderer(serviceProvider).AddComponent<App>("app");
    }
}
```

## Summary

That's it. It's this simple to create more eye-catchy booting screens.