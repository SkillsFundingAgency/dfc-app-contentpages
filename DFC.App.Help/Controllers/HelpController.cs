using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DFC.App.Help.Models;

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
        public IActionResult Breadcrumb()
        {
            return View();
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
