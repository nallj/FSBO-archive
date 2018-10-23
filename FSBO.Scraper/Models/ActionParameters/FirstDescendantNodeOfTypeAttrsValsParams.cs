namespace FSBO.Scraper.Models.ActionParameters
{
    class FirstDescendantNodeOfTypeAttrsValsParams
    {
        public string Node { get; set; }
        public string[] Attributes { get; set; }
        public string[] Values { get; set; }

        // If depth is set, grab the Nth descendant instead of the first occurance.
        public int? Depth { get; set; }
    }
}
