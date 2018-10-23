using System.Collections.Generic;

namespace FSBO.WebServices.Models.Scraping
{
    public class TargetTemplateField
    {
        public int TemplateIndex { get; set; }
        public int? TemplateFieldId { get; set; }
        public int TargetFieldId { get; set; }
        public string Name { get; set; }
        public bool IsTemporaryField { get; set; }

        public List<ScrapingStep> OrderedScrapingSteps { get; set; }
    }
}
