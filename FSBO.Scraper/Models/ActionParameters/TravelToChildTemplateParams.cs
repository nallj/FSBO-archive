namespace FSBO.Scraper.Models.ActionParameters
{
    public class TravelToChildTemplateParams
    {
        public int Child { get; set; }
        public string Source { get; set; }
        public bool IsFromTemporaryField { get; set; }

        public TravelToChildTemplateParams()
        {
            IsFromTemporaryField = true;
        }
    }
}
