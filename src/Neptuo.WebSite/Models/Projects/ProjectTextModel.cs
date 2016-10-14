using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Neptuo.WebSite.Models.Projects
{
    [XmlType("Text")]
    public class ProjectTextModel
    {
        [XmlElement("p")]
        public List<string> Paragraphs { get; private set; }

        public ProjectTextModel()
        {
            Paragraphs = new List<string>();
        }
    }
}