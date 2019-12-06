using DFC.App.ContentPages.Data;
using DFC.App.ContentPages.Data.Contracts;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContentPages.PageService.UnitTests.ContentPageServiceTests
{
    [Trait("Category", "Page Service Unit Tests")]
    public class ContentPageServiceGetAllCategoryTests
    {
        [Fact]
        public async Task ContentPageServiceGetAllListReturnsSuccess()
        {
            // arrange
            const string category = "help";
            var repository = A.Fake<ICosmosRepository<ContentPageModel>>();
            var expectedResults = A.CollectionOfFake<ContentPageModel>(2);

            A.CallTo(() => repository.GetAllAsync(A<Expression<Func<ContentPageModel, bool>>>.Ignored)).Returns(expectedResults);

            var contentPageService = new ContentPageService(repository);

            // act
            var results = await contentPageService.GetAllAsync(category).ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.GetAllAsync(A<Expression<Func<ContentPageModel, bool>>>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(results, expectedResults);
        }

        [Fact]
        public async Task ContentPageServiceGetAllListReturnsNullWhenMissingRepository()
        {
            // arrange
            const string category = "help";
            var repository = A.Dummy<ICosmosRepository<ContentPageModel>>();
            IEnumerable<ContentPageModel> expectedResults = null;

            A.CallTo(() => repository.GetAllAsync(A<Expression<Func<ContentPageModel, bool>>>.Ignored)).Returns(expectedResults);

            var contentPageService = new ContentPageService(repository);

            // act
            var results = await contentPageService.GetAllAsync(category).ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.GetAllAsync(A<Expression<Func<ContentPageModel, bool>>>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(results, expectedResults);
        }

        [Fact]
        public async Task ContentPageServiceGetAllListReturnsNullWhenMissingCategory()
        {
            // arrange
            var repository = A.Dummy<ICosmosRepository<ContentPageModel>>();

            var contentPageService = new ContentPageService(repository);

            // act
            var exceptionResult = await Assert.ThrowsAsync<ArgumentNullException>(async () => await contentPageService.GetAllAsync(null).ConfigureAwait(false)).ConfigureAwait(false);

            // assert
            Assert.Equal("Value cannot be null.\r\nParameter name: category", exceptionResult.Message);
        }
    }
}
