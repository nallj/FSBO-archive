using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FSBO.DAL;
using FSBO.Installer.Clients;

namespace FSBO.Installer
{
    public class InstallerContext : IInstallerContext
    {
        private IWebServicesClient WebSvcClient;

        public InstallerContext(IWebServicesClient webSvcClient)
        {
            //WebSvcClient = webSvcClient ?? new WebServicesClient();
            WebSvcClient = webSvcClient;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="args"></param>
        public void WriteLine(string text, params object[] args)
        {
            if (args.Length > 0)
            {
                text = string.Format(text, args);
            }

            Console.WriteLine(text);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="args"></param>
        public void Warn(string text, params object[] args)
        {
            WriteLine("[WARN] " + text, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="isFatal"></param>
        /// <param name="args"></param>
        public void Error(string text, bool isFatal = false, params object[] args)
        {
            if (isFatal)
                WriteLine("[FATAL ERROR] " + text, args);
            else
                WriteLine("[ERROR] " + text, args);
        }

        /// <summary>
        /// Get Areas already present in the database using the Web Services Client.
        /// </summary>
        /// <returns>List of Area DAL objects that are stored already in the database.</returns>
        public async Task<List<Area>> GetAreas()
        {
            var clientResult = await WebSvcClient.GetAsync<List<Area>>();
            return clientResult;
        }
    }
}
