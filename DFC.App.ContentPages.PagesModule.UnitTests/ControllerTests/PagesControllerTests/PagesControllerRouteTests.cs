using AutoMapper;
using DFC.App.ContentPages.Controllers;
using DFC.App.ContentPages.Data;
using DFC.App.ContentPages.PageService;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContentPages.PagesModule.UnitTests.ControllerTests.PagesControllerTests
{
    [Trait("Category", "Pages Controller Unit Tests")]
    public class PagesControllerRouteTests
    {
        private readonly ILogger<PagesController> logger;
        private readonly IContentPageService fakeContentPageService;
        private readonly IMapper fakeMapper;

        public PagesControllerRouteTests()
        {
            logger = A.Fake<ILogger<PagesController>>();
            fakeContentPageService = A.Fake<IContentPageService>();
            fakeMapper = A.Fake<IMapper>();
        }

        public static IEnumerable<object[]> PagesRouteData => new List<object[]>
        {
            new object[] { "/pages/{category}/{article}/htmlhead", "a-category", "SomeArticle", "Head" },
            new object[] { "/pages/htmlhead", string.Empty, string.Empty, "Head" },
            new object[] { "/pages/{category}/{article}/breadcrumb", "a-category", "SomeArticle", "Breadcrumb" },
            new object[] { "/pages/breadcrumb", string.Empty, string.Empty, "Breadcrumb" },
            new object[] { "/pages/{category}/{article}/contents", "a-category", "SomeArticle", "Body" },
            new object[] { "/pages/contents", string.Empty, string.Empty, "Body" },
        };

        [Theory]
        [MemberData(nameof(PagesRouteData))]
        public async Task PagesControllerCallsContentPageServiceUsingPagesRoute(string route, string category, string article, string actionMethod)
        {
            // Arrange
            var controller = BuildController(route);
            var expectedResult = new ContentPageModel() { Content = "<h1>A document ({0})</h1>" };

            A.CallTo(() => fakeContentPageService.GetByNameAsync(A<string>.Ignored, A<string>.Ignored)).Returns(expectedResult);

            // Act
            var result = await RunControllerAction(controller, category, article, actionMethod).ConfigureAwait(false);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            A.CallTo(() => fakeContentPageService.GetByNameAsync(A<string>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();
            controller.Dispose();
        }

        private static async Task<IActionResult> RunControllerAction(PagesController controller, string category, string article, string actionName)
        {
            switch (actionName)
            {
                case "Head":
                    return await controller.Head(category, article).ConfigureAwait(false);

                case "Breadcrumb":
                    return await controller.Breadcrumb(category, article).ConfigureAwait(false);

                default:
                    return await controller.Body(category, article).ConfigureAwait(false);
            }
        }

        private PagesController BuildController(string route)
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = route;
            httpContext.Request.Headers[HeaderNames.Accept] = MediaTypeNames.Application.Json;

            return new PagesController(logger, fakeContentPageService, fakeMapper)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                },
            };
        }
    }
}