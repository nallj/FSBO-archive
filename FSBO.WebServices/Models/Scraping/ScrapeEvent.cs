using System.Collections.Generic;

namespace FSBO.WebServices.Models.Scraping
{
    public class ScrapeEvent
    {
        public int AreaId { get; set; }
        public List<ScrapeRecord> Records { get; set; }

        /// <summary>
        /// Used by the Scraper.
        /// </summary>
        public bool LockOnCurrentRecord { get; set; }
    }
}
