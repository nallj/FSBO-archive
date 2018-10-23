namespace FSBO.WebServices.Models.Scraping
{
    public enum ScrapingActionType
    {
        NotSpecified,

        // Node Navigation.
        CssSelect,
        FirstDescendantNodeOfType,
        FirstDescendantNodeOfTypeAttrsVals,
        NthDescendantNodeOfType,

        // Data Capture.
        SelectInnerText,
        SelectAttributeValue,
        SelectInnerTextTemporaryValue,

        // Browser Navigation
        TravelToChildTemplate,
        TemporaryTravelToDerivedLocation,

        // Data Manipulation
        SplitData,
        ReplaceInstancesOf,
        RemoveOutsideOfBordersExclusive,
        Trim,

        // Search for Specific Instance.
        NodeWithInnerTextSubstring,

        Invalid
    }
}
