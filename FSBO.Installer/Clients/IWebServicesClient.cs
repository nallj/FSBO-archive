using System.Threading.Tasks;

namespace FSBO.Installer.Clients
{
    public interface IWebServicesClient
    {
        Task<TResult> GetAsync<TResult>();
        Task<TResult> PostAsync<TData, TResult>(TData data);
    }
}
