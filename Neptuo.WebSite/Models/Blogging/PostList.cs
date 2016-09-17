using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Neptuo.WebSite.Models.Blogging
{
    [XmlRoot("Posts")]
    public class PostList : List<PostModel>
    { }
}