using AutoMapper;
using DFC.App.ContentPages.Controllers;
using DFC.App.ContentPages.Data;
using DFC.App.ContentPages.Data.Contracts;
using DFC.App.ContentPages.Filters;
using DFC.App.ContentPages.Framework;
using DFC.App.ContentPages.PageService;
using DFC.App.ContentPages.Repository.CosmosDb;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.ContentPages
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public const string CosmosDbConfigAppSettings = "Configuration:CosmosDbConnections:ContentPages";

        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public static void Configure(IApplicationBuilder app, IHostingEnvironment env, IMapper mapper)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                foreach (string category in new[] { PagesController.CategoryNameForHelp, PagesController.CategoryNameForAlert })
                {
                    // add the site map route
                    routes.MapRoute(
                        name: $"Sitemap-{category}",
                        template: $"{category}/Sitemap.xml",
                        defaults: new { controller = "Sitemap", action = "Sitemap", Category = category });
                }

                // add the robots.txt route
                routes.MapRoute(
                    name: "Robots",
                    template: "Robots.txt",
                    defaults: new { controller = "Robot", action = "Robot" });

                // add the default route
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Pages}/{action=Index}");
            });

            mapper?.ConfigurationProvider.AssertConfigurationIsValid();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var cosmosDbConnection = configuration.GetSection(CosmosDbConfigAppSettings).Get<CosmosDbConnection>();
            var documentClient = new DocumentClient(new Uri(cosmosDbConnection.EndpointUrl), cosmosDbConnection.AccessKey);
            services.AddApplicationInsightsTelemetry();
            services.AddHttpContextAccessor();
            services.AddScoped<ICorrelationIdProvider, CorrelationIdProvider>();
            services.AddSingleton(cosmosDbConnection);
            services.AddSingleton<IDocumentClient>(documentClient);
            services.AddSingleton<ICosmosRepository<ContentPageModel>, CosmosRepository<ContentPageModel>>();
            services.AddScoped<IContentPageService, ContentPageService>();
            services.AddAutoMapper(typeof(Startup).Assembly);

            services.AddMvc(config =>
                {
                    config.Filters.Add<LoggingAsynchActionFilter>();
                    config.RespectBrowserAcceptHeader = true;
                    config.ReturnHttpNotAcceptable = true;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }
    }
}