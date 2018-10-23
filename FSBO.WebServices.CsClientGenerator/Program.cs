using System.IO;

namespace FSBO.WebServices.CsClientGenerator
{
    public class Program
    {
        static void Main(string[] args)
        {
            var swaggerToAngular = new SwaggerToCsharp()
            {
                SwaggerUrl = "http://localhost:23736/swagger/v1/swagger.json"
            };

            var code = swaggerToAngular.GenerateCode();

            // This relative path allows for the client generator to be run from the 'bin' directory.
            SaveTextFile(@"..\..\..\..\FSBO.WebServices.Client\Clients.cs", code);
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