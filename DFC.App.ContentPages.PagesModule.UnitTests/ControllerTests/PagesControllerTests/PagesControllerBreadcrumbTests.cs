using DFC.App.ContentPages.Controllers;
using DFC.App.ContentPages.Data;
using DFC.App.ContentPages.ViewModels;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContentPages.PagesModule.UnitTests.ControllerTests.PagesControllerTests
{
    [Trait("Category", "Pages Controller Unit Tests")]
    public class PagesControllerBreadcrumbTests : BasePagesController
    {
        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public async Task PagesControllerBreadcrumbHtmlReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            const string category = "a-category";
            const string article = "an-article-name";
            var expectedResult = A.Fake<ContentPageModel>();
            var controller = BuildPagesController(mediaTypeName);

            expectedResult.CanonicalName = article;

            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored, A<string>.Ignored)).Returns(expectedResult);

            // Act
            var result = await controller.Breadcrumb(category, article).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<BreadcrumbViewModel>(viewResult.ViewData.Model);

            model.Paths.Count.Should().BeGreaterThan(0);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task PagesControllerBreadcrumbJsonReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            const string category = "a-category";
            const string article = "an-article-name";
            var expectedResult = A.Fake<ContentPageModel>();
            var controller = BuildPagesController(mediaTypeName);

            expectedResult.CanonicalName = article;

            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored, A<string>.Ignored)).Returns(expectedResult);

            // Act
            var result = await controller.Breadcrumb(category, article).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();

            var jsonResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<BreadcrumbViewModel>(jsonResult.Value);

            model.Paths.Count.Should().BeGreaterThan(0);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public async Task PagesControllerBreadcrumbHtmlReturnsSuccessForDefaultArticleName(string mediaTypeName)
        {
            // Arrange
            const string category = "a-category";
            const string article = DefaultHelpArticleName;
            var expectedResult = A.Fake<ContentPageModel>();
            var controller = BuildPagesController(mediaTypeName);

            expectedResult.CanonicalName = article;

            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored, A<string>.Ignored)).Returns(expectedResult);

            // Act
            var result = await controller.Breadcrumb(category, article).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<BreadcrumbViewModel>(viewResult.ViewData.Model);

            model.Paths.Count.Should().BeGreaterThan(0);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public async Task PagesControllerBreadcrumbWithNullArticleHtmlReturnsSuccessForDefaultArticleName(string mediaTypeName)
        {
            // Arrange
            const string category = "a-category";
            const string article = null;
            var expectedResult = A.Fake<ContentPageModel>();
            var controller = BuildPagesController(mediaTypeName);

            expectedResult.CanonicalName = article;

            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored, A<string>.Ignored)).Returns(expectedResult);

            // Act
            var result = await controller.Breadcrumb(category, article).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<BreadcrumbViewModel>(viewResult.ViewData.Model);

            model.Paths.Count.Should().BeGreaterThan(0);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task PagesControllerBreadcrumbJsonReturnsSuccessForDefaultArticleName(string mediaTypeName)
        {
            // Arrange
            const string category = "a-category";
            const string article = DefaultHelpArticleName;
            var expectedResult = A.Fake<ContentPageModel>();
            var controller = BuildPagesController(mediaTypeName);

            expectedResult.CanonicalName = article;

            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored, A<string>.Ignored)).Returns(expectedResult);

            // Act
            var result = await controller.Breadcrumb(category, article).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();

            var jsonResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<BreadcrumbViewModel>(jsonResult.Value);

            model.Paths.Count.Should().BeGreaterThan(0);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task PagesControllerBreadcrumbWithNullArticleJsonReturnsSuccessForDefaultArticleName(string mediaTypeName)
        {
            // Arrange
            const string category = "a-category";
            const string article = null;
            var expectedResult = A.Fake<ContentPageModel>();
            var controller = BuildPagesController(mediaTypeName);

            expectedResult.CanonicalName = article;

            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored, A<string>.Ignored)).Returns(expectedResult);

            // Act
            var result = await controller.Breadcrumb(category, article).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();

            var jsonResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<BreadcrumbViewModel>(jsonResult.Value);

            model.Paths.Count.Should().BeGreaterThan(0);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public async Task PagesControllerBreadcrumbHtmlReturnsSuccessWhenNoData(string mediaTypeName)
        {
            // Arrange
            const string category = "a-category";
            const string article = "an-article-name";
            ContentPageModel expectedResult = null;
            var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored, A<string>.Ignored)).Returns(expectedResult);

            // Act
            var result = await controller.Breadcrumb(category, article).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<BreadcrumbViewModel>(viewResult.ViewData.Model);

            model.Paths.Count.Should().BeGreaterThan(0);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task PagesControllerBreadcrumbJsonReturnsSuccessWhenNoData(string mediaTypeName)
        {
            // Arrange
            const string category = "a-category";
            const string article = "an-article-name";
            ContentPageModel expectedResult = null;
            var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored, A<string>.Ignored)).Returns(expectedResult);

            // Act
            var result = await controller.Breadcrumb(category, article).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();

            var jsonResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<BreadcrumbViewModel>(jsonResult.Value);

            model.Paths.Count.Should().BeGreaterThan(0);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public async Task PagesControllerBreadcrumbReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            const string category = "a-category";
            const string article = "an-article-name";
            var expectedResult = A.Fake<ContentPageModel>();
            var controller = BuildPagesController(mediaTypeName);

            expectedResult.CanonicalName = article;

            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored, A<string>.Ignored)).Returns(expectedResult);

            // Act
            var result = await controller.Breadcrumb(category, article).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();

            var statusResult = Assert.IsType<StatusCodeResult>(result);

            A.Equals((int)HttpStatusCode.NotAcceptable, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}
