﻿using Neptuo.WebSite.Models.Index;
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
            return View(new HomeModel(webDataService.Get().Take(6)));
        }

        public ActionResult Services()
        {
            WebDataService webDataService = new WebDataService(Request.MapPath(WebDataService.DataUri));
            return View(webDataService.Get());
        }
    }
}
