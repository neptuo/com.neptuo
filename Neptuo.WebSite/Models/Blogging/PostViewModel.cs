using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neptuo.WebSite.Models.Blogging
{
    public class PostViewModel
    {
        public PostModel Model { get; private set; }
        public string Content { get; private set; }

        public string Title
        {
            get { return Model.Title; }
        }

        public string Description
        {
            get { return Model.Description; }
        }

        public PostViewModel(PostModel model, string content)
        {
            Model = model;
            Content = content;
        }
    }
}