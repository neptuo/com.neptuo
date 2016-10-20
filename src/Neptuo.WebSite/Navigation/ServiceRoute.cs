using Neptuo.AspNet.Navigation;
using Neptuo.WebSite.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neptuo.WebSite.Navigation
{
    [RouteName("Service")]
    [RouteUrl("service")]
    [RouteController("Content", nameof(ContentController.Service))]
    public class ServiceRoute
    {
    }
}