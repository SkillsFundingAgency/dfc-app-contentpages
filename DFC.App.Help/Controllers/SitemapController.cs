using System;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using DFC.App.Help.Data.Contracts;
using DFC.App.Help.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DFC.App.Help.Controllers
{
    public class SitemapController : Controller
    {
        private readonly ILogger<SitemapController> _logger;
        private readonly IHelpPageService _helpPageService;

        public SitemapController(ILogger<SitemapController> logger, IHelpPageService helpPageService)
        {
            _logger = logger;
            _helpPageService = helpPageService;
        }

        [HttpGet]
        public async Task<ContentResult> Sitemap()
        {
            try
            {
                _logger.LogInformation("Generating Sitemap");

                var sitemapUrlPrefix = $"{Request.Scheme}://{Request.Host}/{PagesController.HelpPathRoot}";
                var sitemap = new Sitemap();

                // add the defaults
                sitemap.Add(new SitemapLocation()
                {
                    Url = sitemapUrlPrefix,
                    Priority = 1
                });

                var helpPageModels = await _helpPageService.GetAllAsync();

                if (helpPageModels?.Count() > 0)
                {
                    foreach (var helpPageModel in helpPageModels.Where(w => w.IncludeInSitemap && string.Compare(w.CanonicalName, PagesController.DefaultArticleName, true) != 0).OrderBy(o => o.CanonicalName))
                    {
                        sitemap.Add(new SitemapLocation()
                        {
                            Url = $"{sitemapUrlPrefix}/{helpPageModel.CanonicalName}",
                            Priority = 1
                        });
                    }
                }

                // extract the sitemap
                string xmlString = sitemap.WriteSitemapToString();

                _logger.LogInformation("Generated Sitemap");

                return Content(xmlString, MediaTypeNames.Application.Xml);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(Sitemap)}: {ex.Message}");
            }

            return null;
        }
    }
}
