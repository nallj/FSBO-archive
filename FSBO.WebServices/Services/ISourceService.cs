using System.Threading.Tasks;
using System.Collections.Generic;

using FSBO.DAL;

namespace FSBO.WebServices.Services
{
    public interface ISourceService
    {
        // Source methods
        IEnumerable<Source> GetSources();
        Task<int> AddSource(Source source);
    }
}
