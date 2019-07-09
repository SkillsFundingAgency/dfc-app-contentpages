using DFC.App.Help.Data;
using DFC.App.Help.Data.Contracts;
using DFC.App.Help.Filters;
using DFC.App.Help.Framework;
using DFC.App.Help.PageService;
using DFC.App.Help.Repository.CosmosDb;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DFC.App.Help
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var cosmosDbConnection = _configuration.GetSection("Configuration:CosmosDbConnections:HelpPages").Get<CosmosDbConnection>();

            services.AddHttpContextAccessor();
            services.AddScoped<ICorrelationIdProvider, CorrelationIdProvider>();
            services.AddSingleton<CosmosDbConnection>(cosmosDbConnection);
            services.AddSingleton<IRepository<HelpPageModel>, Repository<HelpPageModel>>();
            services.AddScoped<IHelpPageService, HelpPageService>();

            services.AddMvc(config =>
                {
                    config.Filters.Add<LoggingAsynchActionFilter>();
                    config.RespectBrowserAcceptHeader = true;
                    //       config.ReturnHttpNotAcceptable = true;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
                    defaults: new { controller = "Sitemap", action = "Sitemap" }
                );

                // add the robots.txt route
                routes.MapRoute(
                    name: "Robots",
                    template: "Robots.txt",
                    defaults: new { controller = "Robot", action = "Robot" }
                );

                // add the default route
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Error}");
            });
        }
    }
}
