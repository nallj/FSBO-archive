using NSwag;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NSwag.CodeGeneration.TypeScript;
using NJsonSchema.CodeGeneration.TypeScript;
using NSwag.CodeGeneration.OperationNameGenerators;

namespace FSBO.WebServices.TsClientGenerator
{
    public class SwaggerToAngular
    {
        public string SwaggerUrl;
        public string SwaggerJson;
        public bool UseJwt;

        /// <summary>
        /// Removes all code for keys, and replaces any references with values.
        /// Necessary because for every resused Enum and new instance is created.
        /// </summary>
        public Dictionary<string, string> EnumsToReplace = new Dictionary<string, string>();

        /// <summary>
        /// Unfortunately NSwag is built on version 2.0.0 of the Swagger API which only supports string-based enums.  Once NSwag upgrades the Swagger API to 2.2.0 number-based enums will be supported and this functionality can be removed.
        /// Issue fixed (but not implemented in NSwag) here: https://github.com/swagger-api/swagger-codegen/pull/2508
        /// </summary>
        public List<string> EnumsToNormalize;

        public string GenerateCode()
        {
            var doc = Task.Run(async () => await GetSwaggerDocAsync()).Result;

            var settings = new SwaggerToTypeScriptClientGeneratorSettings()
            {
                GenerateClientClasses = true,
                GenerateDtoTypes = true,
                ClassName = "{controller}ApiClient",
                PromiseType = PromiseType.Promise,
                Template = TypeScriptTemplate.Angular,
                OperationNameGenerator = new MultipleClientsFromOperationIdOperationNameGenerator()
            };

            settings.TypeScriptGeneratorSettings.TypeStyle = TypeScriptTypeStyle.Class;
            settings.TypeScriptGeneratorSettings.TypeScriptVersion = 2.2m;
            settings.TypeScriptGeneratorSettings.MarkOptionalProperties = true;

            var generator = new SwaggerToTypeScriptClientGenerator(doc, settings);
            var code = generator.GenerateFile();

            var removeAnnoyingBaseApiAndVerRegex = new Regex(@"apiv\d+", RegexOptions.IgnoreCase);
            code = removeAnnoyingBaseApiAndVerRegex.Replace(code, "");

            if (UseJwt)
            {
                code = code.Replace("from '@angular/http';",
                    "from '@angular/http';\nimport { AuthHttp } from 'angular2-jwt';");
                code = code.Replace("@Inject(Http) http: Http", "@Inject(AuthHttp) http: AuthHttp");
                code = code.Replace("private http: Http;", "private http: AuthHttp;");
            }

            code = ReplaceEnums(code);
            code = NormalizeEnums(code);
            code = ReplaceExtraLineBreaks(code);

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
                throw new Exception("Swagger url or json must be provided");
            }
            return doc;
        }

        private string ReplaceEnums(string code)
        {
            if (EnumsToReplace == null || !EnumsToReplace.Any()) return code;

            var enumRemovalReText = string.Join("|", EnumsToReplace.Keys.Select(x => @"(^export enum " + x + @" \{.*?\}$)"));
            code = Regex.Replace(code, enumRemovalReText, "", RegexOptions.Multiline | RegexOptions.Singleline);
            foreach (var kvp in EnumsToReplace)
            {
                // because an enum could be the same as property value, only ones preceeded by a :, indicating a type are replaced
                code = Regex.Replace(code, $@"(?<=:\s*)\b{kvp.Key}\b", kvp.Value);
            }
            return code;
        }

        /// <summary>
        /// Remove string value so that enumerated values are number-based.
        /// </summary>
        private string NormalizeEnums(string code)
        {
            if (EnumsToNormalize == null || !EnumsToNormalize.Any()) return code;

            foreach (var e in EnumsToNormalize)
            {
                var enumRemovalReText = @"^export enum " + e + @" \{.*?\}$";
                var foundEnum = Regex.Match(code, enumRemovalReText, RegexOptions.Multiline | RegexOptions.Singleline);

                var stringValueRemovalRegex = @"\s*=\s*<any>\s*""[^""]*""\s*,";
                var fixedEnum = Regex.Replace(foundEnum.Value, stringValueRemovalRegex, ",", RegexOptions.Multiline | RegexOptions.Singleline);

                code = Regex.Replace(code, enumRemovalReText, fixedEnum, RegexOptions.Multiline | RegexOptions.Singleline);
            }

            return code;
        }

        /// <summary>
        /// After replacing enums extra line breaks show up, so this cleans stuff up.
        /// </summary>
        private string ReplaceExtraLineBreaks(string code)
        {
            code = Regex.Replace(code, @"^\n(^\n{2,})", "\n", RegexOptions.Multiline);
            return code;
        }

    }
}
