using DFC.App.ContentPages.Extensions;
using DFC.App.ContentPages.Models;
using DFC.App.ContentPages.PageService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace DFC.App.ContentPages.Controllers
{
    public class SitemapController : Controller
    {
        private readonly ILogger<SitemapController> logger;
        private readonly IContentPageService contentPageService;

        public SitemapController(ILogger<SitemapController> logger, IContentPageService contentPageService)
        {
            this.logger = logger;
            this.contentPageService = contentPageService;
        }

        [HttpGet]
        [Route("{category}/sitemap.xml")]
        public async Task<ContentResult> Sitemap(string category)
        {
            try
            {
                logger.LogInformation("Generating Sitemap");

                var sitemapUrlPrefix = $"{Request.GetBaseAddress()}";
                var sitemap = new Sitemap();

                // add the defaults
                sitemap.Add(new SitemapLocation
                {
                    Url = $"{sitemapUrlPrefix}{category}",
                    Priority = 1,
                });

                var contentPageModels = await contentPageService.GetAllAsync(category).ConfigureAwait(false);

                if (contentPageModels != null)
                {
                    var contentPageModelsList = contentPageModels.ToList();

                    if (contentPageModelsList.Any())
                    {
                        var sitemapcontentPageModels = contentPageModelsList
                             .Where(w => w.IncludeInSitemap && !w.CanonicalName.Equals(w.Category, StringComparison.OrdinalIgnoreCase))
                             .OrderBy(o => o.CanonicalName);

                        foreach (var contentPageModel in sitemapcontentPageModels)
                        {
                            sitemap.Add(new SitemapLocation
                            {
                                Url = $"{sitemapUrlPrefix}{contentPageModel.Category}/{contentPageModel.CanonicalName}",
                                Priority = 1,
                            });
                        }
                    }
                }

                // extract the sitemap
                var xmlString = sitemap.WriteSitemapToString();

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