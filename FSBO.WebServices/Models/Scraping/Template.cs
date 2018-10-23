namespace FSBO.WebServices.Models.Scraping
{
    public class Template
    {
        public int TemplateId { get; set; }
        public int SourceId { get; set; }
        public string Name { get; set; }

        //public ICollection<ScrapeEvent> ScrapeEvents { get; set; }
        //public ICollection<SetupAction> SetupActions { get; set; }
        //public ICollection<TemplateField> TemplateFields { get; set; }
    }
}
