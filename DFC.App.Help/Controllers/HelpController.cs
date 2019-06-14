using System;
using System.Collections.Generic;
using DFC.App.Help.Models;
using Microsoft.AspNetCore.Mvc;

namespace DFC.App.Help.Controllers
{
    public class HelpController : Controller
    {
        public readonly static Dictionary<string, string> HelpArticles = new Dictionary<string, string>() {
                    { "information-sources", "InformationSources" },
                    { "privacy-and-cookies", "PrivacyAndCookies" },
                    { "terms-and-conditions", "TermsAndConditions" }
                };

        [HttpGet]
        public IActionResult Head()
        {
            return View();
        }

        [HttpGet]
        [Route("Help/Breadcrumb/{**data}")]
        public IActionResult Breadcrumb(string data)
        {
            string[] paths = null;
            string thisLocation = null;

            if (!string.IsNullOrEmpty(data))
            {
                paths = data.Split('/');

                if (paths.Length > 0)
                {
                    thisLocation = paths[paths.Length - 1];
                    paths = new ArraySegment<string>(paths,0,paths.Length-1).ToArray();
                }
            }

            var viewModel = new BreadcrumbViewModel()
            {
                Paths = paths,
                ThisLocation = thisLocation
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult BodyTop()
        {
            return View();
        }

        [HttpGet]
        [Route("help/{**data}")]
        public IActionResult Body(string data)
        {
            string viewName = nameof(Body);

            if (!string.IsNullOrEmpty(data))
            {
                string key = data.ToLower();

                if (HelpArticles.ContainsKey(key))
                {
                    viewName = HelpArticles[key];
                }
            }

            return View(viewName);
        }

        [HttpGet]
        public IActionResult BodyFooter()
        {
            return View();
        }

    }
}
