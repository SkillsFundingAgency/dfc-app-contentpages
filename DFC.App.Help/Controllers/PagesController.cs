using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DFC.App.Help.Data;
using DFC.App.Help.Data.Contracts;
using DFC.App.Help.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DFC.App.Help.Controllers
{
    public class PagesController : BaseController
    {
        public const string HelpPathRoot = "help";
        public const string DefaultArticleName = "help";

        private readonly IHelpPageService _helpPageService;

        public PagesController(IHelpPageService helpPageService, AutoMapper.IMapper mapper) : base(mapper)
        {
            _helpPageService = helpPageService;
        }

        [HttpGet]
        [Route("pages")]
        public async Task<IActionResult> Index()
        {
            var viewModel = new IndexViewModel();
            var helpPageModels = await _helpPageService.GetAllAsync();

            if (helpPageModels != null)
            {
                viewModel.Documents = (from a in helpPageModels.OrderBy(o => o.CanonicalName)
                                       select _mapper.Map<IndexDocumentViewModel>(a)
                );
            }

            return NegotiateContentResult(viewModel);
        }

        [HttpGet]
        [Route("pages/{article}")]
        public async Task<IActionResult> Document(string article)
        {
            var helpPageModel = await GetHelpPageAsync(article);

            if (helpPageModel != null)
            {
                var viewModel = _mapper.Map<DocumentViewModel>(helpPageModel);

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

            var existingHelpPageModel = await _helpPageService.GetByIdAsync(helpPageModel.DocumentId);

            if (existingHelpPageModel == null)
            {
                var createdResponse = await _helpPageService.CreateAsync(helpPageModel);

                return new CreatedAtActionResult(nameof(Document), "Pages", new { article = createdResponse.CanonicalName }, createdResponse);
            }
            else
            {
                var updatedResponse = await _helpPageService.ReplaceAsync(helpPageModel);

                return new OkObjectResult(updatedResponse);
            }
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

        [HttpGet]
        [Route("pages/{article}/htmlhead")]
        [Route("pages/htmlhead")]
        public async Task<IActionResult> Head(string article)
        {
            var viewModel = new HeadViewModel();
            var helpPageModel = await GetHelpPageAsync(article);

            if (helpPageModel != null)
            {
                _mapper.Map(helpPageModel, viewModel);

                viewModel.CanonicalUrl = $"{Request.Scheme}://{Request.Host}/{HelpPathRoot}/{helpPageModel.CanonicalName}";
            }

            return NegotiateContentResult(viewModel);
        }

        [Route("pages/{article}/breadcrumb")]
        [Route("pages/breadcrumb")]
        public async Task<IActionResult> Breadcrumb(string article)
        {
            var helpPageModel = await GetHelpPageAsync(article);
            var viewModel = BuildBreadcrumb(helpPageModel);

            return NegotiateContentResult(viewModel);
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
            var helpPageModel = await GetHelpPageAsync(article);

            if (helpPageModel != null)
            {
                _mapper.Map(helpPageModel, viewModel);
            }
            else
            {
                var alternateHelpPageModel = await GetAlternativeHelpPageAsync(article);

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
        public IActionResult BodyFooter(string article)
        {
            return NoContent();
        }

        #region Define helper methods

        private async Task<HelpPageModel> GetHelpPageAsync(string article)
        {
            string name = !string.IsNullOrWhiteSpace(article) ? article : DefaultArticleName;

            var helpPageModel = await _helpPageService.GetByNameAsync(name);

            return helpPageModel;
        }

        private async Task<HelpPageModel> GetAlternativeHelpPageAsync(string article)
        {
            string name = (!string.IsNullOrWhiteSpace(article) ? article : DefaultArticleName);

            var helpPageModel = await _helpPageService.GetByAlternativeNameAsync(name);

            return helpPageModel;
        }

        private BreadcrumbViewModel BuildBreadcrumb(HelpPageModel helpPageModel)
        {
            var viewModel = new BreadcrumbViewModel
            {
                Paths = new List<BreadcrumbPathViewModel>() {
                    new BreadcrumbPathViewModel()
                    {
                        Route = "/",
                        Title = "Home"
                    },
                    new BreadcrumbPathViewModel()
                    {
                        Route = $"/{HelpPathRoot}",
                        Title = "Help"
                    }
                }
            };

            if (helpPageModel != null && string.Compare(helpPageModel.CanonicalName, DefaultArticleName, true) != 0)
            {
                var articlePathViewModel = new BreadcrumbPathViewModel()
                {
                    Route = $"/{HelpPathRoot}/{helpPageModel.CanonicalName}",
                    Title = helpPageModel.BreadcrumbTitle
                };

                viewModel.Paths.Add(articlePathViewModel);
            }

            viewModel.Paths.Last().AddHyperlink = false;

            return viewModel;
        }

        #endregion

    }
}
