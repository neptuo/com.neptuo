using Neptuo.AspNet.Navigation;
using Neptuo.WebSite.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neptuo.WebSite.Navigation
{
    [RouteName("BlogAtom")]
    [RouteUrl("blog/atom.xml")]
    [RouteController("Content", nameof(ContentController.BlogAtom))]
    public class BlogAtomRoute
    { }
}