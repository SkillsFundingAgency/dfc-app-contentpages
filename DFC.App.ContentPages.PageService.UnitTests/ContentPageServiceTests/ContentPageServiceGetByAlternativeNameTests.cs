using DFC.App.ContentPages.Data;
using DFC.App.ContentPages.Data.Contracts;
using FakeItEasy;
using System;
using System.Linq.Expressions;
using Xunit;

namespace DFC.App.ContentPages.PageService.UnitTests.ContentPageServiceTests
{
    [Trait("Category", "Page Service Unit Tests")]
    public class ContentPageServiceGetByAlternativeNameTests
    {
        [Fact]
        public void ContentPageServiceGetByAlternativeNameReturnsSuccess()
        {
            // arrange
            const string alternativeName = "name1";
            var repository = A.Fake<ICosmosRepository<ContentPageModel>>();
            var expectedResult = A.Fake<ContentPageModel>();

            A.CallTo(() => repository.GetAsync(A<Expression<Func<ContentPageModel, bool>>>.Ignored)).Returns(expectedResult);

            var contentPageService = new ContentPageService(repository);

            // act
            var result = contentPageService.GetByAlternativeNameAsync(alternativeName).Result;

            // assert
            A.CallTo(() => repository.GetAsync(A<Expression<Func<ContentPageModel, bool>>>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public async System.Threading.Tasks.Task ContentPageServiceGetByAlternativeNameReturnsArgumentNullExceptionWhenNullIsUsed()
        {
            // arrange
            var repository = A.Fake<ICosmosRepository<ContentPageModel>>();
            var contentPageService = new ContentPageService(repository);

            // act
            var exceptionResult = await Assert.ThrowsAsync<ArgumentNullException>(async () => await contentPageService.GetByAlternativeNameAsync(null).ConfigureAwait(false)).ConfigureAwait(false);

            // assert
            Assert.Equal("Value cannot be null.\r\nParameter name: alternativeName", exceptionResult.Message);
        }

        [Fact]
        public void ContentPageServiceGetByAlternativeNameReturnsNullWhenMissingRepository()
        {
            // arrange
            const string alternativeName = "name1";
            var repository = A.Dummy<ICosmosRepository<ContentPageModel>>();
            ContentPageModel expectedResult = null;

            A.CallTo(() => repository.GetAsync(A<Expression<Func<ContentPageModel, bool>>>.Ignored)).Returns(expectedResult);

            var contentPageService = new ContentPageService(repository);

            // act
            var result = contentPageService.GetByAlternativeNameAsync(alternativeName).Result;

            // assert
            A.CallTo(() => repository.GetAsync(A<Expression<Func<ContentPageModel, bool>>>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }
    }
}
