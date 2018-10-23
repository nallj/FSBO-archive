using System;
using System.Collections.Generic;

namespace FSBO.WebServices.Models.Scraping
{
    public class EventEmailPackage
    {
        public int EventId { get; set; }

        public string AreaName { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
        public List<ScrapeRecord> Records { get; set; }
    }
}
