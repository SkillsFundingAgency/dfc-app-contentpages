using DFC.App.ContentPages.Data;
using DFC.App.ContentPages.Data.Contracts;
using DFC.App.ContentPages.Extensions;
using DFC.App.ContentPages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.App.ContentPages.Controllers
{
    public class PagesController : Controller
    {
        public const string HelpPathRoot = "help";
        public const string DefaultArticleName = "help";

        private readonly IContentPageService contentPageService;
        private readonly AutoMapper.IMapper mapper;

        public PagesController(IContentPageService contentPageService, AutoMapper.IMapper mapper)
        {
            this.contentPageService = contentPageService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var viewModel = new IndexViewModel();
            var contentPageModels = await contentPageService.GetAllAsync().ConfigureAwait(false);

            if (contentPageModels != null)
            {
                viewModel.Documents = (from a in contentPageModels.OrderBy(o => o.CanonicalName)
                                       select mapper.Map<IndexDocumentViewModel>(a)).ToList();
            }

            return this.NegotiateContentResult(viewModel);
        }

        [HttpGet]
        [Route("pages/{article}")]
        public async Task<IActionResult> Document(string article)
        {
            var contentPageModel = await GetContentPageAsync(article).ConfigureAwait(false);

            if (contentPageModel != null)
            {
                var viewModel = mapper.Map<DocumentViewModel>(contentPageModel);

                viewModel.Breadcrumb = BuildBreadcrumb(contentPageModel);

                return this.NegotiateContentResult(viewModel);
            }

            return NoContent();
        }

        [HttpDelete]
        [Route("pages/{documentId}")]
        public async Task<IActionResult> HelpDelete(Guid documentId)
        {
            var contentPageModel = await contentPageService.GetByIdAsync(documentId).ConfigureAwait(false);

            if (contentPageModel == null)
            {
                return NotFound();
            }

            await contentPageService.DeleteAsync(documentId).ConfigureAwait(false);

            return Ok();
        }

        [HttpGet]
        [Route("pages/{article}/htmlhead")]
        [Route("pages/htmlhead")]
        public async Task<IActionResult> Head(string article)
        {
            var viewModel = new HeadViewModel();
            var contentPageModel = await GetContentPageAsync(article).ConfigureAwait(false);

            if (contentPageModel != null)
            {
                mapper.Map(contentPageModel, viewModel);

                viewModel.CanonicalUrl = $"{Request.Scheme}://{Request.Host}/{HelpPathRoot}/{contentPageModel.CanonicalName}";
            }

            return this.NegotiateContentResult(viewModel);
        }

        [Route("pages/{article}/breadcrumb")]
        [Route("pages/breadcrumb")]
        public async Task<IActionResult> Breadcrumb(string article)
        {
            var contentPageModel = await GetContentPageAsync(article).ConfigureAwait(false);
            var viewModel = BuildBreadcrumb(contentPageModel);

            return this.NegotiateContentResult(viewModel);
        }

        [HttpGet]
        [Route("pages/{article}/bodytop")]
        [Route("pages/bodytop")]
        public IActionResult BodyTop(string article)
        {
            return NoContent();
        }

        [HttpGet]
        [Route("pages/{article}/contents")]
        [Route("pages/contents")]
        public async Task<IActionResult> Body(string article)
        {
            var viewModel = new BodyViewModel();
            var contentPageModel = await GetContentPageAsync(article).ConfigureAwait(false);

            if (contentPageModel != null)
            {
                mapper.Map(contentPageModel, viewModel);
            }
            else
            {
                var alternatecontentPageModel = await GetAlternativeContentPageAsync(article).ConfigureAwait(false);

                if (alternatecontentPageModel != null)
                {
                    var alternateUrl = $"{Request.Scheme}://{Request.Host}/{HelpPathRoot}/{alternatecontentPageModel.CanonicalName}";

                    return RedirectPermanentPreserveMethod(alternateUrl);
                }
            }

            return this.NegotiateContentResult(viewModel);
        }

        [HttpGet]
        [Route("pages/{article}/bodyfooter")]
        [Route("pages/bodyfooter")]
        public IActionResult BodyFooter(string article)
        {
            return NoContent();
        }

        #region Define helper methods

        private static BreadcrumbViewModel BuildBreadcrumb(ContentPageModel contentPageModel)
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

            if (contentPageModel?.CanonicalName != null && !contentPageModel.CanonicalName.Equals(DefaultArticleName, StringComparison.OrdinalIgnoreCase))
            {
                var articlePathViewModel = new BreadcrumbPathViewModel
                {
                    Route = $"/{HelpPathRoot}/{contentPageModel.CanonicalName}",
                    Title = contentPageModel.BreadcrumbTitle,
                };

                viewModel.Paths.Add(articlePathViewModel);
            }

            viewModel.Paths.Last().AddHyperlink = false;

            return viewModel;
        }

        private async Task<ContentPageModel> GetContentPageAsync(string article)
        {
            var name = !string.IsNullOrWhiteSpace(article) ? article : DefaultArticleName;

            var contentPageModel = await contentPageService.GetByNameAsync(name).ConfigureAwait(false);

            return contentPageModel;
        }

        private async Task<ContentPageModel> GetAlternativeContentPageAsync(string article)
        {
            var name = !string.IsNullOrWhiteSpace(article) ? article : DefaultArticleName;

            var contentPageModel = await contentPageService.GetByAlternativeNameAsync(name).ConfigureAwait(false);

            return contentPageModel;
        }

        #endregion Define helper methods
    }
}