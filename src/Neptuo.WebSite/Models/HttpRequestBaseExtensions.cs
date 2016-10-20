using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System.Web
{
    public static class HttpRequestBaseExtensions
    {
        public static bool IsActive(this HttpRequestBase request, string url, bool exact = false)
        {
            string targetUrl = request.AppRelativeCurrentExecutionFilePath;
            if (url.StartsWith("/"))
                targetUrl = request.RawUrl;

            if (exact)
                return targetUrl == url;
            else
                return targetUrl.StartsWith(url);
        }

        public static string ActiveCssClass(this HttpRequestBase request, string url, bool exact = false)
        {
            return IsActive(request, url, exact) ? "active" : null;
        }

        public static string ResolveUrl(this HttpRequestBase request, string url)
        {
            if (!url.StartsWith("~/"))
                return url;

            return VirtualPathUtility.ToAbsolute(url);
        }
    }
}