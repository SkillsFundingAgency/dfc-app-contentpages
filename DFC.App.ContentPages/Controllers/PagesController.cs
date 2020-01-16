using DFC.App.ContentPages.Data.Models;
using DFC.App.ContentPages.Extensions;
using DFC.App.ContentPages.PageService;
using DFC.App.ContentPages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.ContentPages.Controllers
{
    public class PagesController : Controller
    {
        public const string CategoryNameForAlert = "alerts";
        public const string CategoryNameForHelp = "help";

        private readonly ILogger<PagesController> logger;
        private readonly IContentPageService contentPageService;
        private readonly AutoMapper.IMapper mapper;

        public PagesController(ILogger<PagesController> logger, IContentPageService contentPageService, AutoMapper.IMapper mapper)
        {
            this.logger = logger;
            this.contentPageService = contentPageService;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("/")]
        [Route("pages")]
        public async Task<IActionResult> Index()
        {
            logger.LogInformation($"{nameof(Index)} has been called");

            var viewModel = new IndexViewModel();
            var contentPageModels = await contentPageService.GetAllAsync().ConfigureAwait(false);

            if (contentPageModels != null)
            {
                viewModel.Documents = (from a in contentPageModels.OrderBy(o => o.Category).ThenBy(o => o.CanonicalName)
                                       select mapper.Map<IndexDocumentViewModel>(a)).ToList();
                logger.LogInformation($"{nameof(Index)} has succeeded");
            }
            else
            {
                logger.LogWarning($"{nameof(Index)} has returned with no results");
            }

            return this.NegotiateContentResult(viewModel);
        }

        [HttpGet]
        [Route("pages/{category}/{article}")]
        public async Task<IActionResult> Document(string category, string article)
        {
            logger.LogInformation($"{nameof(Document)} has been called");

            var contentPageModel = await GetContentPageAsync(category, article).ConfigureAwait(false);

            if (contentPageModel != null)
            {
                var viewModel = mapper.Map<DocumentViewModel>(contentPageModel);

                viewModel.Breadcrumb = BuildBreadcrumb(contentPageModel);

                logger.LogInformation($"{nameof(Document)} has succeeded for: {category}/{article}");

                return this.NegotiateContentResult(viewModel);
            }

            logger.LogWarning($"{nameof(Document)} has returned no content for: {category}/{article}");

            return NoContent();
        }

        [HttpGet]
        [Route("pages/{category}/{article}/htmlhead")]
        [Route("pages/{category}/htmlhead")]
        public async Task<IActionResult> Head(string category, string article)
        {
            logger.LogInformation($"{nameof(Head)} has been called");

            var viewModel = new HeadViewModel();
            var contentPageModel = await GetContentPageAsync(category, article).ConfigureAwait(false);

            if (contentPageModel != null)
            {
                mapper.Map(contentPageModel, viewModel);

                viewModel.CanonicalUrl = $"{Request.GetBaseAddress()}{contentPageModel.Category}/{contentPageModel.CanonicalName}";
            }

            logger.LogInformation($"{nameof(Head)} has returned content for: {category}/{article}");

            return this.NegotiateContentResult(viewModel);
        }

        [Route("pages/{category}/{article}/breadcrumb")]
        [Route("pages/{category}/breadcrumb")]
        public async Task<IActionResult> Breadcrumb(string category, string article)
        {
            logger.LogInformation($"{nameof(Breadcrumb)} has been called");

            var contentPageModel = await GetContentPageAsync(category, article).ConfigureAwait(false);
            var viewModel = BuildBreadcrumb(contentPageModel);

            logger.LogInformation($"{nameof(Breadcrumb)} has returned content for: {category}/{article}");

            return this.NegotiateContentResult(viewModel);
        }

        [HttpGet]
        [Route("pages/{category}/{article}/bodytop")]
        [Route("pages/{category}/bodytop")]
        public IActionResult BodyTop(string category, string article)
        {
            return NoContent();
        }

        [HttpGet]
        [Route("pages/{category}/{article}/contents")]
        [Route("pages/{category}/contents")]
        public async Task<IActionResult> Body(string category, string article)
        {
            const string ArticlePlaceholder = "{0}";

            logger.LogInformation($"{nameof(Body)} has been called");

            var viewModel = new BodyViewModel();
            var contentPageModel = await GetContentPageAsync(category, article).ConfigureAwait(false);

            if (contentPageModel != null)
            {
                contentPageModel.Content = contentPageModel.Content.Replace(ArticlePlaceholder, article, StringComparison.OrdinalIgnoreCase);

                mapper.Map(contentPageModel, viewModel);
                logger.LogInformation($"{nameof(Body)} has returned content for: {article}");

                return this.NegotiateContentResult(viewModel, contentPageModel);
            }

            var alternateContentPageModel = await GetAlternativeContentPageAsync(category, article).ConfigureAwait(false);
            if (alternateContentPageModel != null)
            {
                var alternateUrl = $"{Request.GetBaseAddress()}{alternateContentPageModel.Category}/{alternateContentPageModel.CanonicalName}";
                logger.LogWarning($"{nameof(Body)} has been redirected for: {category}/{article} to {alternateUrl}");

                return RedirectPermanentPreserveMethod(alternateUrl);
            }

            logger.LogWarning($"{nameof(Body)} has not returned any content for: {category}/{article}");
            return NotFound();
        }

        [HttpGet]
        [Route("pages/{category}/{article}/bodyfooter")]
        [Route("pages/{category}/bodyfooter")]
        public IActionResult BodyFooter(string category, string article)
        {
            return NoContent();
        }

        [HttpPost]
        [Route("pages")]
        public async Task<IActionResult> Create([FromBody]ContentPageModel upsertContentPageModel)
        {
            logger.LogInformation($"{nameof(Create)} has been called");

            if (upsertContentPageModel == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingDocument = await contentPageService.GetByIdAsync(upsertContentPageModel.DocumentId).ConfigureAwait(false);
            if (existingDocument != null)
            {
                return new StatusCodeResult((int)HttpStatusCode.AlreadyReported);
            }

            var response = await contentPageService.UpsertAsync(upsertContentPageModel).ConfigureAwait(false);

            logger.LogInformation($"{nameof(Create)} has upserted content for: {upsertContentPageModel.Category}/{upsertContentPageModel.CanonicalName} with response code {response}");

            return new StatusCodeResult((int)response);
        }

        [HttpPut]
        [Route("pages")]
        public async Task<IActionResult> Update([FromBody]ContentPageModel upsertContentPageModel)
        {
            logger.LogInformation($"{nameof(Update)} has been called");

            if (upsertContentPageModel == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingDocument = await contentPageService.GetByIdAsync(upsertContentPageModel.DocumentId).ConfigureAwait(false);
            if (existingDocument == null)
            {
                return new StatusCodeResult((int)HttpStatusCode.NotFound);
            }

            if (upsertContentPageModel.SequenceNumber <= existingDocument.SequenceNumber)
            {
                return new StatusCodeResult((int)HttpStatusCode.AlreadyReported);
            }

            upsertContentPageModel.Etag = existingDocument.Etag;
            upsertContentPageModel.Category = existingDocument.Category;

            var response = await contentPageService.UpsertAsync(upsertContentPageModel).ConfigureAwait(false);

            logger.LogInformation($"{nameof(Update)} has upserted content for: {upsertContentPageModel.Category}/{upsertContentPageModel.CanonicalName} with response code {response}");

            return new StatusCodeResult((int)response);
        }

        [HttpDelete]
        [Route("pages/{documentId}")]
        public async Task<IActionResult> Delete(Guid documentId)
        {
            logger.LogInformation($"{nameof(Delete)} has been called");

            var isDeleted = await contentPageService.DeleteAsync(documentId).ConfigureAwait(false);
            if (isDeleted)
            {
                logger.LogInformation($"{nameof(Delete)} has deleted content for document Id: {documentId}");
                return Ok();
            }
            else
            {
                logger.LogWarning($"{nameof(Delete)} has returned no content for: {documentId}");
                return NotFound();
            }
        }

        #region Define helper methods

        private static bool IsAlertCategory(string category)
        {
            return string.Compare(category, CategoryNameForAlert, true, CultureInfo.InvariantCulture) == 0;
        }

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
                },
            };

            if (contentPageModel != null && !IsAlertCategory(contentPageModel.Category))
            {
                var articlePathViewModel = new BreadcrumbPathViewModel
                {
                    Route = $"/{contentPageModel.Category}",
                    Title = contentPageModel.Category,
                };

                viewModel.Paths.Add(articlePathViewModel);
            }

            if (contentPageModel?.CanonicalName != null && !contentPageModel.CanonicalName.Equals(contentPageModel.Category, StringComparison.OrdinalIgnoreCase))
            {
                var articlePathViewModel = new BreadcrumbPathViewModel
                {
                    Route = $"/{contentPageModel.Category}/{contentPageModel.CanonicalName}",
                    Title = contentPageModel.BreadcrumbTitle,
                };

                viewModel.Paths.Add(articlePathViewModel);
            }

            viewModel.Paths.Last().AddHyperlink = false;

            return viewModel;
        }

        private async Task<ContentPageModel> GetContentPageAsync(string category, string article)
        {
            var name = !string.IsNullOrWhiteSpace(article) ? article : category;

            var contentPageModel = await contentPageService.GetByNameAsync(category, name).ConfigureAwait(false);

            return contentPageModel;
        }

        private async Task<ContentPageModel> GetAlternativeContentPageAsync(string category, string article)
        {
            var name = !string.IsNullOrWhiteSpace(article) ? article : category;

            var contentPageModel = await contentPageService.GetByAlternativeNameAsync(category, name).ConfigureAwait(false);

            return contentPageModel;
        }

        #endregion Define helper methods
    }
}