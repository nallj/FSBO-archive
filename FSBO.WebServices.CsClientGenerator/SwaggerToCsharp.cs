using NSwag;
using System;
using System.Threading.Tasks;
using NSwag.CodeGeneration.CSharp;
using NSwag.CodeGeneration.OperationNameGenerators;

namespace FSBO.WebServices.CsClientGenerator
{
    public class SwaggerToCsharp
    {
        public string SwaggerUrl;
        public string SwaggerJson;

        public string GenerateCode()
        {
            var doc = Task.Run(async () => await GetSwaggerDocAsync()).Result;

            var settings = new SwaggerToCSharpClientGeneratorSettings()
            {
                GenerateClientClasses = true,
                GenerateDtoTypes = true,
                OperationNameGenerator = new MultipleClientsFromOperationIdOperationNameGenerator()
            };

            settings.CSharpGeneratorSettings.Namespace = "FSBO.WebServices.Client";

            var generator = new SwaggerToCSharpClientGenerator(doc, settings);
            var code = generator.GenerateFile();

            return code;
        }

        private async Task<SwaggerDocument> GetSwaggerDocAsync()
        {
            SwaggerDocument doc = null;
            if (!string.IsNullOrWhiteSpace(SwaggerUrl))
            {
                doc = await SwaggerDocument.FromUrlAsync(SwaggerUrl);
            }
            else if (!string.IsNullOrWhiteSpace(SwaggerJson))
            {
                doc = await SwaggerDocument.FromUrlAsync(SwaggerJson);
            }
            else
            {
                throw new Exception("Swagger URL or JSON must be provided");
            }
            return doc;
        }

    }
}
