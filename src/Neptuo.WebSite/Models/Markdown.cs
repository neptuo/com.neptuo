using CommonMark;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.WebSite.Models
{
    public static class Markdown
    {
        public static string Convert(string text)
        {
            CommonMarkSettings.Default.OutputDelegate = (doc, output, settings) =>
                        new HtmlFormatter(output, settings).WriteDocument(doc);

            string html = CommonMarkConverter.Convert(text);
            return html;
        }
    }
}
