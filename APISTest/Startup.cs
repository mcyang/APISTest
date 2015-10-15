using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(APISTest.Startup))]
namespace APISTest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
