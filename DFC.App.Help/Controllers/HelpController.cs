using System;
using DFC.App.Help.Models;
using Microsoft.AspNetCore.Mvc;

namespace DFC.App.Help.Controllers
{
    public class HelpController : Controller
    {
        [HttpGet]
        public IActionResult Head()
        {
            return View();
        }

        [HttpGet]
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
        [Route("help/index")]
        public IActionResult Body()
        {
            return View();
        }

        [HttpGet]
        public IActionResult BodyFooter()
        {
            return View();
        }

        [HttpGet]
        [Route("help/information-sources")]
        public IActionResult InformationSources()
        {
            return View();
        }

        [HttpGet]
        [Route("help/privacy-and-cookies")]
        public IActionResult PrivacyAndCookies()
        {
            return View();
        }

        [HttpGet]
        [Route("help/terms-and-conditions")]
        public IActionResult TermsAndConditions()
        {
            return View();
        }
    }
}
