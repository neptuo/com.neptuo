using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Neptuo.WebSite.Models.Blogging
{
    [XmlType("Post")]
    [XmlRoot("Post")]
    public class PostModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string FilePath { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Author { get; set; }

        public string GitHubCommentPath { get; set; }
    }
}