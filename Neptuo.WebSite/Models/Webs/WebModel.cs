using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Neptuo.WebSite.Models.Webs
{
    [XmlType("Web")]
    [XmlRoot("Web")]
    public class WebModel
    {
        private const string HttpPregix = "http://";

        [XmlAttribute]
        public bool IsOld { get; set; }

        public string Title { get; set; }
        public string Link { get; set; }

        [XmlArray("Previews")]
        [XmlArrayItem("Preview")]
        public List<ImageModel> Previews { get; set; }

        public string LinkName
        {
            get
            {
                if (!String.IsNullOrEmpty(Link) && Link.StartsWith(HttpPregix))
                    return Link.Substring(HttpPregix.Length);

                return Link;
            }
        }

        [XmlArray("Descriptions")]
        [XmlArrayItem("Description")]
        public List<DescriptionItemModel> Descriptions { get; set; }
    }
}