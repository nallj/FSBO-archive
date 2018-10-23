using System.Collections.Generic;

namespace FSBO.WebServices.Models.Scraping
{
    // Don't do this chief.
    //public enum AreaType { InvalidType, PostalCode, CityState, CityStatePostal }

    public class AreaWithEntries
    {
        public int AreaId { get; set; }
        public AreaType AreaType { get; set; }
        public string Name { get; set; }
        public IEnumerable<EntryPoint> EntryPoints { get; set; }
    }
}