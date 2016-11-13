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
        public const string DataUri = "~/App_Data/";
        public const string SearchPattern = "Projects*.xml";

        private readonly List<ProjectModel> models = new List<ProjectModel>();

        public ProjectDataService(Func<string, string> pathResolver)
        {
            foreach (string filePath in Directory.EnumerateFiles(pathResolver(DataUri), SearchPattern))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(ProjectList));
                    ProjectList list = (ProjectList)serializer.Deserialize(reader);
                    models.AddRange(list);
                }
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