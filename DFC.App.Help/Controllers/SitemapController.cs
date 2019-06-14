using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DFC.App.Help.Models;

namespace DFC.App.Help.Controllers
{
    public class SitemapController : Controller
    {
        private readonly ILogger<SitemapController> _logger;

        public SitemapController(ILogger<SitemapController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ContentResult Sitemap()
        {
            try
            {
                _logger.LogInformation("Generating Sitemap");

                const string helpControllerName = "Help";
                var sitemap = new Sitemap();

                // add the defaults
                sitemap.Add(new SitemapLocation() { Url = Url.Action(nameof(HelpController.Body), helpControllerName, null, Request.Scheme), Priority = 1 });

                foreach(var key in HelpController.HelpArticles.Keys)
                {
                    sitemap.Add(new SitemapLocation() { Url = Url.Action(nameof(HelpController.Body), helpControllerName, null, Request.Scheme) + $"/{key}", Priority = 1 });
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