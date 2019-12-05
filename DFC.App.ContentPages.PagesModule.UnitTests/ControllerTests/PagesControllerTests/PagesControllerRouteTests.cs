using AutoMapper;
using DFC.App.ContentPages.Controllers;
using DFC.App.ContentPages.Data.Contracts;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IContentPageService fakeContentPageService;
        private readonly IMapper fakeMapper;

        public PagesControllerRouteTests()
        {
            fakeContentPageService = A.Fake<IContentPageService>();
            fakeMapper = A.Fake<IMapper>();
        }

        public static IEnumerable<object[]> PagesRouteData => new List<object[]>
        {
            new object[] { "/pages/{article}/htmlhead", "SomeArticle", "Head" },
            new object[] { "/pages/htmlhead", string.Empty, "Head" },
            new object[] { "/pages/{article}/breadcrumb", "SomeArticle", "Breadcrumb" },
            new object[] { "/pages/breadcrumb", string.Empty, "Breadcrumb" },
            new object[] { "/pages/{article}/contents", "SomeArticle", "Body" },
            new object[] { "/pages/contents", string.Empty, "Body" },
        };

        [Theory]
        [MemberData(nameof(PagesRouteData))]
        public async void PagesControllerCallsContentPageServiceUsingPagesRoute(string route, string article, string actionMethod)
        {
            // Arrange
            var controller = BuildController(route);

            // Act
            var result = await RunControllerAction(controller, article, actionMethod).ConfigureAwait(false);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            A.CallTo(() => fakeContentPageService.GetByNameAsync(A<string>.Ignored)).MustHaveHappenedOnceExactly();
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

            return new PagesController(fakeContentPageService, fakeMapper)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                },
            };
        }
    }
}