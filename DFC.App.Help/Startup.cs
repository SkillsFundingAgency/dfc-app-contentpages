using AutoMapper;
using DFC.App.DraftHelp.PageService;
using DFC.App.Help.Data;
using DFC.App.Help.Data.Contracts;
using DFC.App.Help.Extensions;
using DFC.App.Help.Filters;
using DFC.App.Help.Framework;
using DFC.App.Help.PageService;
using DFC.App.Help.Policies.Options;
using DFC.App.Help.Repository.CosmosDb;
using DFC.App.Help.Repository.SitefinityApi.DataContext;
using DFC.App.Help.Repository.SitefinityApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DFC.App.Help
{
    public class Startup
    {
        public const string CosmosDbConfigAppSettings = "Configuration:CosmosDbConnections:HelpPages";

        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
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
            var sitefinityApiConnection = configuration.GetSection("SitefinityApi").Get<SitefinityAPIConnectionSettings>();

            services.AddSingleton(sitefinityApiConnection ?? new SitefinityAPIConnectionSettings());
            services.AddHttpContextAccessor();
            services.AddScoped<ICorrelationIdProvider, CorrelationIdProvider>();
            services.AddSingleton(cosmosDbConnection);
            services.AddSingleton<IDocumentClient>(documentClient);
            services.AddSingleton<ICosmosRepository<HelpPageModel>, CosmosRepository<HelpPageModel>>();
            services.AddScoped<IHelpPageService, HelpPageService>();
            services.AddScoped<IDraftHelpPageService, DraftHelpPageService>();
            services.AddAutoMapper(typeof(Startup).Assembly);
            services.AddHttpClient<ISitefinityODataContext, SitefinityODataContext, HttpClientOptions>(configuration, nameof(HttpClientOptions));
            services.AddHttpClient<ITokenService, TokenService, HttpClientOptions>(configuration, nameof(HttpClientOptions));

            services.AddMvc(config =>
                {
                    config.Filters.Add<LoggingAsynchActionFilter>();
                    config.RespectBrowserAcceptHeader = true;
                    config.ReturnHttpNotAcceptable = true;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IMapper mapper)
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
                // add the site map route
                routes.MapRoute(
                    name: "Sitemap",
                    template: "Sitemap.xml",
                    defaults: new { controller = "Sitemap", action = "Sitemap" });

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
    }
}