using AutoMapper;
using DFC.App.Help.Controllers;
using DFC.App.Help.Data.Contracts;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.Help.PagesModule.UnitTests.ControllerTests.PagesControllerTests
{
    public class PagesControllerDraftTests
    {
        private readonly IHelpPageService fakeHelpPageService;
        private readonly IMapper fakeMapper;

        public PagesControllerDraftTests()
        {
            this.fakeHelpPageService = A.Fake<IHelpPageService>();
            this.fakeMapper = A.Fake<IMapper>();
        }

        [Theory]
        [MemberData(nameof(DraftRouteData))]
        public async void PagesControllerCallsHelpPageServiceWithIsDraftTrueWhenUsingDraftRoute(string route, string article, string actionMethod)
        {
            var controller = BuildController(route);

            var result = await RunControllerAction(controller, article, actionMethod).ConfigureAwait(false);

            Assert.IsType<OkObjectResult>(result);
            A.CallTo(() => fakeHelpPageService.GetByNameAsync(A<string>.Ignored, true)).MustHaveHappenedOnceExactly();
            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(NonDraftRouteData))]
        public async void PagesControllerCallsHelpPageServiceWithIsDraftFalseWhenUsingNonDraftRoute(string route, string article, string actionMethod)
        {
            var controller = BuildController(route);

            var result = await RunControllerAction(controller, article, actionMethod).ConfigureAwait(false);

            Assert.IsType<OkObjectResult>(result);
            A.CallTo(() => fakeHelpPageService.GetByNameAsync(A<string>.Ignored, false)).MustHaveHappenedOnceExactly();
            controller.Dispose();
        }

        private static async Task<IActionResult> RunControllerAction(PagesController controller, string article, string actionName)
        {
            switch (actionName)
            {
                case "Head":
                    return await controller.Head(article).ConfigureAwait(false);

                case "Breadcrumb":
                    return await controller.Breadcrumb(article).ConfigureAwait(false);

                default:
                    return await controller.Body(article).ConfigureAwait(false);
            }
        }

        private PagesController BuildController(string route)
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = route;
            httpContext.Request.Headers[HeaderNames.Accept] = MediaTypeNames.Application.Json;

            return new PagesController(fakeHelpPageService, fakeMapper)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext
                }
            };
        }

        public static IEnumerable<object[]> DraftRouteData => new List<object[]>
        {
            new object[] { "/draft/{article}/htmlhead", "SomeArticle", "Head" },
            new object[] { "/draft/htmlhead", string.Empty, "Head" },
            new object[] { "/draft/{article}/breadcrumb", "SomeArticle", "Breadcrumb" },
            new object[] { "/draft/breadcrumb", string.Empty, "Breadcrumb" },
            new object[] { "/draft/{article}/contents", "SomeArticle", "Body" },
            new object[] { "/draft/contents", string.Empty, "Body" }
        };

        public static IEnumerable<object[]> NonDraftRouteData => new List<object[]>
        {
            new object[] { "/pages/{article}/htmlhead", "SomeArticle", "Head" },
            new object[] { "/pages/htmlhead", string.Empty, "Head" },
            new object[] { "/pages/{article}/breadcrumb", "SomeArticle", "Breadcrumb" },
            new object[] { "/pages/breadcrumb", string.Empty, "Breadcrumb" },
            new object[] { "/pages/{article}/contents", "SomeArticle", "Body" },
            new object[] { "/pages/contents", string.Empty, "Body" }
        };
    }
}