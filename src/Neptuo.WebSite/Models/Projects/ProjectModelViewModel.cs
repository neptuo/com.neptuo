using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neptuo.WebSite.Models.Projects
{
    public class ProjectModelViewModel : List<ProjectModel>
    {
        public bool IsDetail { get; set; }
    }
}