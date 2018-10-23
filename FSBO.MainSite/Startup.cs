using Owin;
using Microsoft.Owin;

[assembly: OwinStartup(typeof(FSBO.MainSite.Startup))]

namespace FSBO.MainSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
