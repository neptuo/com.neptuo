using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Neptuo.WebSite.Models.Webs
{
    [XmlType("Description")]
    [XmlRoot("Description")]
    public class DescriptionItemModel
    {
        public string Cs { get; set; }
        public string En { get; set; }
    }
}