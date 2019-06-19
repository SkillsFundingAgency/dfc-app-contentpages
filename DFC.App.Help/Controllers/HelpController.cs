using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DFC.App.Help.Models.Cosmos;
using DFC.App.Help.Services;
using DFC.App.Help.ViewModels;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;

namespace DFC.App.Help.Controllers
{
    public class HelpController : Controller
    {
        public const string HelpPathRoot = "help";
        private const string IndexArticleName = "index";

        private readonly IHelpPageService _helpPageService;

        public HelpController(IHelpPageService helpPageService)
        {
            _helpPageService = helpPageService;
        }

        [HttpGet]
        [Route("help/pages/{article}/htmlhead")]
        public async Task<IActionResult> Head(string article)
        {
            var vm = new HeadViewModel()
            {
                Contents = null
            };
            var helpPageModel = await GetHelpPageAsync(article);

            if (helpPageModel != null)
            {
                vm.Title = helpPageModel.Title;
                vm.Contents = GenerateMetaTags(helpPageModel.Metatags);
            }

            return View(vm);
        }

        [HttpGet]
        [Route("help/pages/{article}/breadcrumb")]
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

                vm.Paths.Last().IsLastItem = true;
            }

            return View(vm);
        }

        [HttpGet]
        [Route("help/pages/{article}/bodytop")]
        public async Task<IActionResult> BodyTop(string article)
        {
            var vm = new BodyTopViewModel();
            var helpPageModel = await GetHelpPageAsync(article);

            if (helpPageModel != null)
            {
                vm.Title = helpPageModel.Title;
                vm.Contents = null;
            }

            return View(vm);
        }

        [HttpGet]
        [Route("help/pages/{article}/contents")]
        public async Task<IActionResult> Body(string article)
        {
            var vm = new BodyViewModel();
            var helpPageModel = await GetHelpPageAsync(article);

            if (helpPageModel != null)
            {
                vm.Title = helpPageModel.Title;
                vm.Contents = new HtmlString(helpPageModel.Contents);
            }

            return View(vm);
        }

        [HttpGet]
        [Route("help/pages/{article}/bodyfooter")]
        public async Task<IActionResult> BodyFooter(string article)
        {
            var vm = new BodyFooterViewModel();
            var helpPageModel = await GetHelpPageAsync(article);

            if (helpPageModel != null)
            {
                vm.Title = helpPageModel.Title;
                vm.Contents = null;
            }

            return View(vm);
        }

        #region Define helper methods

        private async Task<HelpPageModel> GetHelpPageAsync(string article)
        {
            string name = (!string.IsNullOrWhiteSpace(article) ? article : IndexArticleName);

            var helpPageModel = await _helpPageService.GetByNameAsync(name);

            return helpPageModel;
        }

        private HtmlString GenerateMetaTags(Models.Cosmos.MetatagsModel metatagsModel)
        {
            if (metatagsModel != null)
            {
                var sb = new StringBuilder();

                if (!string.IsNullOrWhiteSpace(metatagsModel.Description))
                {
                    sb.AppendLine($"<meta name=\"description\" content=\"{ metatagsModel.Description}\">");
                }

                if (!string.IsNullOrWhiteSpace(metatagsModel.Keywords))
                {
                    sb.AppendLine($"<meta name=\"keywords\" content=\"{ metatagsModel.Keywords}\">");
                }

                return new HtmlString(sb.ToString());
            }

            return null;
        }

        #endregion

    }
}
