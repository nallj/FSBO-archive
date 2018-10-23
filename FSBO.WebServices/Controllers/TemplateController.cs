using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

using FSBO.WebServices.Services;
using FSBO.WebServices.Models.Scraping;

namespace FSBO.WebServices.Controllers
{
    [Route("api/[controller]")]
    public class TemplateController : Controller
    {
        private ITemplateService TempSvc;

        public TemplateController(ITemplateService tempSvc)
        {
            TempSvc = tempSvc;
        }


        [HttpGet("Templates")]
        public IEnumerable<TemplateInstructions> GetTemplates()
        {
            return TempSvc.GetTemplates();
        }

        [HttpPost("Templates")]
        public async Task<int> AddTemplate(Template template)
        {
            return await TempSvc.AddTemplate(template);
        }

        [HttpGet("Templates/{templateId}/Events")]
        public async Task<EventEmailPackage> GetMostRecentScrapeEvent(int templateId)
        {
            return await TempSvc.GetMostRecentScrapeEvent(templateId);
        }

        [HttpPost("Templates/{templateId}/Events")]
        public async Task AddScrapeEvent(int templateId, [FromBody] ScrapeEvent scrapeEvent)
        {
            await TempSvc.AddScrapeEvent(templateId, scrapeEvent);
        }

    }
}