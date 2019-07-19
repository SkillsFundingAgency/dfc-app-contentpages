using DFC.App.Help.Controllers;
using DFC.App.Help.Data;
using DFC.App.Help.Data.Contracts;
using DFC.App.Help.ViewModels;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using Xunit;

namespace DFC.App.Help.PagesModule.UnitTests.ControllerTests.PagesControllerTests
{
    public class PagesIndexTests
    {
        [Theory]
        [InlineData("*/*")]
        [InlineData(MediaTypeNames.Text.Html)]
        [InlineData(MediaTypeNames.Application.Json)]
        public async void HelpPageServiceGetAllListReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            const int resultsCount = 2;
            var expectedResults = A.CollectionOfFake<HelpPageModel>(resultsCount);
            var fakeHelpPageService = A.Fake<IHelpPageService>();
            var fakeMapper = A.Fake<AutoMapper.IMapper>();
            var controller = new PagesController(fakeHelpPageService, fakeMapper)
            {
                ControllerContext = BuildContext(mediaTypeName),
            };

            A.CallTo(() => fakeHelpPageService.GetAllAsync()).Returns(expectedResults);
            A.CallTo(() => fakeMapper.Map<IndexDocumentViewModel>(A<HelpPageModel>.Ignored)).Returns(A.Fake<IndexDocumentViewModel>());

            // Act
            var result = await controller.Index().ConfigureAwait(false);

            // Assert
            A.CallTo(() => fakeHelpPageService.GetAllAsync()).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeMapper.Map<IndexDocumentViewModel>(A<HelpPageModel>.Ignored)).MustHaveHappened(resultsCount, Times.Exactly);

            IndexViewModel model = null;

            switch (mediaTypeName)
            {
                case MediaTypeNames.Application.Json:
                    var jsonResult = Assert.IsType<OkObjectResult>(result);
                    model = Assert.IsAssignableFrom<IndexViewModel>(jsonResult.Value);
                    break;
                default:
                    var viewResult = Assert.IsType<ViewResult>(result);
                    model = Assert.IsAssignableFrom<IndexViewModel>(viewResult.ViewData.Model);
                    break;
            }

            A.Equals(resultsCount, model.Documents.Count());

            controller.Dispose();
        }

        [Theory]
        [InlineData("*/*")]
        [InlineData(MediaTypeNames.Text.Html)]
        [InlineData(MediaTypeNames.Application.Json)]
        public async void HelpPageServiceGetAllListReturnsSuccessWhenNoData(string mediaTypeName)
        {
            // Arrange
            const int resultsCount = 0;
            IEnumerable<HelpPageModel> expectedResults = null;
            var fakeHelpPageService = A.Fake<IHelpPageService>();
            var fakeMapper = A.Fake<AutoMapper.IMapper>();
            var controller = new PagesController(fakeHelpPageService, fakeMapper)
            {
                ControllerContext = BuildContext(mediaTypeName),
            };

            A.CallTo(() => fakeHelpPageService.GetAllAsync()).Returns(expectedResults);
            A.CallTo(() => fakeMapper.Map<IndexDocumentViewModel>(A<HelpPageModel>.Ignored)).Returns(A.Fake<IndexDocumentViewModel>());

            // Act
            var result = await controller.Index().ConfigureAwait(false);

            // Assert
            A.CallTo(() => fakeHelpPageService.GetAllAsync()).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeMapper.Map<IndexDocumentViewModel>(A<HelpPageModel>.Ignored)).MustHaveHappened(resultsCount, Times.Exactly);

            IndexViewModel model = null;

            switch (mediaTypeName)
            {
                case MediaTypeNames.Application.Json:
                    var jsonResult = Assert.IsType<OkObjectResult>(result);
                    model = Assert.IsAssignableFrom<IndexViewModel>(jsonResult.Value);
                    break;
                default:
                    var viewResult = Assert.IsType<ViewResult>(result);
                    model = Assert.IsAssignableFrom<IndexViewModel>(viewResult.ViewData.Model);
                    break;
            }

            A.Equals(null, model.Documents);

            controller.Dispose();
        }

        [Theory]
        [InlineData(MediaTypeNames.Text.Plain)]
        public async void HelpPageServiceGetAllListReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            const int resultsCount = 0;
            IEnumerable<HelpPageModel> expectedResults = null;
            var fakeHelpPageService = A.Fake<IHelpPageService>();
            var fakeMapper = A.Fake<AutoMapper.IMapper>();
            var controller = new PagesController(fakeHelpPageService, fakeMapper)
            {
                ControllerContext = BuildContext(mediaTypeName),
            };

            A.CallTo(() => fakeHelpPageService.GetAllAsync()).Returns(expectedResults);
            A.CallTo(() => fakeMapper.Map<IndexDocumentViewModel>(A<HelpPageModel>.Ignored)).Returns(A.Fake<IndexDocumentViewModel>());

            // Act
            var result = await controller.Index().ConfigureAwait(false);

            // Assert
            A.CallTo(() => fakeHelpPageService.GetAllAsync()).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeMapper.Map<IndexDocumentViewModel>(A<HelpPageModel>.Ignored)).MustHaveHappened(resultsCount, Times.Exactly);

            var statusResult = Assert.IsType<StatusCodeResult>(result);

            A.Equals((int)HttpStatusCode.NotAcceptable, statusResult.StatusCode);

            controller.Dispose();
        }

        private ControllerContext BuildContext(string mediaTypeName)
        {
            var httpContext = new DefaultHttpContext();

            httpContext.Request.Headers[HeaderNames.Accept] = mediaTypeName;

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            return controllerContext;
        }
    }
}
