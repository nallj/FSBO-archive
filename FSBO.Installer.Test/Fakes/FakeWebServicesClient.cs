using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBO.Installer.Test.Fakes
{
    public class FakeWebServicesClient : Clients.IWebServicesClient
    {
        public object ClientResult { get; set; }

        //public bool GetAsyncWasCalled { get; set; }

        public FakeWebServicesClient()
        {

        }


        public async Task<TResult> GetAsync<TResult>()
        {
            return await Task.FromResult((TResult)ClientResult);
        }

        public async Task<TResult> PostAsync<TData, TResult>(TData data)
        {
            return await Task.FromResult((TResult)ClientResult);
        }
    }
}
