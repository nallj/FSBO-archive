using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Web.Script.Serialization;

using HtmlAgilityPack;
using ScrapySharp.Network;
using ScrapySharp.Extensions;

using FSBO.WebServices.Client;
using FSBO.Scraper.Models.ActionParameters;

namespace FSBO.Scraper
{
    class Program
    {
        private static AreaClient AreaClient;
        //private static SourceClient SourceClient;
        private static TemplateClient TemplateClient;

        static Program()
        {
            AreaClient = new AreaClient();
            //SourceClient = new SourceClient();
            TemplateClient = new TemplateClient();
        }

        static void Main(string[] args)
        {
            Console.Write("Are you sure now is the best time for a scraping? Y/N  ");
            var answer = Console.ReadKey();

            Console.WriteLine("\n\n------------------------------------------------\n");

            if (answer.Key == ConsoleKey.Y)
            {
                // Do stuff.
                ScrapeAllSources().Wait();

                Console.WriteLine("\n\nPress any key to close...");
            }
            else if (answer.Key == ConsoleKey.N)    
            {
                // Don't do stuff.
                Console.WriteLine("\n\nThen why did you bother me, man?");
            }
            else
            {
                Console.WriteLine("\n\nThat's not an option, you ass.");
            }

        }

        public static async Task ScrapeAllSources()
        {
            //var dataSources = await SourceClient.GetSourcesAsync();
            var areasOfConcern = await AreaClient.GetAreasAsync();
            var allTemplates = await TemplateClient.GetTemplatesAsync();

            var topLevelBrowser = new ScrapingBrowser();

            // Dunno what these do but the tutorial had them.
            //topLevelBrowser.AllowAutoRedirect = true;
            //topLevelBrowser.AllowMetaRedirect = true;

            foreach (var area in areasOfConcern)
            {
                foreach (var entryPoint in area.EntryPoints)
                {
                    // Get a scraping Template for the Source associated with the current Entry Point.
                    var topLevelTemplate = allTemplates
                        .First(t => t.SourceId == entryPoint.SourceId && t.IsTopLevel);

                    // Create new scraping event and assign the area ID.
                    var scrapingEvent = new ScrapeEvent();
                    scrapingEvent.AreaId = area.AreaId;

                    // Create the record collection.
                    scrapingEvent.Records = new System.Collections.ObjectModel.ObservableCollection<ScrapeRecord>();
                    
                    // Begin scraping from the top-level Template.
                    ScrapeFromTemplate(topLevelBrowser, scrapingEvent, entryPoint.EntryPointUri, topLevelTemplate, allTemplates);

                    // Send scraping event to the WebServices to be added to the database.
                    await TemplateClient.AddScrapeEventAsync(topLevelTemplate.TemplateId, scrapingEvent);
                }
            }

        }

        private static void ScrapeFromTemplate(ScrapingBrowser scrapingBrowser, ScrapeEvent scrapingEvent, string templateUri, TemplateInstructions sourceTemplate, System.Collections.ObjectModel.ObservableCollection<TemplateInstructions> allTemplates)
        {
            // If the lock is not set to true, then scraping has begun for a new record (new property).
            //if (!scrapingEvent.LockOnCurrentRecord)
            //{
            //    scrapingEvent.Records.Add(new ScrapeRecord());
            //}

            var entryUri = new Uri(templateUri);
            var pageResult = scrapingBrowser.NavigateToPage(entryUri);
            var scraperPointer = pageResult.Html;

            // Perform setup actions for current Template.
            scraperPointer = PerformTemplateSetup(scraperPointer, sourceTemplate.OrderedSetup);
            
            // If the source template is a top-level template then looping over several items is necessary.
            if (sourceTemplate.IsTopLevel)
            {
                Console.WriteLine($"Traveling to top-level template @ {templateUri}");

                // Loop through each scrapable item and following all Scraping Steps for each Template Field to gather together the complete record.
                while (scraperPointer != null)
                {
                    // If this record violates any disqualifiers then skip it.
                    if (sourceTemplate.Disqualifiers.Count() != 0)
                    {
                        var disqualifyRecord = false;

                        foreach (var disqualifier in sourceTemplate.Disqualifiers)
                        {
                            switch (disqualifier.DisqualificationType)
                            {
                                case DisqualificationType.RecordNodeHasClass:
                                    var classAttr = scraperPointer.Attributes["class"];

                                    if (classAttr != null && classAttr.Value.Contains(disqualifier.Parameters))
                                        disqualifyRecord = true;
                                    break;

                                default:
                                    throw new Exception("Invalid or unknown disqualifier.");
                            }

                            if (disqualifyRecord)
                                break;
                        }

                        if (disqualifyRecord)
                        {
                            // Move pointer to next property and continue.
                            scraperPointer = scraperPointer.NextSibling;
                            continue;
                        }
                    }

                    // Create a record for the new property.
                    var newRecord = new ScrapeRecord();

                    // Create needed dictionaries.
                    newRecord.TargetFieldIdToValueDictionary = new Dictionary<string, string>();
                    newRecord.TemporaryFieldIdToValueDictionary = new Dictionary<string, string>();

                    // Add the new record to the current event.
                    scrapingEvent.Records.Add(newRecord);

                    SinglePassScrapeTemplate(scrapingBrowser, scrapingEvent, scraperPointer, sourceTemplate, allTemplates);

                    // Move pointer to next property.
                    scraperPointer = scraperPointer.NextSibling;
                }
            }
            else
            {
                Console.WriteLine($"\nTraveling to child template @ {templateUri}");
                
                SinglePassScrapeTemplate(scrapingBrowser, scrapingEvent, scraperPointer, sourceTemplate, allTemplates);

                // Child templates are the last things that are handled in the scraping order.  Once a child template is broken out of set the record lock to false since a new record is about to begin (nested child template depth should be irrelevant).
                //scrapingEvent.LockOnCurrentRecord = false;
            }
            
        }

        private static void SinglePassScrapeTemplate(ScrapingBrowser scrapingBrowser, ScrapeEvent scrapingEvent, HtmlNode scraperPointer, TemplateInstructions sourceTemplate, System.Collections.ObjectModel.ObservableCollection<TemplateInstructions> allTemplates)
        {
            var absoluteUri = scrapingBrowser.Referer.AbsoluteUri;
            var rootUri = absoluteUri.Substring(0, absoluteUri.IndexOf('/', 8));

            foreach (var templateField in sourceTemplate.TemplateFields)
            {
                if (templateField.OrderedScrapingSteps.Count() == 0)
                    continue;

                try
                {
                    PerformScrapingForTemplateField(scraperPointer, templateField, scrapingEvent, rootUri);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\t\"{templateField.Name}\" field missing from entry.");
                }
                
                // Perform navigation action if the last step of a template's scraping instructions dictates it.
                var lastStep = templateField.OrderedScrapingSteps.Last();
                if (lastStep.ActionType == ScrapingActionType.TravelToChildTemplate)
                {
                    var jsonSerializer = new JavaScriptSerializer();

                    var travelToChildTemplateParams = new TravelToChildTemplateParams();
                    travelToChildTemplateParams = jsonSerializer
                        .Deserialize<TravelToChildTemplateParams>(lastStep.Parameters);

                    // Need child template instructions.
                    var childTemplateId = travelToChildTemplateParams.Child;
                    var childTemplate = allTemplates
                        .First(t => t.TemplateId == childTemplateId && !t.IsTopLevel);

                    int targetFieldId;
                    var sourceIsNumericId = int.TryParse(travelToChildTemplateParams.Source, out targetFieldId);
                    string targetUri;
                    
                    // The target URI was saved as a temporary field.
                    if (travelToChildTemplateParams.IsFromTemporaryField)
                        targetUri = scrapingEvent.Records.Last().TemporaryFieldIdToValueDictionary[targetFieldId.ToString()];

                    // The target URI is a standard, non-temporary field.
                    else
                        targetUri = scrapingEvent.Records.Last().TargetFieldIdToValueDictionary[targetFieldId.ToString()];

                    // Set record lock so that the same record is used.
                    scrapingEvent.LockOnCurrentRecord = true;
                    
                    ScrapeFromTemplate(scrapingBrowser, scrapingEvent, rootUri + targetUri, childTemplate, allTemplates);
                }
            }
        }

        private static HtmlNode PerformTemplateSetup(HtmlNode pointer, IEnumerable<SetupStep> orderedSetupSteps)
        {
            foreach (var setupStep in orderedSetupSteps)
            {
                switch (setupStep.ActionType)
                {
                    case ScrapingActionType.CssSelect:
                        pointer = pointer
                            .CssSelect(setupStep.Parameters)
                            .First();
                        break;

                    case ScrapingActionType.FirstDescendantNodeOfType:
                        pointer = pointer
                            .Descendants()
                            .First(c => c.Name == setupStep.Parameters);
                        break;

                    // Can't be done since this produces a collection instead of a single node.
                    //case ScrapingActionType.ChildNodes:
                    //    pointer = pointer.SelectNodes(setupStep.Parameters);
                    //    break;

                    /*case ScrapingActionType.FirstChildNodeOfType:
                        pointer = pointer
                            .ChildNodes
                            .First(c => c.Name == setupStep.Parameters);
                        break;*/

                    default:
                        throw new Exception("Invalid or unknown setup step.");
                }
            }

            return pointer;
        }

        private static void PerformScrapingForTemplateField(HtmlNode pointer, TargetTemplateField templateField, ScrapeEvent scrapeEvent, string rootUri)
        {
            string targetValue = null;
            var jsonSerializer = new JavaScriptSerializer();

            foreach (var setupStep in templateField.OrderedScrapingSteps)
            {
                switch (setupStep.ActionType)
                {
                    case ScrapingActionType.CssSelect:
                        pointer = pointer
                            .CssSelect(setupStep.Parameters)
                            .First();
                        break;

                    case ScrapingActionType.FirstDescendantNodeOfType:
                        pointer = pointer
                            .Descendants()
                            .First(c => c.Name == setupStep.Parameters);
                        break;

                    case ScrapingActionType.FirstDescendantNodeOfTypeAttrsVals:
                        
                        var firstDescendantNodeOfTypeAttrsValsParams = new FirstDescendantNodeOfTypeAttrsValsParams();
                        firstDescendantNodeOfTypeAttrsValsParams = jsonSerializer
                            .Deserialize<FirstDescendantNodeOfTypeAttrsValsParams>(setupStep.Parameters);

                        var possibleChoices = pointer
                            .Descendants()
                            .Where(n => n.Name == firstDescendantNodeOfTypeAttrsValsParams.Node);

                        // All attribute-value pairs must be present.
                        for (var i = 0; i < firstDescendantNodeOfTypeAttrsValsParams.Attributes.Count(); i++)
                        {
                            var attribute = firstDescendantNodeOfTypeAttrsValsParams.Attributes[i];
                            var correspondingValue = firstDescendantNodeOfTypeAttrsValsParams.Values[i];

                            possibleChoices = possibleChoices
                                .Where(n =>
                                    n.Attributes.Contains(attribute) &&
                                    n.Attributes[attribute].Value == correspondingValue);
                        }

                        // If a depth value is given, grab the Nth descendant.  Otherwise, grab the first.
                        if (firstDescendantNodeOfTypeAttrsValsParams.Depth.HasValue)
                        {
                            pointer = possibleChoices.ElementAt(firstDescendantNodeOfTypeAttrsValsParams.Depth.Value - 1);
                        }
                        else
                        {
                            pointer = possibleChoices.First();
                        }

                        break;

                    case ScrapingActionType.NthDescendantNodeOfType:
                        break;

                    case ScrapingActionType.SelectInnerText:
                        targetValue = pointer.InnerText;
                        break;

                    case ScrapingActionType.SelectAttributeValue:
                        targetValue = pointer.Attributes[setupStep.Parameters].Value;
                        break;

                    //case ScrapingActionType.SelectInnerTextTemporaryValue:
                    //    temporaryValueKey = setupStep.Parameters;
                    //    targetValue = pointer.InnerText;
                    //    break;

                    // Perform no action - navigation can't ocur in a static method.
                    case ScrapingActionType.TravelToChildTemplate:
                        break;

                    case ScrapingActionType.TemporaryTravelToDerivedLocation:

                        var temporaryBrowser = new ScrapingBrowser();

                        var entryUri = new Uri(rootUri + targetValue);
                        var pageResult = temporaryBrowser.NavigateToPage(entryUri);
                        pointer = pageResult.Html;

                        break;

                    case ScrapingActionType.SplitData:

                        var splitDataParams = new SplitDataParams();
                        splitDataParams = jsonSerializer
                            .Deserialize<SplitDataParams>(setupStep.Parameters);

                        var location = pointer.InnerText.IndexOf(splitDataParams.Split);

                        for (var i = 0; i < splitDataParams.Index; i++)
                            location = pointer.InnerText.IndexOf(splitDataParams.Split, location);

                        //var splitSubstring = pointer.InnerText.Substring()

                        break;

                    case ScrapingActionType.ReplaceInstancesOf:

                        var replaceInstancesOfParams = new ReplaceInstancesOfParams();
                        replaceInstancesOfParams = jsonSerializer
                            .Deserialize<ReplaceInstancesOfParams>(setupStep.Parameters);

                        targetValue = targetValue.Replace(replaceInstancesOfParams.Replace, replaceInstancesOfParams.With);

                        break;

                    case ScrapingActionType.RemoveOutsideOfBordersExclusive:

                        var removeOutsideOfBordersParams = new RemoveOutsideOfBordersParams();
                        removeOutsideOfBordersParams = jsonSerializer
                            .Deserialize<RemoveOutsideOfBordersParams>(setupStep.Parameters);

                        var left = removeOutsideOfBordersParams.Left;
                        var right = removeOutsideOfBordersParams.Right;
                        var leftLen = left.Length;

                        var indexOfRight = targetValue.IndexOf(right);
                        var indexOfLeft = targetValue.LastIndexOf(left, indexOfRight);

                        targetValue = targetValue.Substring(indexOfLeft + left.Length, indexOfRight - indexOfLeft - leftLen);

                        break;

                    case ScrapingActionType.Trim:

                        targetValue = targetValue.Trim();
                        break;

                    case ScrapingActionType.NodeWithInnerTextSubstring:
                        
                        var nodeWithInnerTextSubstringParams = new NodeWithInnerTextSubstringParams();
                        nodeWithInnerTextSubstringParams = jsonSerializer
                            .Deserialize<NodeWithInnerTextSubstringParams>(setupStep.Parameters);
                        
                        pointer = pointer
                            .Descendants()
                            .First(n => n.Name == nodeWithInnerTextSubstringParams.Node
                                && n.InnerText.Contains(nodeWithInnerTextSubstringParams.Substring));

                        break;
                        
                    default:
                        throw new Exception("Invalid or unknown setup step.");
                }
            }

            var scrapeRecord = scrapeEvent.Records.Last();

            // Add recovered value to the scrape event field value dictionary.
            var stringTempFieldId = templateField.TargetFieldId.ToString(); // .TemplateFieldId.Value
            if (targetValue != null)
                if (templateField.IsTemporaryField)
                    scrapeRecord.TemporaryFieldIdToValueDictionary.Add(stringTempFieldId, targetValue);
                else
                    scrapeRecord.TargetFieldIdToValueDictionary.Add(stringTempFieldId, targetValue);
        }

    }
}