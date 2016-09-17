using Neptuo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Neptuo.WebSite.Models.Blogging
{
    public class PostDataService
    {
        public const string DataUri = "~/App_Data/Blog.xml";

        private IEnumerable<PostModel> models;

        public PostDataService(string dataUri)
        {
            using (StreamReader reader = new StreamReader(dataUri))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(PostList));
                models = (PostList)serializer.Deserialize(reader);
            }
        }

        public IEnumerable<PostModel> Get()
        {
            return models;
        }

        public PostModel Find(DateTime releaseDate, string url)
        {
            releaseDate = releaseDate.Date;
            url = url.ToLowerInvariant();

            return models
                .Where(p => p.ReleaseDate == releaseDate && p.Url == url)
                .FirstOrDefault();
        }

        public string GetContent(PostModel model, Func<string, string> pathMapper)
        {
            string path = pathMapper(model.FilePath);



            return null;
        }
    }
}