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
    public class PagesControllerHeadTests : BasePagesController
    {
        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public async Task PagesControllerHeadHtmlReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            const string category = "a-category";
            const string article = "an-article-name";
            var expectedResult = A.Fake<ContentPageModel>();
            var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored, A<string>.Ignored)).Returns(expectedResult);
            A.CallTo(() => FakeMapper.Map(A<ContentPageModel>.Ignored, A<HeadViewModel>.Ignored)).Returns(A.Fake<HeadViewModel>());

            // Act
            var result = await controller.Head(category, article).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map(A<ContentPageModel>.Ignored, A<HeadViewModel>.Ignored)).MustHaveHappenedOnceExactly();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<HeadViewModel>(viewResult.ViewData.Model);

            model.CanonicalUrl.Should().NotBeNullOrWhiteSpace();

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task PagesControllerHeadJsonReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            const string category = "a-category";
            const string article = "an-article-name";
            var expectedResult = A.Fake<ContentPageModel>();
            var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored, A<string>.Ignored)).Returns(expectedResult);
            A.CallTo(() => FakeMapper.Map(A<ContentPageModel>.Ignored, A<HeadViewModel>.Ignored)).Returns(A.Fake<HeadViewModel>());

            // Act
            var result = await controller.Head(category, article).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map(A<ContentPageModel>.Ignored, A<HeadViewModel>.Ignored)).MustHaveHappenedOnceExactly();

            var jsonResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<HeadViewModel>(jsonResult.Value);

            model.CanonicalUrl.Should().NotBeNullOrWhiteSpace();

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public async Task PagesControllerHeadWithNullArticleHtmlReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            const string category = "a-category";
            const string article = null;
            var expectedResult = A.Fake<ContentPageModel>();
            var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored, A<string>.Ignored)).Returns(expectedResult);
            A.CallTo(() => FakeMapper.Map(A<ContentPageModel>.Ignored, A<HeadViewModel>.Ignored)).Returns(A.Fake<HeadViewModel>());

            // Act
            var result = await controller.Head(category, article).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map(A<ContentPageModel>.Ignored, A<HeadViewModel>.Ignored)).MustHaveHappenedOnceExactly();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<HeadViewModel>(viewResult.ViewData.Model);

            model.CanonicalUrl.Should().NotBeNullOrWhiteSpace();

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task PagesControllerHeadWithNullArticleJsonReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            const string category = "a-category";
            const string article = null;
            var expectedResult = A.Fake<ContentPageModel>();
            var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored, A<string>.Ignored)).Returns(expectedResult);
            A.CallTo(() => FakeMapper.Map(A<ContentPageModel>.Ignored, A<HeadViewModel>.Ignored)).Returns(A.Fake<HeadViewModel>());

            // Act
            var result = await controller.Head(category, article).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map(A<ContentPageModel>.Ignored, A<HeadViewModel>.Ignored)).MustHaveHappenedOnceExactly();

            var jsonResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<HeadViewModel>(jsonResult.Value);

            model.CanonicalUrl.Should().NotBeNullOrWhiteSpace();

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public async Task PagesControllerHeadHtmlReturnsSuccessWhenNoData(string mediaTypeName)
        {
            // Arrange
            const string category = "a-category";
            const string article = "an-article-name";
            ContentPageModel expectedResult = null;
            var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored, A<string>.Ignored)).Returns(expectedResult);

            // Act
            var result = await controller.Head(category, article).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<HeadViewModel>(viewResult.ViewData.Model);

            model.CanonicalUrl.Should().BeNull();

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task PagesControllerHeadJsonReturnsSuccessWhenNoData(string mediaTypeName)
        {
            // Arrange
            const string category = "a-category";
            const string article = "an-article-name";
            ContentPageModel expectedResult = null;
            var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored, A<string>.Ignored)).Returns(expectedResult);

            // Act
            var result = await controller.Head(category, article).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();

            var jsonResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<HeadViewModel>(jsonResult.Value);

            model.CanonicalUrl.Should().BeNull();

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public async Task PagesControllerHeadReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            const string category = "a-category";
            const string article = "an-article-name";
            var expectedResult = A.Fake<ContentPageModel>();
            var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored, A<string>.Ignored)).Returns(expectedResult);
            A.CallTo(() => FakeMapper.Map(A<ContentPageModel>.Ignored, A<HeadViewModel>.Ignored)).Returns(A.Fake<HeadViewModel>());

            // Act
            var result = await controller.Head(category, article).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map(A<ContentPageModel>.Ignored, A<HeadViewModel>.Ignored)).MustHaveHappenedOnceExactly();

            var statusResult = Assert.IsType<StatusCodeResult>(result);

            A.Equals((int)HttpStatusCode.NotAcceptable, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}
