using System.IO;
using System.Collections.Generic;

namespace FSBO.WebServices.TsClientGenerator
{
    public class Program
    {
        static void Main(string[] args)
        {
            var swaggerToAngular = new SwaggerToAngular()
            {
                SwaggerUrl = "http://localhost:23736/swagger/v1/swagger.json",
                UseJwt = false,
                EnumsToReplace = new Dictionary<string, string>()
                {
                    // Examples from TCC.CustomerManager
                    //{ @"IssueStatusOrSet\d+", "IssueStatusOrSet" },
                    //{ @"(IdentifierType\d+|identifierType)", "IdentifierType" },
                    //{ @"Status\d*", "IssueStatus" },
                    //{ @"IssueSummaryIssueStatus\d*", "IssueStatus" }
                },
                EnumsToNormalize = new List<string>()
                {

                }
            };

            var code = swaggerToAngular.GenerateCode();

            // This relative path allows for the client generator to be run from the 'bin' directory.
            SaveTextFile(@"..\..\..\..\FSBO.ClientPortal\Application\WebServicesClient.service.ts", code);
        }

        static void SaveTextFile(string path, string content)
        {
            using (var sw = File.CreateText(path))
            {
                sw.Write(content);
            }
        }
    }
}