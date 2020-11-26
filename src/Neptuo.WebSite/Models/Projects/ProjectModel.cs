using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Neptuo.WebSite.Models.Projects
{
    [XmlType("Project")]
    [XmlRoot("Project")]
    public class ProjectModel
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }

        public string PrivacyPolicy { get; set; }

        public TextModel Text { get; set; }
        public string Type { get; set; }
        public string LocalUrl { get; set; }

        public string ProjectUrl { get; set; }

        public string DocumentationUrl { get; set; }
        public string DownloadUrl { get; set; }
        public string VsixGalleryId { get; set; }
        public string NugetPackageId { get; set; }

        public string Licence { get; set; }
        public string LicenceUrl { get; set; }

        public string BuildStatusUrl { get; set; }
        public string BuildStatusBadge { get; set; }

        [XmlArray("Relations")]
        [XmlArrayItem("Relation")]
        public List<ProjectRelationModel> Relations { get; set; }

        public ProjectImageModel Images { get; set; }

        [XmlElement("Version")]
        //[XmlArray("Versions")]
        //[XmlArrayItem("Version")]
        public List<ProjectVersionModel> Versions { get; private set; }

        public ProjectModel()
        {
            Versions = new List<ProjectVersionModel>();
            Relations = new List<ProjectRelationModel>();
        }

        public string GetPrivacyPolicy(Func<string, string> pathResolver)
        {
            string absolutePath = pathResolver(Path.Combine("~/App_data", PrivacyPolicy));
            if (File.Exists(absolutePath))
            {
                string content = Markdown.Convert(File.ReadAllText(absolutePath));
                content = content.Replace("{Project.Name}", Name);
                return content;
            }

            return String.Empty;
        }
    }
}