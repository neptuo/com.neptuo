using Neptuo.WebSite.Models.Index;
using Neptuo.WebSite.Models.Projects;
using Neptuo.WebSite.Models.Webs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Neptuo.WebSite.Controllers
{
    public class ContentController : Controller
    {
        public ActionResult Index()
        {
            string pathValue = (RouteData.Values["path"] ?? String.Empty).ToString();
            if (String.IsNullOrEmpty(pathValue))
                pathValue = "Hello";

            if (!System.IO.File.Exists(Request.MapPath(Path.Combine("~/Views/Content", pathValue + ".cshtml"))))
                pathValue = "NotFound";

            return View(pathValue);
        }

        public ActionResult Home()
        {
            WebDataService webDataService = new WebDataService(Request.MapPath(WebDataService.DataUri));
            ProjectDataService projectDataService = new ProjectDataService(Request.MapPath(ProjectDataService.DataUri));
            return View(new HomeModel(webDataService.Get().Take(6), projectDataService.Get().Take(10)));
        }

        public ActionResult Project(string type, string project)
        {
            ProjectDataService dataService = new ProjectDataService(Request.MapPath(ProjectDataService.DataUri));
            return View(dataService
                .Get()
                .Where(p => String.IsNullOrEmpty(type) || p.Type.ToLowerInvariant() == type.ToLowerInvariant())
                .Where(p => String.IsNullOrEmpty(project) || p.LocalUrl.ToLowerInvariant() == project.ToLowerInvariant())
            );
        }

        public ActionResult Service()
        {
            WebDataService webDataService = new WebDataService(Request.MapPath(WebDataService.DataUri));
            return View(webDataService.Get());
        }
    }
}
