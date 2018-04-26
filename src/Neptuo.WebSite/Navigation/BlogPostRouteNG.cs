using Neptuo.AspNet.Navigation;
using Neptuo.WebSite.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neptuo.WebSite.Navigation
{
    [RouteName("BlogPostNG")]
    [RouteUrl("blog/{year}/{month}/{slug}")]
    [RouteController("Content", nameof(ContentController.BlogPost))]
    public class BlogPostRouteNG
    {
        public int Year { get; private set; }
        public string Month { get; private set; }
        public string Slug { get; private set; }

        public BlogPostRouteNG(DateTime dateTime, string slug)
        {
            Year = dateTime.Year;
            Month = dateTime.Month.ToString("D2");
            Slug = slug;
        }
    }
}