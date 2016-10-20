using Neptuo.AspNet.Navigation;
using Neptuo.WebSite.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neptuo.WebSite.Navigation
{
    [RouteName("BlogPost")]
    [RouteUrl("blog/{year}/{month}/{day}/{slug}")]
    [RouteController("Content", nameof(ContentController.BlogPost))]
    public class BlogPostRoute
    {
        public int Year { get; private set; }
        public int Month { get; private set; }
        public int Day { get; private set; }
        public string Slug { get; private set; }

        public BlogPostRoute(DateTime dateTime, string slug)
        {
            Year = dateTime.Year;
            Month = dateTime.Month;
            Day = dateTime.Day;
            Slug = slug;
        }
    }
}