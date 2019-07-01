using System;
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
        [Route("pages")]
        public async Task<IActionResult> Index()
        {
            var vm = new IndexViewModel();
            var helpPageModels = await _helpPageService.GetListAsync();

            if (helpPageModels != null)
            {
                vm.Documents = (from a in helpPageModels.OrderBy(o => o.CanonicalName)
                                select new IndexDocumentViewModel()
                                {
                                    CanonicalName = a.CanonicalName
                                }
                );
            }

            return NegotiateContentResult(vm);
        }

        [HttpGet]
        [Route("pages/{article}")]
        public async Task<IActionResult> Document(string article)
        {
            var helpPageModel = await GetHelpPageAsync(article);

            if (helpPageModel != null)
            {
                var vm = new DocumentViewModel
                {
                    Breadcrumb = BuildBreadcrumb(helpPageModel),

                    DocumentId = helpPageModel.DocumentId,
                    CanonicalName = helpPageModel.CanonicalName,
                    Title = helpPageModel.BreadcrumbTitle,
                    IncludeInSitemap = helpPageModel.IncludeInSitemap,
                    Description = helpPageModel.MetaTags?.Description,
                    Keywords = helpPageModel.MetaTags?.Keywords,
                    Content = new HtmlString(helpPageModel.Content),
                    LastReviewed = helpPageModel.LastReviewed,
                    AlternativeNames = helpPageModel.AlternativeNames
                };

                return NegotiateContentResult(vm);
            }

            return NoContent();
        }

        [HttpGet]
        [Route("pages/{article}/htmlhead")]
        public async Task<IActionResult> Head(string article)
        {
            var vm = new HeadViewModel();
            var helpPageModel = await GetHelpPageAsync(article);

            if (helpPageModel != null)
            {
                vm.CanonicalName = helpPageModel.CanonicalName;
                vm.Title = helpPageModel.BreadcrumbTitle;
                vm.Description = helpPageModel.MetaTags?.Description;
                vm.Keywords = helpPageModel.MetaTags?.Keywords;
            }

            return NegotiateContentResult(vm);
        }

        [HttpDelete]
        [Route("pages/{documentId}")]
        public async Task<IActionResult> HelpDelete(Guid documentId)
        {
            var doc = await _helpPageService.GetByIdAsync(documentId);
            if (doc == null)
            {
                return NotFound();
            }

            await _helpPageService.DeleteAsync(documentId);
            return Ok();
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

            var existingHelpPageModel = await _helpPageService.GetByIdAsync(helpPageModel.DocumentId);

            if (existingHelpPageModel == null)
            {
                if (helpPageModel.DocumentId == Guid.Empty)
                {
                    helpPageModel.DocumentId = Guid.NewGuid();
                }

                var createdResponse = await _helpPageService.CreateAsync(helpPageModel);

                return new CreatedAtActionResult(nameof(Document), "Pages", new { article = createdResponse.CanonicalName }, createdResponse);
            }
            else
            {
                var updatedResponse = await _helpPageService.ReplaceAsync(helpPageModel);

                return new OkObjectResult(updatedResponse);
            }
        }

        [HttpGet]
        [Route("pages/{article}/breadcrumb")]
        [Produces("text/html", "application/json")]
        public async Task<IActionResult> Breadcrumb(string article)
        {
            var helpPageModel = await GetHelpPageAsync(article);
            var vm = BuildBreadcrumb(helpPageModel);

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
                vm.Content = new HtmlString(helpPageModel.Content);
            }
            else
            {
                var alternateHelpPageModel = await GetAlyernativeHelpPageAsync(article);

                if (alternateHelpPageModel != null)
                {
                    var alternateUrl = $"{Request.Scheme}://{Request.Host}/{this.ControllerContext.ActionDescriptor.ControllerName}/{alternateHelpPageModel.CanonicalName}";

                    return RedirectPermanent(alternateUrl);
                }
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

        private async Task<HelpPageModel> GetAlyernativeHelpPageAsync(string article)
        {
            string name = (!string.IsNullOrWhiteSpace(article) ? article : IndexArticleName);

            var helpPageModel = await _helpPageService.GetByAlternativeNameAsync(name);

            return helpPageModel;
        }

        private BreadcrumbViewModel BuildBreadcrumb(HelpPageModel helpPageModel)
        {
            var vm = new BreadcrumbViewModel
            {
                Paths = new List<BreadcrumbPathViewModel>() {
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
                }
            };

            if (helpPageModel != null && string.Compare(helpPageModel.CanonicalName, IndexArticleName, true) != 0)
            {
                var articlePathViewModel = new BreadcrumbPathViewModel()
                {
                    Route = $"/{HelpPathRoot}/{helpPageModel.CanonicalName}",
                    Title = helpPageModel.BreadcrumbTitle
                };

                vm.Paths.Add(articlePathViewModel);
            }

            vm.Paths.Last().AddHyperlink = false;

            return vm;
        }

        #endregion

    }
}
