using DFC.App.ContentPages.Data.Models;
using DFC.App.ContentPages.ViewModels;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContentPages.PagesModule.UnitTests.ControllerTests.PagesControllerTests
{
    [Trait("Category", "Pages Controller Unit Tests")]
    public class PagesControllerDocumentTests : BasePagesController
    {
        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public async Task PagesControllerDocumentHtmlReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            const string category = "a-category";
            const string article = "an-article-name";
            var expectedResult = A.Fake<ContentPageModel>();
            var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored, A<string>.Ignored)).Returns(expectedResult);
            A.CallTo(() => FakeMapper.Map<DocumentViewModel>(A<ContentPageModel>.Ignored)).Returns(A.Fake<DocumentViewModel>());

            // Act
            var result = await controller.Document(category, article).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<DocumentViewModel>(A<ContentPageModel>.Ignored)).MustHaveHappenedOnceExactly();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<DocumentViewModel>(viewResult.ViewData.Model);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task PagesControllerDocumentJsonReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            const string category = "a-category";
            const string article = "an-article-name";
            var expectedResult = A.Fake<ContentPageModel>();
            var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored, A<string>.Ignored)).Returns(expectedResult);
            A.CallTo(() => FakeMapper.Map<DocumentViewModel>(A<ContentPageModel>.Ignored)).Returns(A.Fake<DocumentViewModel>());

            // Act
            var result = await controller.Document(category, article).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<DocumentViewModel>(A<ContentPageModel>.Ignored)).MustHaveHappenedOnceExactly();

            var jsonResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<DocumentViewModel>(jsonResult.Value);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public async Task PagesControllerDocumentHtmlReturnsNoContentWhenNoData(string mediaTypeName)
        {
            // Arrange
            const string category = "a-category";
            const string article = "an-article-name";
            ContentPageModel expectedResult = null;
            var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored, A<string>.Ignored)).Returns(expectedResult);

            // Act
            var result = await controller.Document(category, article).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();

            var statusResult = Assert.IsType<NoContentResult>(result);

            A.Equals((int)HttpStatusCode.NoContent, statusResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task PagesControllerDocumentJsonReturnsNoContentWhenNoData(string mediaTypeName)
        {
            // Arrange
            const string category = "a-category";
            const string article = "an-article-name";
            ContentPageModel expectedResult = null;
            var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored, A<string>.Ignored)).Returns(expectedResult);

            // Act
            var result = await controller.Document(category, article).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();

            var statusResult = Assert.IsType<NoContentResult>(result);

            A.Equals((int)HttpStatusCode.NoContent, statusResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public async Task PagesControllerDocumentReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            const string category = "a-category";
            const string article = "an-article-name";
            var expectedResult = A.Fake<ContentPageModel>();
            var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored, A<string>.Ignored)).Returns(expectedResult);
            A.CallTo(() => FakeMapper.Map<DocumentViewModel>(A<ContentPageModel>.Ignored)).Returns(A.Fake<DocumentViewModel>());

            // Act
            var result = await controller.Document(category, article).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<DocumentViewModel>(A<ContentPageModel>.Ignored)).MustHaveHappenedOnceExactly();

            var statusResult = Assert.IsType<StatusCodeResult>(result);

            A.Equals((int)HttpStatusCode.NotAcceptable, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}
