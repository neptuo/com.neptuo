using Neptuo.WebSite.Navigation;
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
            
            routes.MapModel<ProjectRoute>();
            routes.MapModel<BlogAtomRoute>();
            routes.MapModel<BlogPostRoute>();
            routes.MapModel<BlogRoute>();

            routes.MapRoute(
                name: "Home",
                url: "",
                defaults: new { controller = "Content", action = "Home" }
            );

            routes.MapRoute(
                name: "404",
                url: "404.html",
                defaults: new { controller = "Content", action = "Index" }
            );

            routes.MapRoute(
                name: "ContentFallBack",
                url: "{*path}",
                defaults: new { controller = "Content", action = "Index" }
            );
        }
    }
}