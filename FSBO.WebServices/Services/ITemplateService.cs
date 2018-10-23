using System.Threading.Tasks;
using System.Collections.Generic;

using FSBO.WebServices.Models.Scraping;

namespace FSBO.WebServices.Services
{
    public interface ITemplateService
    {
        // Template methods
        IEnumerable<TemplateInstructions> GetTemplates();
        Task<int> AddTemplate(Template template);
        Task<EventEmailPackage> GetMostRecentScrapeEvent(int templateId);
        Task AddScrapeEvent(int templateId, ScrapeEvent scrapeEvent);
    }
}
