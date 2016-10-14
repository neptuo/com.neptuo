using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neptuo.WebSite.Models
{
    public class MenuItem
    {
        public string Text { get; set; }
        public string Url { get; set; }
        public List<MenuItem> SubItems { get; set; }

        public MenuItem()
        {
            SubItems = new List<MenuItem>();
        }
    }
}