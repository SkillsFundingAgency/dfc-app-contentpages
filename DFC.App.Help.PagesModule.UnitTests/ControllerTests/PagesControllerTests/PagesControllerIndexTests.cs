using DFC.App.Help.Controllers;
using DFC.App.Help.Data;
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
    public class PagesControllerIndexTests : BasePagesController
    {
        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public async void PagesControllerIndexHtmlReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            const int resultsCount = 2;
            var expectedResults = A.CollectionOfFake<HelpPageModel>(resultsCount);
            var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => fakeHelpPageService.GetAllAsync()).Returns(expectedResults);
            A.CallTo(() => fakeMapper.Map<IndexDocumentViewModel>(A<HelpPageModel>.Ignored)).Returns(A.Fake<IndexDocumentViewModel>());

            // Act
            var result = await controller.Index().ConfigureAwait(false);

            // Assert
            A.CallTo(() => fakeHelpPageService.GetAllAsync()).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeMapper.Map<IndexDocumentViewModel>(A<HelpPageModel>.Ignored)).MustHaveHappened(resultsCount, Times.Exactly);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IndexViewModel>(viewResult.ViewData.Model);

            A.Equals(resultsCount, model.Documents.Count());

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async void PagesControllerIndexJsonReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            const int resultsCount = 2;
            var expectedResults = A.CollectionOfFake<HelpPageModel>(resultsCount);
            var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => fakeHelpPageService.GetAllAsync()).Returns(expectedResults);
            A.CallTo(() => fakeMapper.Map<IndexDocumentViewModel>(A<HelpPageModel>.Ignored)).Returns(A.Fake<IndexDocumentViewModel>());

            // Act
            var result = await controller.Index().ConfigureAwait(false);

            // Assert
            A.CallTo(() => fakeHelpPageService.GetAllAsync()).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeMapper.Map<IndexDocumentViewModel>(A<HelpPageModel>.Ignored)).MustHaveHappened(resultsCount, Times.Exactly);

            var jsonResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IndexViewModel>(jsonResult.Value);

            A.Equals(resultsCount, model.Documents.Count());

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public async void PagesControllerIndexHtmlReturnsSuccessWhenNoData(string mediaTypeName)
        {
            // Arrange
            const int resultsCount = 0;
            IEnumerable<HelpPageModel> expectedResults = null;
            var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => fakeHelpPageService.GetAllAsync()).Returns(expectedResults);
            A.CallTo(() => fakeMapper.Map<IndexDocumentViewModel>(A<HelpPageModel>.Ignored)).Returns(A.Fake<IndexDocumentViewModel>());

            // Act
            var result = await controller.Index().ConfigureAwait(false);

            // Assert
            A.CallTo(() => fakeHelpPageService.GetAllAsync()).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeMapper.Map<IndexDocumentViewModel>(A<HelpPageModel>.Ignored)).MustHaveHappened(resultsCount, Times.Exactly);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IndexViewModel>(viewResult.ViewData.Model);

            A.Equals(null, model.Documents);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async void PagesControllerIndexJsonReturnsSuccessWhenNoData(string mediaTypeName)
        {
            // Arrange
            const int resultsCount = 0;
            IEnumerable<HelpPageModel> expectedResults = null;
            var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => fakeHelpPageService.GetAllAsync()).Returns(expectedResults);
            A.CallTo(() => fakeMapper.Map<IndexDocumentViewModel>(A<HelpPageModel>.Ignored)).Returns(A.Fake<IndexDocumentViewModel>());

            // Act
            var result = await controller.Index().ConfigureAwait(false);

            // Assert
            A.CallTo(() => fakeHelpPageService.GetAllAsync()).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeMapper.Map<IndexDocumentViewModel>(A<HelpPageModel>.Ignored)).MustHaveHappened(resultsCount, Times.Exactly);

            var jsonResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IndexViewModel>(jsonResult.Value);

            A.Equals(null, model.Documents);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public async void PagesControllerIndexReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            const int resultsCount = 0;
            IEnumerable<HelpPageModel> expectedResults = null;
            var controller = BuildPagesController(mediaTypeName);

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
    }
}
