using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Neptuo.WebSite.Models.Webs
{
    public class ImageModel
    {
        [XmlAttribute]
        public string Link { get; set; }
        [XmlAttribute]
        public int Width { get; set; }
        [XmlAttribute]
        public int Height { get; set; }

        public ImageModel()
        {
            Width = 127;
            Height = 90;
        }
    }
}