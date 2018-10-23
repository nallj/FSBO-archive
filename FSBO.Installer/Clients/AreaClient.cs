using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBO.Installer.Clients
{
    public class AreaClient
    {
        private IWebServicesClient WebSvcClient;

        public AreaClient(IWebServicesClient webSvcClient)
        {
            WebSvcClient = webSvcClient;
        }
    }
}
