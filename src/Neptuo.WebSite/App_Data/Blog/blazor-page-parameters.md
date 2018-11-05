> [Blazor](https://github.com/aspnet/Blazor) is an experimental Web UI framework from Microsoft. There are plenty of posts about so I'm not going to describe it and I'm directly heading to my topic.

Today I was implementing [URL update when month is selected in Money summary](https://github.com/maraf/Money/issues/124). 

![Money Summary](/Content/Images/Blog/blazor-page-parameters/tabs.png)

Idea is that when a user selects a tab, it updates the URL, and when he navigates back and forward, it works like the "web".

![Money Summary](/Content/Images/Blog/blazor-page-parameters/tabs-done.png)

I was scared to use a standard link, because I though that it will re-render the whole page. 

> This page consists of a query for loading a list of tabs (months) and a query to load tab content (month summary). I did not wanted to load list of months, because they are right there, alread loaded.

By a mistake I created a standard link and passed click on it to the Blazor. And nothing happend. After a while I found that I was loading data in `OnInitAsync` and that this method wasn't invoked.

So I tried to move data loading into `OnParametersSetAsync` and it started to work! When a page links to itself, Blazor doesn't create a new instance of page/component, it reuses the current one and passes it a new set of parameters.

### A small bug

There is a small bug the current version (v0.6.0). The parameters are not cleared when they are not present in a new link. 
Let's show it on a sample. I have two routes in the summary page:

```
@page "/"
@page "/{Year}/{Month}"

```

A second one bind parameters to codebehind properties:

```C#
[Parameter]
protected string Year { get; set; }

[Parameter]
protected string Month { get; set; }
```

When a user navigates to a link with paramters set (eg. `/2018/10`) and than to a link without them (eg. `/`), Blazor doesn't clear values from `Year` and `Month` properties.
A current workaround is to clear these properties before parameter binding.

```C#
public override void SetParameters(ParameterCollection parameters)
{
    // Clear previous parameter values.
    Year = null;
    Month = null;

    base.SetParameters(parameters);
}
```

## Related files:

- [Summary.cshtml](https://github.com/maraf/Money/blob/master/src/Money.UI.Blazor/Pages/Summary.cshtml) - UI for the summary page.
- [SummaryBase.cs](https://github.com/maraf/Money/blob/master/src/Money.UI.Blazor/Pages/SummaryBase.cs) - Codebehind for the summary page.
