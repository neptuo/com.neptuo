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

Than we have realized that do object, that is passed to the `RouteLink` or `RouteUrl` could be of any type, and that this type can be hand written class, not just anonymous class generated from `new { ... }`. So we have written a class

```C#
public class BlogPostRouteValues
{
    public int Year { get; private set; }
    public int Month { get; private set; }
    public int Day { get; private set; }
    public string Url { get; private set; }

    public BlogPostRoute(DateTime dateTime, string url)
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
@Url.RouteUrl("BlogPost", new BlogPostRoute(post.ReleaseDate, post.Url))
```

And we liked it. We liked it vary much, because we can use this class for both `HtmlHelper` methods and `UrlHelper` methods, because it is nothing more then route values class.
