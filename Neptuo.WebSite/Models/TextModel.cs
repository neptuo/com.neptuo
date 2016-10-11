using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Neptuo.WebSite.Models
{
    public class TextModel
    {
        [XmlAttribute]
        public Language Language { get; set; }

        [XmlText]
        public string Content { get; set; }

        public string ContentToHtml()
        {
            switch (Language)
            {
                case Language.PlainText:
                case Language.Html:
                    return Content;
                case Language.Markdown:
                    return CommonMarkConverter.Convert(Content);
                default:
                    throw new NotSupportedException(Language.ToString());
            }
        }
    }

    public enum Language
    {
        PlainText,
        Markdown,
        Html
    }
}