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
            return models;
        }
    }

    [XmlRoot("Projects")]
    public class ProjectList : List<ProjectModel> 
    { }
}