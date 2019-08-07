using DFC.App.Help.Controllers;
using DFC.App.Help.Data;
using DFC.App.Help.ViewModels;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Xunit;

namespace DFC.App.Help.PagesModule.UnitTests.ControllerTests.PagesControllerTests
{
    [Trait("Category", "Pages Controller Unit Tests")]
    public class PagesControllerBreadcrumbTests : BasePagesController
    {
        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public async void PagesControllerBreadcrumbHtmlReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            const string article = "an-article-name";
            var expectedResult = A.Fake<HelpPageModel>();
            var controller = BuildPagesController(mediaTypeName);

            expectedResult.CanonicalName = article;

            A.CallTo(() => FakeHelpPageService.GetByNameAsync(A<string>.Ignored, A<bool>.Ignored)).Returns(expectedResult);

            // Act
            var result = await controller.Breadcrumb(article).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeHelpPageService.GetByNameAsync(A<string>.Ignored, A<bool>.Ignored)).MustHaveHappenedOnceExactly();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<BreadcrumbViewModel>(viewResult.ViewData.Model);

            model.Paths.Count.Should().BeGreaterThan(0);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async void PagesControllerBreadcrumbJsonReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            const string article = "an-article-name";
            var expectedResult = A.Fake<HelpPageModel>();
            var controller = BuildPagesController(mediaTypeName);

            expectedResult.CanonicalName = article;

            A.CallTo(() => FakeHelpPageService.GetByNameAsync(A<string>.Ignored, A<bool>.Ignored)).Returns(expectedResult);

            // Act
            var result = await controller.Breadcrumb(article).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeHelpPageService.GetByNameAsync(A<string>.Ignored, A<bool>.Ignored)).MustHaveHappenedOnceExactly();

            var jsonResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<BreadcrumbViewModel>(jsonResult.Value);

            model.Paths.Count.Should().BeGreaterThan(0);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public async void PagesControllerBreadcrumbHtmlReturnsSuccessForDefaultArticleName(string mediaTypeName)
        {
            // Arrange
            const string article = PagesController.DefaultArticleName;
            var expectedResult = A.Fake<HelpPageModel>();
            var controller = BuildPagesController(mediaTypeName);

            expectedResult.CanonicalName = article;

            A.CallTo(() => FakeHelpPageService.GetByNameAsync(A<string>.Ignored, A<bool>.Ignored)).Returns(expectedResult);

            // Act
            var result = await controller.Breadcrumb(article).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeHelpPageService.GetByNameAsync(A<string>.Ignored, A<bool>.Ignored)).MustHaveHappenedOnceExactly();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<BreadcrumbViewModel>(viewResult.ViewData.Model);

            model.Paths.Count.Should().BeGreaterThan(0);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async void PagesControllerBreadcrumbJsonReturnsSuccessForDefaultArticleName(string mediaTypeName)
        {
            // Arrange
            const string article = PagesController.DefaultArticleName;
            var expectedResult = A.Fake<HelpPageModel>();
            var controller = BuildPagesController(mediaTypeName);

            expectedResult.CanonicalName = article;

            A.CallTo(() => FakeHelpPageService.GetByNameAsync(A<string>.Ignored, A<bool>.Ignored)).Returns(expectedResult);

            // Act
            var result = await controller.Breadcrumb(article).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeHelpPageService.GetByNameAsync(A<string>.Ignored, A<bool>.Ignored)).MustHaveHappenedOnceExactly();

            var jsonResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<BreadcrumbViewModel>(jsonResult.Value);

            model.Paths.Count.Should().BeGreaterThan(0);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public async void PagesControllerBreadcrumbHtmlReturnsSuccessWhenNoData(string mediaTypeName)
        {
            // Arrange
            const string article = "an-article-name";
            HelpPageModel expectedResult = null;
            var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => FakeHelpPageService.GetByNameAsync(A<string>.Ignored, A<bool>.Ignored)).Returns(expectedResult);

            // Act
            var result = await controller.Breadcrumb(article).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeHelpPageService.GetByNameAsync(A<string>.Ignored, A<bool>.Ignored)).MustHaveHappenedOnceExactly();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<BreadcrumbViewModel>(viewResult.ViewData.Model);

            model.Paths.Count.Should().BeGreaterThan(0);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async void PagesControllerBreadcrumbJsonReturnsSuccessWhenNoData(string mediaTypeName)
        {
            // Arrange
            const string article = "an-article-name";
            HelpPageModel expectedResult = null;
            var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => FakeHelpPageService.GetByNameAsync(A<string>.Ignored, A<bool>.Ignored)).Returns(expectedResult);

            // Act
            var result = await controller.Breadcrumb(article).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeHelpPageService.GetByNameAsync(A<string>.Ignored, A<bool>.Ignored)).MustHaveHappenedOnceExactly();

            var jsonResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<BreadcrumbViewModel>(jsonResult.Value);

            model.Paths.Count.Should().BeGreaterThan(0);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public async void PagesControllerBreadcrumbReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            const string article = "an-article-name";
            var expectedResult = A.Fake<HelpPageModel>();
            var controller = BuildPagesController(mediaTypeName);

            expectedResult.CanonicalName = article;

            A.CallTo(() => FakeHelpPageService.GetByNameAsync(A<string>.Ignored, A<bool>.Ignored)).Returns(expectedResult);

            // Act
            var result = await controller.Breadcrumb(article).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeHelpPageService.GetByNameAsync(A<string>.Ignored, A<bool>.Ignored)).MustHaveHappenedOnceExactly();

            var statusResult = Assert.IsType<StatusCodeResult>(result);

            A.Equals((int)HttpStatusCode.NotAcceptable, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}
