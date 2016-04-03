using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Neptuo.WebSite.Models.Projects
{
    [XmlType("Project")]
    [XmlRoot("Project")]
    public class ProjectModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string LocalUrl { get; set; }

        public string ProjectUrl { get; set; }

        public string DocumentationUrl { get; set; }

        public string Licence { get; set; }
        public string LicenceUrl { get; set; }

        //[XmlArray("Versions")]
        //[XmlArrayItem("Version")]
        //public ProjectVersionModel Versions { get; set; }
    }
}