using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Serialization;

namespace Neptuo.WebSite.Models.Projects
{
    [XmlType("Description")]
    public class ProjectVersionDescriptionModel
    {
        private static Regex issueRegex = new Regex("(#([0-9])+)", RegexOptions.Compiled);

        [XmlText]
        public string Content { get; set; }

        [XmlElement("Description")]
        public List<ProjectVersionDescriptionModel> Descriptions { get; private set; }

        public ProjectVersionDescriptionModel()
        {
            Descriptions = new List<ProjectVersionDescriptionModel>();
        }

        public string GetContent(string projectUrl)
        {
            if (projectUrl.Contains("github.com"))
                return issueRegex.Replace(Content, match => $"<a target='_blank' href='{projectUrl}/issues/{match.Groups[2].Value}'>{match.Groups[0].Value}</a>");

            return Content;
        }
    }
}