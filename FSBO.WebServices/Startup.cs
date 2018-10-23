using AutoMapper;
using Newtonsoft.Json;
using NSwag.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

using FSBO.WebServices.Models;
using FSBO.WebServices.Services;
using FSBO.WebServices.Extensions;

namespace FSBO.WebServices
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            // Add MVC to services as per normal.  Additionally, add the JSON Exception Filter Attribute provided by NSwag.
            services.AddMvc(options =>
            {
                //options.OutputFormatters.Clear();
                //options.OutputFormatters.Add(new JsonOutputFormatter(new JsonSerializerSettings()
                //{
                //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                //}, System.Buffers.ArrayPool<char>.Shared));

                options.Filters.Add(new JsonExceptionFilterAttribute());
            });

            // Add AutoMapper service.
            services.AddAutoMapper();

            var webServicesSettingsSection = Configuration.GetSection("WebServicesSettings");

            // Get Web Service settings specified in the appsettings.json file.
            services.Configure<WebServicesSettings>(webServicesSettingsSection);

            // Cross-Origin Resource Sharing (CORS)
            var appOrigin = (string)webServicesSettingsSection.GetValue(typeof(string), "CorsAppOrigin");
                //.GetSection("Cors")
                //.GetValue(typeof(string), "AppOrigin");
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                    builder.AllowAnyHeader()
                        .AllowAnyMethod()
                        .WithOrigins(new string[] { appOrigin })
                        .AllowCredentials());
            });

            var contextConnectionString = Configuration.GetConnectionString("FsboContext");

            // Add database context.
            services
                .AddFsboContext(contextConnectionString)
                .AddFsboContextFactoryMethod(contextConnectionString);
            //services.AddScoped(x =>
            //{
            //    var dbConnectionString = Configuration.GetConnectionString("FsboContext");
            //    var dbContext = new DAL.FsboContext(dbConnectionString);

            //    // Don't have the DbContext take initiative and grab everything it can.  Let me decide what to explicitly include.
            //    dbContext.Configuration.LazyLoadingEnabled = false;

            //    return dbContext;
            //});

            // Add application services.
            services
                .AddScoped<IAreaService, AreaService>()
                .AddScoped<ISourceService, SourceService>()
                .AddScoped<ITemplateService, TemplateService>()
                .AddScoped<IUserService, UserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            // Register the Swagger generator and the UI; the UI doesn't seem to work for some reason.
            //new SwaggerUiOwinSettings

            // Register the Swagger generator when in the development environment.
            if (env.IsDevelopment())
                app.UseSwagger(typeof(Startup).Assembly, new SwaggerOwinSettings
                {
                    // According to NSwag's GitHub documentation, when integrating with .NET Core middleware, CamelCase JSON property name handling must be enabled <https://github.com/NSwag/NSwag/wiki/Middlewares#integrate-with-aspnet-core>.
                    DefaultPropertyNameHandling = NJsonSchema.PropertyNameHandling.CamelCase
                });

            // Register custom exception handler.
            app.UseMiddleware(typeof(Middleware.ExceptionHandler));


            // TUTORIAL: configure jwt authentication
            var appSettings = app.ApplicationServices.GetService<IOptions<WebServicesSettings>>().Value;
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                }
            });
            // /TUTORIAL

            app.UseCors("CorsPolicy");
            app.UseMvc();
        }
    }
}
