using System.Collections.Generic;

namespace FSBO.WebServices.Models.Scraping
{
    public class TemplateInstructions
    {
        public int SourceId { get; set; }

        public int TemplateId { get; set; }

        public bool IsTopLevel { get; set; }

        public List<SetupStep> OrderedSetup { get; set; }

        public List<RecordDisqualification> Disqualifiers { get; set; }

        public List<TargetTemplateField> TemplateFields { get; set; }
    }
}
