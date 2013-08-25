using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace Neptuo.WebSite.Models.Webs
{
    public class WebDataService
    {
        public const string DataUri = "~/App_Data/Webs.xml";
        private WebList models;

        public WebDataService(string dataUri)
        {
            using (StreamReader reader = new StreamReader(dataUri))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(WebList));
                models = (WebList)serializer.Deserialize(reader);
            }
        }

        public IEnumerable<WebModel> Get()
        {
            return models;
        }
    }

    [XmlRoot("Webs")]
    public class WebList : List<WebModel> 
    { }
}