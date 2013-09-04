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

        public HomeModel(IEnumerable<WebModel> webs)
        {
            Webs = webs;
        }
    }
}