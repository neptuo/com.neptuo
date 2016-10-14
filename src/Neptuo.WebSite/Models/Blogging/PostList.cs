using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Neptuo.WebSite.Models.Blogging
{
    [XmlRoot("Posts", Namespace = "http://schemas.neptuo.com/xsd/neptuo-website-blog.xsd")]
    public class PostList : List<PostModel>
    { }
}