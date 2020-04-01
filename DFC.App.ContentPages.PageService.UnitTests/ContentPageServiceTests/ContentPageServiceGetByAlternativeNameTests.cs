using DFC.App.ContentPages.Data.Contracts;
using DFC.App.ContentPages.Data.Models;
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
            const string category = "help";
            const string alternativeName = "name1";
            var repository = A.Fake<ICosmosRepository<ContentPageModel>>();
            var expectedResult = A.Fake<ContentPageModel>();

            A.CallTo(() => repository.GetAsync(A<Expression<Func<ContentPageModel, bool>>>.Ignored)).Returns(expectedResult);

            var contentPageService = new ContentPageService(repository);

            // act
            var result = contentPageService.GetByAlternativeNameAsync(category, alternativeName).Result;

            // assert
            A.CallTo(() => repository.GetAsync(A<Expression<Func<ContentPageModel, bool>>>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public async System.Threading.Tasks.Task ContentPageServiceGetByAlternativeNameReturnsArgumentNullExceptionWhenNullNameIsUsed()
        {
            // arrange
            const string category = "help";
            var repository = A.Fake<ICosmosRepository<ContentPageModel>>();
            var contentPageService = new ContentPageService(repository);

            // act
            var exceptionResult = await Assert.ThrowsAsync<ArgumentNullException>(async () => await contentPageService.GetByAlternativeNameAsync(category, null).ConfigureAwait(false)).ConfigureAwait(false);

            // assert
            Assert.Equal("Value cannot be null. (Parameter 'alternativeName')", exceptionResult.Message);
        }

        [Fact]
        public async System.Threading.Tasks.Task ContentPageServiceGetByAlternativeNameReturnsArgumentNullExceptionWhenNullCategoryIsUsed()
        {
            // arrange
            const string alternativeName = "name1";
            var repository = A.Fake<ICosmosRepository<ContentPageModel>>();
            var contentPageService = new ContentPageService(repository);

            // act
            var exceptionResult = await Assert.ThrowsAsync<ArgumentNullException>(async () => await contentPageService.GetByAlternativeNameAsync(null, alternativeName).ConfigureAwait(false)).ConfigureAwait(false);

            // assert
            Assert.Equal("Value cannot be null. (Parameter 'category')", exceptionResult.Message);
        }

        [Fact]
        public void ContentPageServiceGetByAlternativeNameReturnsNullWhenMissingRepository()
        {
            // arrange
            const string category = "help";
            const string alternativeName = "name1";
            var repository = A.Dummy<ICosmosRepository<ContentPageModel>>();
            ContentPageModel expectedResult = null;

            A.CallTo(() => repository.GetAsync(A<Expression<Func<ContentPageModel, bool>>>.Ignored)).Returns(expectedResult);

            var contentPageService = new ContentPageService(repository);

            // act
            var result = contentPageService.GetByAlternativeNameAsync(category, alternativeName).Result;

            // assert
            A.CallTo(() => repository.GetAsync(A<Expression<Func<ContentPageModel, bool>>>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }
    }
}
