using DFC.App.Help.Data;
using DFC.App.Help.ViewModels;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Xunit;

namespace DFC.App.Help.PagesModule.UnitTests.ControllerTests.PagesControllerTests
{
    public class PagesControllerBodyTopTests : BasePagesController
    {
        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public async void PagesControllerBodyTopHtmlReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            const string article = "an-article-name";
            HelpPageModel expectedResult = null;
            var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => fakeHelpPageService.GetByNameAsync(A<string>.Ignored, A<bool>.Ignored)).Returns(expectedResult);

            // Act
            var result = await controller.Document(article).ConfigureAwait(false);

            // Assert
            A.CallTo(() => fakeHelpPageService.GetByNameAsync(A<string>.Ignored, A<bool>.Ignored)).MustHaveHappenedOnceExactly();

            var statusResult = Assert.IsType<NoContentResult>(result);

            A.Equals((int)HttpStatusCode.NoContent, statusResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async void PagesControllerBodyTopJsonReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            const string article = "an-article-name";
            HelpPageModel expectedResult = null;
            var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => fakeHelpPageService.GetByNameAsync(A<string>.Ignored, A<bool>.Ignored)).Returns(expectedResult);

            // Act
            var result = await controller.Document(article).ConfigureAwait(false);

            // Assert
            A.CallTo(() => fakeHelpPageService.GetByNameAsync(A<string>.Ignored, A<bool>.Ignored)).MustHaveHappenedOnceExactly();

            var statusResult = Assert.IsType<NoContentResult>(result);

            A.Equals((int)HttpStatusCode.NoContent, statusResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public async void PagesControllerBodyTopHtmlReturnsSuccessWhenNoData(string mediaTypeName)
        {
            // Arrange
            const string article = "an-article-name";
            HelpPageModel expectedResult = null;
            var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => fakeHelpPageService.GetByNameAsync(A<string>.Ignored, A<bool>.Ignored)).Returns(expectedResult);

            // Act
            var result = await controller.Document(article).ConfigureAwait(false);

            // Assert
            A.CallTo(() => fakeHelpPageService.GetByNameAsync(A<string>.Ignored, A<bool>.Ignored)).MustHaveHappenedOnceExactly();

            var statusResult = Assert.IsType<NoContentResult>(result);

            A.Equals((int)HttpStatusCode.NoContent, statusResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async void PagesControllerBodyTopJsonReturnsSuccessWhenNoData(string mediaTypeName)
        {
            // Arrange
            const string article = "an-article-name";
            HelpPageModel expectedResult = null;
            var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => fakeHelpPageService.GetByNameAsync(A<string>.Ignored, A<bool>.Ignored)).Returns(expectedResult);

            // Act
            var result = await controller.Document(article).ConfigureAwait(false);

            // Assert
            A.CallTo(() => fakeHelpPageService.GetByNameAsync(A<string>.Ignored, A<bool>.Ignored)).MustHaveHappenedOnceExactly();

            var statusResult = Assert.IsType<NoContentResult>(result);

            A.Equals((int)HttpStatusCode.NoContent, statusResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public async void PagesControllerBodyTopReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            const string article = "an-article-name";
            HelpPageModel expectedResult = null;
            var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => fakeHelpPageService.GetByNameAsync(A<string>.Ignored, A<bool>.Ignored)).Returns(expectedResult);

            // Act
            var result = await controller.Document(article).ConfigureAwait(false);

            // Assert
            A.CallTo(() => fakeHelpPageService.GetByNameAsync(A<string>.Ignored, A<bool>.Ignored)).MustHaveHappenedOnceExactly();

            var statusResult = Assert.IsType<NoContentResult>(result);

            A.Equals((int)HttpStatusCode.NoContent, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}
