using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DFC.App.Help.Models.Cosmos;
using DFC.App.Help.Services;
using DFC.App.Help.ViewModels;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;

namespace DFC.App.Help.Controllers
{
    public class PagesController : BaseController
    {
        public const string HelpPathRoot = "help";
        private const string IndexArticleName = "index";

        private readonly IHelpPageService _helpPageService;

        public PagesController(IHelpPageService helpPageService)
        {
            _helpPageService = helpPageService;
        }

        [HttpGet]
        [Route("pages/{article}/htmlhead")]
        public async Task<IActionResult> Head(string article)
        {
            var vm = new HeadViewModel();
            var helpPageModel = await GetHelpPageAsync(article);

            if (helpPageModel != null)
            {
                vm.Title = helpPageModel.Title;
                vm.Description = helpPageModel.Metatags?.Description;
                vm.Keywords = helpPageModel.Metatags?.Keywords;
            }

            return NegotiateContentResult(vm);
        }

        [HttpDelete]
        [Route("pages/{article}")]
        public IActionResult HelpDelete(string article)
        {
            return StatusCode((int)HttpStatusCode.NotImplemented);
        }

        [HttpPut]
        [HttpPost]
        [Route("pages/help")]
        public async Task<IActionResult> HelpCreateOrUpdate([FromBody]HelpPageModel helpPageModel)
        {
            if (helpPageModel == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            HelpPageModel clientResponse = null;
            var existingHelpPageModel = await GetHelpPageAsync(helpPageModel.Name);
            if (existingHelpPageModel == null)
            {
                clientResponse = await _helpPageService.CreateAsync(helpPageModel);
            }
            else
            {
                clientResponse = await _helpPageService.ReplaceAsync(helpPageModel);
            }

            return new CreatedAtActionResult("Head", "Pages", new { article = clientResponse.Name }, clientResponse);
        }

        [HttpGet]
        [Route("pages/{article}/breadcrumb")]
        [Produces("text/html", "application/json")]
        public async Task<IActionResult> Breadcrumb(string article)
        {
            var vm = new BreadcrumbViewModel();
            var helpPageModel = await GetHelpPageAsync(article);

            if (!string.IsNullOrWhiteSpace(article))
            {
                vm.Paths = new List<BreadcrumbPathViewModel>() {
                    new BreadcrumbPathViewModel()
                    {
                        Route = "/",
                        Title = "Home"
                    },
                    new BreadcrumbPathViewModel()
                    {
                        Route = $"/{HelpPathRoot}/{IndexArticleName}",
                        Title = "Help"
                    }
                };

                if (helpPageModel != null && string.Compare(helpPageModel.Name, IndexArticleName, true) != 0)
                {
                    var articlePathViewModel = new BreadcrumbPathViewModel()
                    {
                        Route = $"/{HelpPathRoot}/{helpPageModel.Name}",
                        Title = helpPageModel.Title
                    };

                    vm.Paths.Add(articlePathViewModel);
                    vm.Title = helpPageModel.Title;
                }

                vm.Paths.Last().AddHyperlink = false;
            }

            return NegotiateContentResult(vm);
        }

        [HttpGet]
        [Route("pages/{article}/bodytop")]
        public IActionResult BodyTop(string article)
        {
            return NoContent();
        }

        [HttpGet]
        [Route("pages/{article}/contents")]
        public async Task<IActionResult> Body(string article)
        {
            var vm = new BodyViewModel();
            var helpPageModel = await GetHelpPageAsync(article);

            if (helpPageModel != null)
            {
                vm.Title = helpPageModel.Title;
                vm.Contents = new HtmlString(helpPageModel.Contents);
            }

            return NegotiateContentResult(vm);
        }

        [HttpGet]
        [Route("pages/{article}/bodyfooter")]
        public IActionResult BodyFooter(string article)
        {
            return NoContent();
        }

        #region Define helper methods

        private async Task<HelpPageModel> GetHelpPageAsync(string article)
        {
            string name = (!string.IsNullOrWhiteSpace(article) ? article : IndexArticleName);

            var helpPageModel = await _helpPageService.GetByNameAsync(name);

            return helpPageModel;
        }

        #endregion

    }
}
