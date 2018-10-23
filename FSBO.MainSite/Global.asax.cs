using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Optimization;

namespace FSBO.MainSite
{
    public class MvcApplication : HttpApplication
    {

        protected void Application_Start()
        {
            //AreaRegistration.RegisterAllAreas();
            //GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //TelemetryConfiguration.Active.InstrumentationKey = WebConfigurationManager.AppSettings["ApplicationInsightsInstrumentationKey"];
        }

        protected void Application_Error(object sender, System.EventArgs e)
        {
            //Handle nonce exception.
            var ex = Server.GetLastError();
            /*if (ex.GetType() == typeof(OpenIdConnectProtocolInvalidNonceException)) // & HttpContext.Current.IsCustomErrorEnabled)
            {
                Server.ClearError();
                Response.Redirect("~/Home/NonceException");
            }*/
        }

    }
}
