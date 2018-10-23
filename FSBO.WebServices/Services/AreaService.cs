using AutoMapper;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper.QueryableExtensions;

using FSBO.WebServices.Models.Scraping;

namespace FSBO.WebServices.Services
{
    public class AreaService : IAreaService
    {
        private DAL.IFsboContext DbContext;

        public AreaService(DAL.IFsboContext dbContext)
        {
            DbContext = dbContext ?? new DAL.FsboContext();
        }


        public IEnumerable<AreaType> GetAreaTypes()
        {
            var areaTypes = DbContext.AreaTypes.ProjectTo<AreaType>();
            return areaTypes;
        }

        public async Task<int> AddAreaType(AreaType areaType)
        {
            var dbAreaType = Mapper.Map<DAL.AreaType>(areaType);
            DbContext.AreaTypes.Add(dbAreaType);
            await DbContext.SaveChangesAsync();

            return areaType.AreaTypeId;
        }

        public IEnumerable<AreaWithEntries> GetAreas()
        {
            var areasWithEntries = DbContext.Areas.ProjectTo<AreaWithEntries>();
            return areasWithEntries;
        }

        public async Task<AreaWithEntries> GetAreaById(int areaId)
        {
            var dbArea = await DbContext.Areas.FirstOrDefaultAsync(a => a.AreaId == areaId);
            var area = Mapper.Map<AreaWithEntries>(dbArea);

            return area;
        }

        /*public async Task<int> AddArea(Area area)
        {
            DbContext.Areas.Add(area);
            await DbContext.SaveChangesAsync();

            return area.AreaId;
        }*/

    }
}