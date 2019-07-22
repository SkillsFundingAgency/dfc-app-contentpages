using DFC.App.Help.Data.Contracts;
using DFC.App.Help.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace DFC.App.Help.Controllers
{
    public class SitemapController : Controller
    {
        private readonly ILogger<SitemapController> logger;
        private readonly IHelpPageService helpPageService;

        public SitemapController(ILogger<SitemapController> logger, IHelpPageService helpPageService)
        {
            this.logger = logger;
            this.helpPageService = helpPageService;
        }

        [HttpGet]
        public async Task<ContentResult> Sitemap()
        {
            try
            {
                logger.LogInformation("Generating Sitemap");

                var sitemapUrlPrefix = $"{Request.Scheme}://{Request.Host}/{PagesController.HelpPathRoot}";
                var sitemap = new Sitemap();

                // add the defaults
                sitemap.Add(new SitemapLocation()
                {
                    Url = sitemapUrlPrefix,
                    Priority = 1,
                });

                var helpPageModels = await helpPageService.GetAllAsync().ConfigureAwait(false);

                if (helpPageModels?.Count() > 0)
                {
                    var sitemapHelpPageModels = helpPageModels
                         .Where(w => w.IncludeInSitemap && !w.CanonicalName.Equals(PagesController.DefaultArticleName, StringComparison.OrdinalIgnoreCase))
                         .OrderBy(o => o.CanonicalName);

                    foreach (var helpPageModel in sitemapHelpPageModels)
                    {
                        sitemap.Add(new SitemapLocation()
                        {
                            Url = $"{sitemapUrlPrefix}/{helpPageModel.CanonicalName}",
                            Priority = 1,
                        });
                    }
                }

                // extract the sitemap
                string xmlString = sitemap.WriteSitemapToString();

                logger.LogInformation("Generated Sitemap");

                return Content(xmlString, MediaTypeNames.Application.Xml);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(Sitemap)}: {ex.Message}");
            }

            return null;
        }
    }
}
