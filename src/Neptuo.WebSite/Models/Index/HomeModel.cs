using Neptuo.WebSite.Models.Blogging;
using Neptuo.WebSite.Models.Projects;
using Neptuo.WebSite.Models.Webs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neptuo.WebSite.Models.Index
{
    public class HomeModel
    {
        public IEnumerable<WebModel> Webs { get; set; }
        public IEnumerable<ProjectModel> Projects { get; set; }
        public PostModel BlogPost { get; set; }

        public HomeModel(IEnumerable<WebModel> webs, IEnumerable<ProjectModel> projects, PostModel blogPost)
        {
            Webs = webs;
            Projects = projects;
            BlogPost = blogPost;
        }
    }
}