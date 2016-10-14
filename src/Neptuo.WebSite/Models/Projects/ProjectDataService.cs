using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Neptuo.WebSite.Models.Projects
{
    public class ProjectDataService
    {
        public const string DataUri = "~/App_Data/Projects.xml";

        private ProjectList models;

        public ProjectDataService(string dataUri)
        {
            using (StreamReader reader = new StreamReader(dataUri))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ProjectList));
                models = (ProjectList)serializer.Deserialize(reader);
            }
        }

        public IEnumerable<ProjectModel> Get()
        {
            return models.OrderBy(p => p.Name);
        }
    }

    [XmlRoot("Projects", Namespace = "http://schemas.neptuo.com/xsd/neptuo-website-projects.xsd")]
    public class ProjectList : List<ProjectModel> 
    { }
}