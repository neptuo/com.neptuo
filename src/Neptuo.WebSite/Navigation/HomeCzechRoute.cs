using Neptuo.AspNet.Navigation;
using Neptuo.WebSite.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neptuo.WebSite.Navigation
{
    [RouteName("HomeCzech")]
    [RouteUrl("cesky")]
    [RouteController("Content", nameof(ContentController.Home))]
    [RouteDefault("IsCzech", true)]
    public class HomeCzechRoute
    {
    }
}