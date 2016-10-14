using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Neptuo.WebSite.Models.Projects
{
    [XmlType("Description")]
    public class ProjectVersionDescriptionModel
    {
        [XmlText]
        public string Content { get; set; }

        [XmlElement("Description")]
        public List<ProjectVersionDescriptionModel> Descriptions { get; private set; }

        public ProjectVersionDescriptionModel()
        {
            Descriptions = new List<ProjectVersionDescriptionModel>();
        }
    }
}