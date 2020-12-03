using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Neptuo.WebSite.Models.Projects
{
    [XmlType("Version")]
    public class ProjectVersionModel
    {
        public string Name { get; set; }
        public string TagName { get; set; }
        public string DownloadUrl { get; set; }

        [XmlElement("Description")]
        public List<ProjectVersionDescriptionModel> Descriptions { get; private set; }

        public ProjectVersionModel()
        {
            Descriptions = new List<ProjectVersionDescriptionModel>();
        }
    }
}