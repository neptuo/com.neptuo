## Creating route links and passing in values

I'm not using ASP.NET MVC as much and currently this web site is the only MVC project I'm working on. I was always using `new { ... }` for passing route values when creating links or URLs.

```Razor
@Html.ActionLink("View project detail", "Project", "Detail", new { ID = @project.ID })
```

A real fun started when I was trying to create a link to the detail of blog post. 

```Razor
@Html.RouteLink("View post detail", "BlogPost", new { 
    Year = post.PublishedAt.Year, 
    Month = post.PublishedAt.Month, 
    Day = post.PublishedAt.Day, 
    Slug = post.Slug })
```

Because we wanted 'nice' URLs containing year, month, day and post partial url, and our model contains release date as `DateTime`, I had to 'deconstruct' the DateTime object to separate fields. 

I don't like, but when it was used in a single place, it was ok. But this week we have introduced RSS feed and so we needed to generate links to posts again, but in different view. This lead to think, how can we unify this?

### 1) Create extension method of HtmlHelper.

This was our initial thought. Just create a `PostLink` extension method on the `HtmlHelper` and pass in the release date and partial post URL.

```Razor
@Html.PostLink("View post detail", post.PublishedAt, post.Slug)
```

Right after that, we would surely needed the same extension method on the `UrlHelper`.

### 2) Shared/partial/model view

We can also use display view for model property.

```Razor
@Html.DisplayFor(m => m.post, "PostLinkView")
```

And then create a cshtml view in the `~/Views/Shared/DisplayViews/PostListView.cshtml`. 

I think this is suitable for a link different scenario than the one we challenge here. This is best suited when a more complex HTML is needed, like when we want to share a whole template for the post preview.

![Blog post preview](/Content/Images/Blog/mvc-route-values/blog-post-preview.png)

### 3) Using model, aka C# class

Than we have realized that the object passed to the `RouteLink` or `RouteUrl` could be of any type, and that this type can even be a hand written class, not just anonymous class generated from `new { ... }`. So we have written a class.

```C#
public class BlogPostRouteValues
{
    public int Year { get; private set; }
    public int Month { get; private set; }
    public int Day { get; private set; }
    public string Url { get; private set; }

    public BlogPostRouteValues(DateTime dateTime, string url)
    {
        Year = dateTime.Year;
        Month = dateTime.Month;
        Day = dateTime.Day;
        Url = url;
    }
}
```

And used it in our views

```
@Url.RouteUrl("BlogPost", new BlogPostRouteValues(post.ReleaseDate, post.Url))
```

And we liked it. We liked it vary much, because we can use this class for both `HtmlHelper` methods and `UrlHelper` methods, because it is nothing more then route values class.

## Backgrounds of the AspNet (model) Navigation project

The hand written route values class inspired us to create a small NuGet package with support for creating links and urls, registering routes and do navigation in MVC projects using classes.

We have realized that a name of the route could be part of the class, as const field or using custom attribute. Also the route URL could be placed here and default values too. So we have added these fields to our `BlogPostRouteValues`.

```C#
public class BlogPostRouteValues
{
    public const string RouteName = "BlogPost";
    public const string RouteTemplate = "blog/{year}/{month}/{day}/{slug}";
    public static readonly object Defaults = new { Controller = "Content", Action = "BlogPost" };
    
    ...
}
```

With this class, we can register the route in our `RouteConfig.cs`.

```C#
routes.MapRoute(
    name: BlogPostRouteValues.RouteName,
    url: BlogPostRouteValues.RouteTemplate,
    defauls: BlogPostRouteValues.Defaults
);
```

With a bit of reflection, this can be even simplified to an extension method of `RouteCollection` that requires only type of the route values class.

```
public static Route MapModel<TModel>(this RouteCollection routes)
{
    FieldInfo routeNameFieldInfo = typeof(TModel).GetField("RouteName", BindingFlags.Static | BindingFlags.Public);
    FieldInfo urlFieldInfo = typeof(TModel).GetField("RouteTemplate", BindingFlags.Static | BindingFlags.Public);
    FieldInfo defaultsFieldInfo = typeof(TModel).GetField("Defaults", BindingFlags.Static | BindingFlags.Public);

    string routeName = (string)routeNameFieldInfo.GetRawConstantValue();
    string url = (string)urlFieldInfo.GetRawConstantValue();
    object defaults = defaultsFieldInfo.GetValue(null);
    return routes.MapRoute(routeName, url, defaults);
}
```

The registration is now as simple as this single line of code.
 
 ```C#
 routes.MapModel<BlogPostRouteValues>();
 ```
 
Now, we have finished the route registration, as I'm not sure if it can be simplified even more.

 > There should some `null` checkes and because not all of these fields are required, the route can be registered with only one static field, the `RouteTemplate`. But lets skip this for now.

### Creating links and URLs

Because route values class now contains both actual route values and name of the route to use, we were able to create another simple extension method, this time on `HtmlHelper` and `UrlHelper`.
 
```C#
public static string ModelUrl(this UrlHelper url, object route)
{
    FieldInfo fieldInfo = route.GetType().GetField("RouteName", BindingFlags.Static | BindingFlags.Public);
    string routeName = (string)fieldInfo.GetRawConstantValue();
    return url.RouteUrl(routeName, route);
}

public static MvcHtmlString ModelLink(this HtmlHelper html, string linkText, object route)
{
    FieldInfo fieldInfo = route.GetType().GetField("RouteName", BindingFlags.Static | BindingFlags.Public);
    string routeName = (string)fieldInfo.GetRawConstantValue();
    return html.RouteLink(linkText, routeName, route);
}
```
 
> Also here the `RouteName` is not required and the reflection can be skipped.

Now we can use these methods for creating links and URLs.

```Razor
@Html.ModelLink("View post detail", new BlogPostRouteValues(post.ReleaseDate, post.Url))
...
@Url.ModelUrl(new BlogPostRouteValues(post.ReleaseDate, post.Url))
```

This really awsome and simple and it only costs create route values class. Such class shouldn't even contain read-only properties and constructor, it could be simple anemic POCO. So there aren't too much LoC introduced in this pattern.
