using Neptuo.WebSite.Models.Blogging;
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
            if (String.IsNullOrEmpty(type))
                return View("ProjectOverview");

            ProjectDataService dataService = new ProjectDataService(Request.MapPath(ProjectDataService.DataUri));

            ProjectModelViewModel viewModel = new ProjectModelViewModel()
            {
                IsDetail = !String.IsNullOrEmpty(project)
            };

            viewModel.AddRange(dataService
                .Get()
                .Where(p => String.IsNullOrEmpty(type) || p.Type.ToLowerInvariant() == type.ToLowerInvariant())
                .Where(p => String.IsNullOrEmpty(project) || p.LocalUrl.ToLowerInvariant() == project.ToLowerInvariant())
            );

            return View(viewModel);
        }

        public ActionResult Service()
        {
            WebDataService dataService = new WebDataService(Request.MapPath(WebDataService.DataUri));
            return View(dataService.Get());
        }

        public ActionResult Blog(DateTime? releaseDate, string slug)
        {
            PostDataService dataService = new PostDataService(Request.MapPath(PostDataService.DataUri));

            if (releaseDate == null || slug == null)
                return View(dataService.Get());

            PostModel post = dataService.Find(releaseDate.Value, slug);
            if (post == null)
                return View("NotFound");

            string content = dataService.GetContent(post, Request.MapPath);
            return View("BlogPost", new PostViewModel(post, ));
        }
    }
}
