using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

using FSBO.DAL;
using FSBO.WebServices.Services;

namespace FSBO.WebServices.Controllers
{
    [Route("api/[controller]")]
    public class SourceController : Controller
    {
        private ISourceService SrcSvc;

        public SourceController(ISourceService srcSvc)
        {
            SrcSvc = srcSvc;
        }


        /*[HttpGet("Sources")]
        public IEnumerable<Source> GetSources()
        {
            return SrcSvc.GetSources();
        }

        [HttpPost("Sources")]
        public async Task<int> AddSource(Source source)
        {
            return await SrcSvc.AddSource(source);
        }*/

    }
}