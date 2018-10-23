namespace FSBO.WebServices.Models.Scraping
{
    public class BaseAction
    {
        public ScrapingActionType ActionType { get; set; }
        public string Parameters { get; set; }
    }
}
