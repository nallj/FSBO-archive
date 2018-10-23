using System;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

using FSBO.WebServices.Client;
using System.Collections.Generic;

namespace FSBO.Distributor
{
    class Program
    {
        /*private const string ZillowEmailStyles = @"

.scraping-records {}
.event-record {}

h2 { margin-bottom: 0 }
h3 { margin-top: 3px }
h4 { margin-top: 3px }
";*/

        // Don't include "<style><templateStyles></style>".  Can't embed styles in this way.
        private const string ZillowEmailTemplate = @"

<h1>Your Daily FSBO Report</h1>

<p>Good <timeOfDayGreeting>, <subscriberName>.  Here are all the records associated with the ""<scapedArea>"" area recovered on ""<eventDateTime>.""<p>

<div id=""scraping-records"" style=""padding-top: 5px""><eventRecords></div>

";
        private const string ZillowEmailRecordTemplate = @"

<div class=""event-record"">
    <h2 style=""margin-bottom: 0""><targetId:3></h2>
    <h3 style=""margin-top: 3px""><targetId:6>, <targetId:7> <targetId:8></h3>
    <h4 style=""margin-top: 3px""><a href=""https://zillow.com<targetId:24>"" target=""_blank"">https://zillow.com<targetId:24></a></h4>

    <ul>
        <if present=""22""><li>Owner, Info: <targetId:22></li></if>
        <if present=""13""><li>Asking Price: <targetId:13></li></if>
        <if present=""14""><li>Bedroom Count: <targetId:14></li></if>
        <if present=""15""><li>Bathroom Count: <targetId:15></li></if>
        <if present=""25""><li>Age of Listing: <targetId:25></li></if>
        <if present=""16""><li>Area (in sqft): <targetId:16></li></if>
        <if present=""17""><li>Description: <targetId:17></li></if>
        <if present=""23""><li>County Appraiser: <a href=""<targetId:23>""><targetId:23></a></li></if>
    </ul>
</div>

";

        private static TemplateClient TemplateClient;
        private static UserClient UserClient;

        static Program()
        {
            TemplateClient = new TemplateClient();
            UserClient = new UserClient();
        }


        static void Main(string[] args)
        {
            Console.Write("Are you sure now is the best time for distribution? Y/N  ");
            //var answer = Console.ReadKey();

            //if (answer.Key == ConsoleKey.Y)
            //{
                // Do stuff.
                EmailSubscribers().Wait();
            /*}
            else if (answer.Key == ConsoleKey.N)
            {
                // Don't do stuff.
                Console.WriteLine("\n\nThen why did you bother me, man?");
            }
            else
            {
                Console.WriteLine("\n\nThat's not an option, you ass.");
            }*/
        }

        private static async Task EmailSubscribers()
        {
            var availableTemplates = await TemplateClient.GetTemplatesAsync();
            var zillowTemplate = availableTemplates.First(t => t.IsTopLevel);

            var recentZillowScrapeEvent = await TemplateClient.GetMostRecentScrapeEventAsync(zillowTemplate.TemplateId);

            var recordTemplates = new List<string>();
            foreach (var record in recentZillowScrapeEvent.Records)
            {
                var template = ZillowEmailRecordTemplate;
                var recordData = record.TargetFieldIdToValueDictionary;

                // Satisfy all "if" blocks first.
                var ifSubstr = "<if present=\"";
                var ifLocation = template.IndexOf(ifSubstr);

                while (ifLocation != -1)
                {
                    var ifClosing = template.IndexOf("</if>", ifLocation);
                    var openingLen = ifLocation + ifSubstr.Length;

                    var idQuoteClosing = template.IndexOf("\"", openingLen);
                    var targetIdOfConcern = template.Substring(openingLen, idQuoteClosing - openingLen);

                    var continueFrom = ifClosing;

                    // If the record does not contain the ID found within the "if" block, remove the block and its' contents.
                    if (!recordData.ContainsKey(targetIdOfConcern))
                    {
                        template = template.Substring(0, ifLocation) + template.Substring(ifClosing + 5);
                        continueFrom = ifLocation;
                    }

                    // Check for next "if" block.
                    ifLocation = template.IndexOf(ifSubstr, continueFrom);
                }

                // Replace all remaining "<targetId:x>" tags with the corresponding data value.
                var targetTagSubstr = "<targetId:";
                var tagLocation = template.IndexOf(targetTagSubstr);

                while (tagLocation != -1)
                {
                    var tagClosing = template.IndexOf(">", tagLocation);
                    var openingLen = tagLocation + targetTagSubstr.Length;

                    var targetIdOfConcern = template.Substring(openingLen, tagClosing - openingLen);

                    template =
                        template.Substring(0, tagLocation)
                        + (recordData[targetIdOfConcern] ?? "").Trim()
                        + template.Substring(tagClosing + 1);

                    // Check for next "targetId" tag.
                    tagLocation = template.IndexOf(targetTagSubstr);
                }

                recordTemplates.Add(template);
            }

            // Get the head template that will be the body of the email.
            var masterTemplate = ZillowEmailTemplate;
            
            masterTemplate = ReplaceSingleTagWithString(masterTemplate, "<timeOfDayGreeting>", ProduceTimeOfDayGreeting());

            masterTemplate = ReplaceSingleTagWithString(masterTemplate, "<scapedArea>", recentZillowScrapeEvent.AreaName);

            masterTemplate = ReplaceSingleTagWithString(masterTemplate, "<eventDateTime>", recentZillowScrapeEvent.TimeStamp.ToLongDateString() + " at " + recentZillowScrapeEvent.TimeStamp.ToLongTimeString());

            var allRecords = recordTemplates.Aggregate((x, y) => x + "<br />" + y);
            masterTemplate = ReplaceSingleTagWithString(masterTemplate, "<eventRecords>", allRecords);

            // Always do last (prevent conflicts with string.Format(...)).
            //masterTemplate = ReplaceSingleTagWithString(masterTemplate, "<templateStyles>", ZillowEmailStyles);

            // Retrieve all active subscribers.  Loop through all of them and send each subscriber an email.
            var activeSubscribers = await UserClient.GetSubscribersAsync();
            foreach (var subscriber in activeSubscribers)
            {
                // Now send out emails. Replace "<subscriberName>" tags with subscriber's names.
                ReplaceSubscriberTagAndSendEmail(masterTemplate, subscriber);
            }

        }

        private static string ReplaceSingleTagWithString(string template, string tag, string value)
        {
            var tagLocation = template.IndexOf(tag);
            var tagClosing = template.IndexOf(">", tagLocation);
            
            template =
                template.Substring(0, tagLocation)
                + value
                + template.Substring(tagClosing + 1);

            return template;
        }

        private static string ProduceTimeOfDayGreeting()
        {
            var now = DateTime.Now;

            if (now.Hour < 12)
                return "Morning";

            else if (now.Hour > 11 && now.Hour < 17)
                return "Afternoon";

            else
                return "Evening";
        }

        private static void ReplaceSubscriberTagAndSendEmail(string masterTemplate, Subscriber subscriber)
        {
            masterTemplate = ReplaceSingleTagWithString(masterTemplate, "<subscriberName>", subscriber.Name);

            using (var client = new SmtpClient("mail.jamesbnall.com"))
            {
                var userName = "do.not.reply@jamesbnall.com";
                var password = "yJqMNvZvE1bsoymHBU7rvL2IxubCmYr4";

                client.Credentials = new System.Net.NetworkCredential(userName, password);

                var email = new MailMessage(userName, subscriber.Email);

                email.Body = masterTemplate;
                email.Subject = "Your Daily FSBO Report";
                email.IsBodyHtml = true;

                try
                {
                    client.Send(email);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occured: " + ex.Message);
                }
            }
        }

    }
}