using DFC.App.ContentPages.Data.Contracts;
using DFC.App.ContentPages.Data.Models;
using FakeItEasy;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContentPages.PageService.UnitTests.ContentPageServiceTests
{
    [Trait("Category", "Page Service Unit Tests")]
    public class ContentPageServiceGetByNameTests
    {
        private const string Category = "category1";
        private const string CanonicalName = "name1";
        private readonly ICosmosRepository<ContentPageModel> repository;
        private readonly IContentPageService contentPageService;

        public ContentPageServiceGetByNameTests()
        {
            repository = A.Fake<ICosmosRepository<ContentPageModel>>();
            contentPageService = new ContentPageService(repository);
        }

        [Fact]
        public async Task ContentPageServiceGetByNameReturnsSuccess()
        {
            // arrange
            var expectedResult = A.Fake<ContentPageModel>();
            A.CallTo(() => repository.GetAsync(A<string>.Ignored, A<Expression<Func<ContentPageModel, bool>>>.Ignored)).Returns(expectedResult);

            // act
            var result = await contentPageService.GetByNameAsync(Category, CanonicalName).ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.GetAsync(A<string>.Ignored, A<Expression<Func<ContentPageModel, bool>>>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Equal(result, expectedResult);
        }

        [Fact]
        public async Task ContentPageServiceGetByNameReturnsArgumentNullExceptionWhenNullNameIsUsed()
        {
            // arrange

            // act
            var exceptionResult = await Assert.ThrowsAsync<ArgumentNullException>(async () => await contentPageService.GetByNameAsync(Category, null).ConfigureAwait(false)).ConfigureAwait(false);

            // assert
            Assert.Equal("Value cannot be null.\r\nParameter name: canonicalName", exceptionResult.Message);
        }

        [Fact]
        public async Task ContentPageServiceGetByNameReturnsArgumentNullExceptionWhenNullCategoryIsUsed()
        {
            // arrange

            // act
            var exceptionResult = await Assert.ThrowsAsync<ArgumentNullException>(async () => await contentPageService.GetByNameAsync(null, CanonicalName).ConfigureAwait(false)).ConfigureAwait(false);

            // assert
            Assert.Equal("Value cannot be null.\r\nParameter name: category", exceptionResult.Message);
        }

        [Fact]
        public async Task ContentPageServiceGetByNameReturnsNullWhenMissingRepository()
        {
            // arrange
            A.CallTo(() => repository.GetAsync(A<string>.Ignored, A<Expression<Func<ContentPageModel, bool>>>.Ignored)).Returns((ContentPageModel)null);

            // act
            var result = await contentPageService.GetByNameAsync(Category, CanonicalName).ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.GetAsync(A<string>.Ignored, A<Expression<Func<ContentPageModel, bool>>>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Null(result);
        }
    }
}