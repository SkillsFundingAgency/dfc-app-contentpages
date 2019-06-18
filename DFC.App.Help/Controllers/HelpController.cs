using System;
using System.Collections.Generic;
using DFC.App.Help.Models;
using DFC.App.Help.Services;
using DFC.App.Help.ViewModels;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;

namespace DFC.App.Help.Controllers
{
    public class HelpController : Controller
    {
        private readonly IHelpPageService _helpPageService;

        public HelpController(IHelpPageService helpPageService)
        {
            _helpPageService = helpPageService;
        }

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
                    paths = new ArraySegment<string>(paths, 0, paths.Length - 1).ToArray();
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
        public async System.Threading.Tasks.Task<IActionResult> Body(string data)
        {
            string name = (!string.IsNullOrWhiteSpace(data) ? data : "index");
            var vm = new DocumentViewModel()
            {
                Title = "Unknown Help document",
                Contents = new HtmlString("Unknown Help document")
            };

            var doc = await _helpPageService.GetByNameAsync(name);

            if (doc != null)
            {
                vm.Title = doc.Title;
                vm.Contents = new HtmlString(doc.Contents);
            }

            return View(vm);
        }

        [HttpGet]
        public IActionResult BodyFooter()
        {
            return View();
        }

    }
}
