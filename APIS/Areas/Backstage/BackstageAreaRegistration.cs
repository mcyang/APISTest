using System.Web.Mvc;

namespace APIS.Areas.Backstage
{
    //定義網址路由的，可以比對App_Start\RouteConfig.cs，可以看到定義路由的方法極為類似。
    public class BackstageAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Backstage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Backstage_main",
                "Backstage/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}