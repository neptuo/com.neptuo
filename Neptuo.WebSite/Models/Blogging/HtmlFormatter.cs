using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using CommonMark;
using CommonMark.Syntax;
using Skybrud.SyntaxHighlighter;

namespace Neptuo.WebSite.Models.Blogging
{
    public class HtmlFormatter : CommonMark.Formatters.HtmlFormatter
    {
        public HtmlFormatter(TextWriter target, CommonMarkSettings settings)
            : base(target, settings)
        { }

        protected override void WriteBlock(Block block, bool isOpening, bool isClosing, out bool ignoreChildNodes)
        {
            string content;
            if (block.Tag == BlockTag.FencedCode && TryFormatCode(block.FencedCodeData.Info, block.StringContent, out content))
            {
                Write(content);
                ignoreChildNodes = true;
            }
            else
            {
                base.WriteBlock(block, isOpening, isClosing, out ignoreChildNodes);
            }
        }

        private bool TryFormatCode(string language, StringContent content, out string result)
        {
            string source = content.ToString();
            switch (language.ToLowerInvariant())
            {
                case "xml":
                    result = Highlighter.HighlightXml(source);
                    return true;
                case "javascript":
                case "js":
                    result = Highlighter.HighlightJavaScript(source);
                    return true;
                case "json":
                    result = Highlighter.HighlightJson(source);
                    return true;
                case "csharp":
                case "c#":
                    result = Highlighter.HighlightCSharp(source);
                    return true;
            }

            result = null;
            return false;
        }
    }
}