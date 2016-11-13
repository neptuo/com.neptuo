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

        public ActionResult Home(bool isCzech = false)
        {
            WebDataService webDataService = new WebDataService(Request.MapPath(WebDataService.DataUri));
            ProjectDataService projectDataService = new ProjectDataService(Request.MapPath);
            PostDataService postDataService = new PostDataService(Request.MapPath(PostDataService.DataUri));

            HomeModel viewModel = new HomeModel(
                webDataService.Get().Take(6),
                projectDataService.Get().Take(10),
                postDataService.Get().First(p => p.Url == "website-introduction")
            );

            if (isCzech)
                return View("Home.cz", viewModel);

            return View(viewModel);
        }

        public ActionResult Project(string type, string project)
        {
            if (String.IsNullOrEmpty(type))
                return View("ProjectOverview");

            ProjectDataService dataService = new ProjectDataService(Request.MapPath);

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

        public ActionResult Blog(int? year, int? month, int? day)
        {
            PostDataService dataService = new PostDataService(Request.MapPath(PostDataService.DataUri));
            return View(dataService.Get(year, month, day));
        }

        public ActionResult BlogPost(int year, int month, int day, string slug)
        {
            PostDataService dataService = new PostDataService(Request.MapPath(PostDataService.DataUri));
            PostModel post = dataService.Find(new DateTime(year, month, day), slug);
            if (post == null)
                return View("NotFound");

            string content = dataService.GetContent(post, Request.MapPath);
            return View(new PostViewModel(post, content));
        }

        public ActionResult BlogAtom()
        {
            PostDataService dataService = new PostDataService(Request.MapPath(PostDataService.DataUri));
            return View(dataService.Get());
        }
    }
}
