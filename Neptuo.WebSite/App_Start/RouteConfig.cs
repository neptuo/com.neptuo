using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Neptuo.WebSite
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //Old urls


            //Custom routes

            routes.MapRoute(
                name: "Service",
                url: "service",
                defaults: new { controller = "Content", action = "Service" }
            );

            routes.MapRoute(
                name: "Project",
                url: "project/{type}/{project}",
                defaults: new { controller = "Content", action = "Project", type = (string)null, project = (string)null }
            );

            routes.MapRoute(
                name: "BlogPost",
                url: "blog/{year}/{month}/{day}/{slug}",
                defaults: new { controller = "Content", action = "Blog", year = (int?)null, month = (int?)null, day = (int?)null, slug = (string)null }
            );

            routes.MapRoute(
                name: "Home",
                url: "",
                defaults: new { controller = "Content", action = "Home" }
            );

            routes.MapRoute(
                name: "ContentFallBack",
                url: "{*path}",
                defaults: new { controller = "Content", action = "Index" }
            );
        }
    }
}