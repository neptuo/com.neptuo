using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Neptuo.WebSite.Models.Projects
{
    public class ProjectRelationModel
    {
        [XmlAttribute("Text")]
        public string Text { get; set; }

        [XmlAttribute("Url")]
        public string Url { get; set; }
    }
}