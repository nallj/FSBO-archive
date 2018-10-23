using System;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;

using FSBO.Installer.Installers;

namespace FSBO.Installer
{
    public class Runner
    {
        public IInstallerContext InstallContext { get; set; }

        void Main(string[] args)
        {
            Console.Write("Are you sure now is the best time for an installation? Y/N  ");
            var answer = Console.ReadKey();

            if (answer.Key == ConsoleKey.Y)
            {
                var apiUri = ConfigurationManager.AppSettings["WebServicesUri"];

                var webSvcClient = new Clients.WebServicesClient(apiUri);
                InstallContext = new InstallerContext(webSvcClient);
                
                PerformAreaInstallation();
            }
            else if (answer.Key == ConsoleKey.N)
            {
                // Don't do stuff.
                Console.WriteLine("\n\nThen why did you bother me, man?");
            }
            else
            {
                Console.WriteLine("\n\nThat's not an option, you ass.");
            }

        }

        private void PerformAreaInstallation()
        {
            var areaInstaller = new AreaInstaller(InstallContext);
            areaInstaller.Install();
            //var existingDbAreas = InstallContext.GetAreas();
            //var areasOnFile = InstallContext.
        }
    }
}
