using DFC.App.Help.Data;
using DFC.App.Help.Data.Contracts;
using DFC.App.Help.Extensions;
using DFC.App.Help.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.App.Help.Controllers
{
    public class PagesController : BaseController
    {
        public const string HelpPathRoot = "help";
        public const string DefaultArticleName = "help";

        private readonly IHelpPageService helpPageService;

        public PagesController(IHelpPageService helpPageService, AutoMapper.IMapper mapper) : base(mapper)
        {
            this.helpPageService = helpPageService;
        }

        [HttpGet]
        [Route("pages")]
        public async Task<IActionResult> Index()
        {
            var viewModel = new IndexViewModel();
            var helpPageModels = await helpPageService.GetAllAsync().ConfigureAwait(false);

            if (helpPageModels != null)
            {
                viewModel.Documents = (from a in helpPageModels.OrderBy(o => o.CanonicalName)
                                       select mapper.Map<IndexDocumentViewModel>(a)).ToList();
            }

            return NegotiateContentResult(viewModel);
        }

        [HttpGet]
        [Route("pages/{article}")]
        public async Task<IActionResult> Document(string article)
        {
            var helpPageModel = await GetHelpPageAsync(article).ConfigureAwait(false);

            if (helpPageModel != null)
            {
                var viewModel = mapper.Map<DocumentViewModel>(helpPageModel);

                viewModel.Breadcrumb = BuildBreadcrumb(helpPageModel);

                return NegotiateContentResult(viewModel);
            }

            return NoContent();
        }

        [HttpPut]
        [HttpPost]
        [Route("pages")]
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

            var existingHelpPageModel = await helpPageService.GetByIdAsync(helpPageModel.DocumentId).ConfigureAwait(false);

            if (existingHelpPageModel == null)
            {
                var createdResponse = await helpPageService.CreateAsync(helpPageModel).ConfigureAwait(false);

                return new CreatedAtActionResult(nameof(Document), "Pages", new { article = createdResponse.CanonicalName }, createdResponse);
            }
            else
            {
                var updatedResponse = await helpPageService.ReplaceAsync(helpPageModel).ConfigureAwait(false);

                return new OkObjectResult(updatedResponse);
            }
        }

        [HttpDelete]
        [Route("pages/{documentId}")]
        public async Task<IActionResult> HelpDelete(Guid documentId)
        {
            var doc = await helpPageService.GetByIdAsync(documentId).ConfigureAwait(false);

            if (doc == null)
            {
                return NotFound();
            }

            await helpPageService.DeleteAsync(documentId).ConfigureAwait(false);
            return Ok();
        }

        [HttpGet]
        [Route("pages/{article}/htmlhead")]
        [Route("pages/htmlhead")]
        [Route("draft/{article}/htmlhead")]
        [Route("draft/htmlhead")]
        public async Task<IActionResult> Head(string article)
        {
            var viewModel = new HeadViewModel();
            var helpPageModel = await GetHelpPageAsync(article).ConfigureAwait(false);

            if (helpPageModel != null)
            {
                mapper.Map(helpPageModel, viewModel);

                viewModel.CanonicalUrl = $"{Request.Scheme}://{Request.Host}/{HelpPathRoot}/{helpPageModel.CanonicalName}";
            }

            return NegotiateContentResult(viewModel);
        }

        [Route("pages/{article}/breadcrumb")]
        [Route("pages/breadcrumb")]
        [Route("draft/{article}/breadcrumb")]
        [Route("draft/breadcrumb")]
        public async Task<IActionResult> Breadcrumb(string article)
        {
            var helpPageModel = await GetHelpPageAsync(article).ConfigureAwait(false);
            var viewModel = BuildBreadcrumb(helpPageModel);

            return NegotiateContentResult(viewModel);
        }

        [HttpGet]
        [Route("pages/{article}/bodytop")]
        [Route("pages/bodytop")]
        [Route("draft/{article}/bodytop")]
        [Route("draft/bodytop")]
        public IActionResult BodyTop(string article)
        {
            return NoContent();
        }

        [HttpGet]
        [Route("pages/{article}/contents")]
        [Route("pages/contents")]
        [Route("draft/{article}/contents")]
        [Route("draft/contents")]
        public async Task<IActionResult> Body(string article)
        {
            var viewModel = new BodyViewModel();
            var helpPageModel = await GetHelpPageAsync(article).ConfigureAwait(false);

            if (helpPageModel != null)
            {
                mapper.Map(helpPageModel, viewModel);
            }
            else
            {
                var alternateHelpPageModel = await GetAlternativeHelpPageAsync(article).ConfigureAwait(false);

                if (alternateHelpPageModel != null)
                {
                    var alternateUrl = $"{Request.Scheme}://{Request.Host}/{HelpPathRoot}/{alternateHelpPageModel.CanonicalName}";

                    return RedirectPermanentPreserveMethod(alternateUrl);
                }
            }

            return NegotiateContentResult(viewModel);
        }

        [HttpGet]
        [Route("pages/{article}/bodyfooter")]
        [Route("pages/bodyfooter")]
        [Route("draft/{article}/bodyfooter")]
        [Route("draft/bodyfooter")]
        public IActionResult BodyFooter(string article)
        {
            return NoContent();
        }

        #region Define helper methods

        private async Task<HelpPageModel> GetHelpPageAsync(string article)
        {
            var isDraft = Request.IsDraftRequest();
            var name = !string.IsNullOrWhiteSpace(article) ? article : DefaultArticleName;

            var helpPageModel = await helpPageService.GetByNameAsync(name, isDraft).ConfigureAwait(false);

            return helpPageModel;
        }

        private async Task<HelpPageModel> GetAlternativeHelpPageAsync(string article)
        {
            var name = !string.IsNullOrWhiteSpace(article) ? article : DefaultArticleName;

            var helpPageModel = await helpPageService.GetByAlternativeNameAsync(name).ConfigureAwait(false);

            return helpPageModel;
        }

        private static BreadcrumbViewModel BuildBreadcrumb(HelpPageModel helpPageModel)
        {
            var viewModel = new BreadcrumbViewModel
            {
                Paths = new List<BreadcrumbPathViewModel>()
                {
                    new BreadcrumbPathViewModel()
                    {
                        Route = "/",
                        Title = "Home",
                    },
                    new BreadcrumbPathViewModel
                    {
                        Route = $"/{HelpPathRoot}",
                        Title = "Help",
                    },
                },
            };

            if (helpPageModel?.CanonicalName != null && helpPageModel.CanonicalName.Equals(DefaultArticleName, StringComparison.OrdinalIgnoreCase))
            {
                var articlePathViewModel = new BreadcrumbPathViewModel
                {
                    Route = $"/{HelpPathRoot}/{helpPageModel.CanonicalName}",
                    Title = helpPageModel.BreadcrumbTitle,
                };

                viewModel.Paths.Add(articlePathViewModel);
            }

            viewModel.Paths.Last().AddHyperlink = false;

            return viewModel;
        }

        #endregion Define helper methods
    }
}