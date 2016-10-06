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

### 2)
