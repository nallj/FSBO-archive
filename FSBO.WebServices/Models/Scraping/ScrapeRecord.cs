using System.Collections.Generic;

namespace FSBO.WebServices.Models.Scraping
{
    public class ScrapeRecord
    {
        public Dictionary<int, string> TargetFieldIdToValueDictionary { get; set; }
        public Dictionary<int, string> TemporaryFieldIdToValueDictionary { get; set; }
    }
}
