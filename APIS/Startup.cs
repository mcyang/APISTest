using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute("startup1", typeof(APIS.Startup))]
namespace APIS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
