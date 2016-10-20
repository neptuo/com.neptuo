using Neptuo.AspNet.Navigation;
using Neptuo.WebSite.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neptuo.WebSite.Navigation
{
    [RouteName("Blog")]
    [RouteUrl("blog/{year}/{month}/{day}")]
    [RouteController("Content", nameof(ContentController.Blog))]
    [RouteDefault("year", null)]
    [RouteDefault("month", null)]
    [RouteDefault("day", null)]
    public class BlogRoute
    {
        public int? Year { get; private set; }
        public int? Month { get; private set; }
        public int? Day { get; private set; }

        public BlogRoute()
        { }

        public BlogRoute(int year)
        {
            Year = year;
        }

        public BlogRoute(int year, int month)
            : this(year)
        {
            Month = month;
        }

        public BlogRoute(int year, int month, int day)
            : this(year, month)
        {
            Day = day;
        }
    }
}