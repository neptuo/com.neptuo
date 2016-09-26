using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neptuo.WebSite.Models.Blogging
{
    public class PostViewModel
    {
        public PostModel Post { get; private set; }
        public string HtmlContent { get; private set; }

        public string Title
        {
            get { return Post.Title; }
        }

        public string Description
        {
            get { return Post.Description; }
        }

        public PostViewModel(PostModel model, string content)
        {
            Post = model;
            HtmlContent = content;
        }
    }
}