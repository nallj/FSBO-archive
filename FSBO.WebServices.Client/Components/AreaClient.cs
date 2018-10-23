using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBO.WebServices.Client.Components
{
    public class AreaClient : IAreaClient
    {
        private IWebServicesClient WebSvcClient;

        public AreaClient(IWebServicesClient webSvcClient)
        {
            WebSvcClient = webSvcClient;
        }


    }
}
