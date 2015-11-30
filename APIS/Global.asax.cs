using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace APIS
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //隱藏ASP.NET MVC 送出的版本編號(資安考量:揭漏最少資訊原則)
            MvcHandler.DisableMvcResponseHeader = true;

            AreaRegistration.RegisterAllAreas(); //將區域(Area)的路由(~/Area/backstage/backstageAreaRegistration.cs)註冊進RouteTable.Route
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes); //將網站根目錄的路由(~/App_Start/RouteConfig.cs)註冊進RouteTable.Route
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
