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
        public string Icon { get; set; }
        public string Description { get; set; }

        //[XmlArray("Text")]
        public ProjectTextModel Text { get; set; }
        public TextModel Text2 { get; set; }
        public string Type { get; set; }
        public string LocalUrl { get; set; }

        public string ProjectUrl { get; set; }

        public string DocumentationUrl { get; set; }
        public string DownloadUrl { get; set; }

        public string Licence { get; set; }
        public string LicenceUrl { get; set; }

        [XmlArray("Relations")]
        [XmlArrayItem("Relation")]
        public List<ProjectRelationModel> Relations { get; set; }

        public ProjectImageModel Images { get; set; }

        [XmlElement("Version")]
        //[XmlArray("Versions")]
        //[XmlArrayItem("Version")]
        public List<ProjectVersionModel> Versions { get; private set; }

        public ProjectModel()
        {
            Versions = new List<ProjectVersionModel>();
            Relations = new List<ProjectRelationModel>();
        }
    }
}