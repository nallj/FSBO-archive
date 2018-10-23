using System.Threading.Tasks;
using System.Collections.Generic;

using FSBO.WebServices.Models.Scraping;

namespace FSBO.WebServices.Services
{
    public interface IAreaService
    {
        // Area Type methods
        IEnumerable<AreaType> GetAreaTypes();
        Task<int> AddAreaType(AreaType areaType);

        // Area methods
        IEnumerable<AreaWithEntries> GetAreas();
        Task<AreaWithEntries> GetAreaById(int areaId);
        //Task<int> AddArea(Area area);
    }
}