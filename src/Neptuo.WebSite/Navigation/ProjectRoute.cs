using Neptuo;
using Neptuo.AspNet.Navigation;
using Neptuo.WebSite.Controllers;
using Neptuo.WebSite.Models.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neptuo.WebSite.Navigation
{
    [RouteName("Project")]
    [RouteUrl("project/{type}/{*project}")]
    [RouteController("Content", nameof(ContentController.Project))]
    [RouteDefault("type", null)]
    [RouteDefault("project", null)]
    public class ProjectRoute
    {
        public string Type { get; private set; }
        public string Project { get; private set; }

        public ProjectRoute()
        { }

        public ProjectRoute(ProjectModel model)
        {
            Ensure.NotNull(model, "model");
            Type = model.Type;
            Project = model.LocalUrl;
        }

        public static ProjectRoute All()
        {
            return new ProjectRoute()
            {
                Type = "all"
            };
        }
    }
}