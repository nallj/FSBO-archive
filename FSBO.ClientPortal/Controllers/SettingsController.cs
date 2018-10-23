using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace FSBO.ClientPortal.Controllers
{
    public class SettingsController : Controller
    {

        private string envSettingsJs;

        public SettingsController(IConfiguration config)
        {
            // since IConfiguration can't be accessed in a static constructor we need to check 
            if (string.IsNullOrWhiteSpace(envSettingsJs))
            {
                envSettingsJs = RenderEnvSettingsJsContent(config);
            }
        }

        /// <summary>
        /// Gets Enivornment Settings as a javascript file that copies them to the variable window.envSettings;
        /// </summary>
        /// <returns></returns>
        public IActionResult Environment()
        {
            return Content(envSettingsJs, "application/javascript");
        }

        /// <summary>
        /// Creates JavaScript which puts usefull environment settings on the window obj
        /// </summary>
        private string RenderEnvSettingsJsContent(IConfiguration config)
        {
            var settings = new Dictionary<string, object>();
            settings.Add("apiPath", config["ApiPath"]);
            //settings.Add("insightsKey", config["ApplicationInsights:InstrumentationKey"]);
            //settings.Add("isProduction", SafeReadBool(config["IsProduction"]));
            //if (User?.Identity.IsAuthenticated ?? false)
            //{
            //    settings.Add("userName", User.Identity.Name);
            //}
            var serializedSettings = JsonConvert.SerializeObject(settings);

            return string.Format(@"var __envSettings; (function () {{ __envSettings = {0}; }})()", serializedSettings);

            //bool SafeReadBool(string str)
            //{
            //    bool value;
            //    return (bool.TryParse(str, out value)) ? value : false;
            //}
        }


    }
}
