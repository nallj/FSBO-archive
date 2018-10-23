using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

using FSBO.WebServices.Services;
using FSBO.WebServices.Models.Scraping;

namespace FSBO.WebServices.Controllers
{
    [Route("api/[controller]")]
    public class AreaController : Controller
    {
        private IAreaService AreaSvc;

        public AreaController(IAreaService areaSvc)
        {
            AreaSvc = areaSvc;
        }


        [HttpGet("AreaTypes")]
        public IEnumerable<AreaType> GetAreaTypes()
        {
            var serviceResponse = AreaSvc.GetAreaTypes();
            return serviceResponse;
        }

        [HttpPost("AreaTypes")]
        public async Task<int> PostAreaType([FromBody] AreaType areaType)
        {
            return await AreaSvc.AddAreaType(areaType);
        }

        [HttpGet("Areas")]
        public IEnumerable<AreaWithEntries> GetAreas()
        {
            var serviceResponse = AreaSvc.GetAreas();
            return serviceResponse;
        }

        [HttpGet("Areas/{areaId}")]
        public async Task<AreaWithEntries> GetAreaById(int areaId)
        {
            var serviceResponse = await AreaSvc.GetAreaById(areaId);
            return serviceResponse;
        }

        /*[HttpPost("Areas")]
        public async Task<int> PostArea([FromBody] Area area)
        {
            var serviceResponse = await AreaSvc.AddArea(area);
            return serviceResponse;
        }
        */
    }
}