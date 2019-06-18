using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DFC.App.Help.Models;
using DFC.App.Help.Services;

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

                const string helpControllerName = "Help";
                var sitemap = new Sitemap();

                // add the defaults
                sitemap.Add(new SitemapLocation() { Url = Url.Action(nameof(HelpController.Body), helpControllerName, null, Request.Scheme), Priority = 1 });

                var helpPageModels = await _helpPageService.GetListAsync();

                if (helpPageModels?.Count > 0)
                {
                    foreach (var helpPageModel in helpPageModels.Where(w => w.IncludeInSitemap).OrderBy(o => o.Name))
                    {
                        sitemap.Add(new SitemapLocation() { Url = Url.Action(nameof(HelpController.Body), helpControllerName, null, Request.Scheme) + $"/{helpPageModel.Name}", Priority = 1 });
                    }
                }

                // extract the sitemap
                string xmlString = sitemap.WriteSitemapToString();

                _logger.LogInformation("Generated Sitemap");

                return Content(xmlString, "application/xml");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(Sitemap)}: {ex.Message}");
            }

            return null;
        }
    }
}