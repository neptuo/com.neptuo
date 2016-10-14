using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Neptuo.WebSite.Models.Projects
{
    [XmlType("Images")]
    public class ProjectImageModel
    {
        public string Main { get; set; }

        [XmlElement("Additional")]
        public List<string> Additional { get; private set; }

        public ProjectImageModel()
        {
            Additional = new List<string>();
        }
    }
}