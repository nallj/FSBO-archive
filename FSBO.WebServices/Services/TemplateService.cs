using AutoMapper;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper.QueryableExtensions;

using FSBO.WebServices.Models.Scraping;

namespace FSBO.WebServices.Services
{
    public class TemplateService : ITemplateService
    {
        private DAL.IFsboContext DbContext;

        public TemplateService(DAL.IFsboContext dbContext)
        {
            DbContext = dbContext ?? new DAL.FsboContext();
        }

        public IEnumerable<TemplateInstructions> GetTemplates()
        {
            var templates = DbContext.Templates
                .ProjectTo<TemplateInstructions>()
                .ToList();

            return templates;
        }

        public async Task<int> AddTemplate(Template template)
        {
            var dbTemplate = Mapper.Map<DAL.Template>(template);

            DbContext.Templates.Add(dbTemplate);
            await DbContext.SaveChangesAsync();

            return template.TemplateId;
        }

        public async Task<EventEmailPackage> GetMostRecentScrapeEvent(int templateId)
        {
            // Get the most recent event associated with the target Template.
            var dbMostRecentEvent = await DbContext.ScrapeEvents
                .Include(i => i.Area)
                .Include(i => i.ScrapeRecords)
                .Include(i => i.ScrapeRecords.Select(s => s.TargetValues))
                .Include(i => i.ScrapeRecords.Select(s => s.TargetValues.Select(sel => sel.TargetField)))
                .Where(t => t.TemplateId == templateId)
                .OrderByDescending(o => o.TimeStamp)
                .FirstAsync();

            var mostRecentEvent = Mapper.Map<EventEmailPackage>(dbMostRecentEvent);
            return mostRecentEvent;
        }

        public async Task AddScrapeEvent(int templateId, ScrapeEvent scrapeEvent)
        {
            // Get the target template.
            var dbTemplate = DbContext.Templates
                .First(t => t.TemplateId == templateId);

            // Map the object to the corresponding DAL model.
            var dbScrapeEvent = Mapper.Map<DAL.ScrapeEvent>(scrapeEvent);

            // Add the mapped event to the database and save the changes.
            dbTemplate.ScrapeEvents.Add(dbScrapeEvent);
            await DbContext.SaveChangesAsync();
        }

    }
}