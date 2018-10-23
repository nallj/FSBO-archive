using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FSBO.Application.Startup))]
namespace FSBO.Application
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
