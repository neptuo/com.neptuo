using CommonMark;
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

        public IEnumerable<PostModel> Get(int? year, int? month, int? day)
        {
            IEnumerable<PostModel> result = models;
            if (year != null)
            {
                result = result.Where(p => p.ReleaseDate.Year == year.Value);

                if (month != null)
                {
                    result = result.Where(p => p.ReleaseDate.Month == month.Value);

                    if (day != null)
                        result = result.Where(p => p.ReleaseDate.Day == day.Value);
                }
            }

            return result.OrderByDescending(p => p.ReleaseDate);
        }

        public PostModel Find(int year, int month, string url)
        {
            url = url.ToLowerInvariant();

            return models
                .Where(p => p.ReleaseDate.Year == year && p.ReleaseDate.Month == month && p.Url == url)
                .FirstOrDefault();
        }

        public string GetContent(PostModel model, Func<string, string> pathMapper)
        {
            string path = pathMapper(model.FilePath);
            string fileContent = File.ReadAllText(path);

            CommonMarkSettings.Default.OutputDelegate = (doc, output, settings) =>
                new HtmlFormatter(output, settings).WriteDocument(doc);

            string html = CommonMarkConverter.Convert(fileContent);
            return html;
        }
    }
}