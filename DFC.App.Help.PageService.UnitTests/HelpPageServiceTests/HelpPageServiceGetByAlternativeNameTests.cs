using DFC.App.Help.Data;
using DFC.App.Help.Data.Contracts;
using FakeItEasy;
using System;
using System.Linq.Expressions;
using Xunit;

namespace DFC.App.Help.PageService.UnitTests.HelpPageServiceTests
{
    [Trait("Category", "Page Service Unit Tests")]
    public class HelpPageServiceGetByAlternativeNameTests
    {
        [Fact]
        public void HelpPageServiceGetByAlternativeNameReturnsSuccess()
        {
            // arrange
            const string alternativeName = "name1";
            var repository = A.Fake<ICosmosRepository<HelpPageModel>>();
            var expectedResult = A.Fake<HelpPageModel>();

            A.CallTo(() => repository.GetAsync(A<Expression<Func<HelpPageModel, bool>>>.Ignored)).Returns(expectedResult);

            var helpPageService = new HelpPageService(repository, A.Fake<IDraftHelpPageService>());

            // act
            var result = helpPageService.GetByAlternativeNameAsync(alternativeName).Result;

            // assert
            A.CallTo(() => repository.GetAsync(A<Expression<Func<HelpPageModel, bool>>>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public async System.Threading.Tasks.Task HelpPageServiceGetByAlternativeNameReturnsArgumentNullExceptionWhenNullIsUsed()
        {
            // arrange
            var repository = A.Fake<ICosmosRepository<HelpPageModel>>();
            var helpPageService = new HelpPageService(repository, A.Fake<IDraftHelpPageService>());

            // act
            var exceptionResult = await Assert.ThrowsAsync<ArgumentNullException>(async () => await helpPageService.GetByAlternativeNameAsync(null).ConfigureAwait(false)).ConfigureAwait(false);

            // assert
            Assert.Equal("Value cannot be null.\r\nParameter name: alternativeName", exceptionResult.Message);
        }

        [Fact]
        public void HelpPageServiceGetByAlternativeNameReturnsNullWhenMissingRepository()
        {
            // arrange
            const string alternativeName = "name1";
            var repository = A.Dummy<ICosmosRepository<HelpPageModel>>();
            HelpPageModel expectedResult = null;

            A.CallTo(() => repository.GetAsync(A<Expression<Func<HelpPageModel, bool>>>.Ignored)).Returns(expectedResult);

            var helpPageService = new HelpPageService(repository, A.Fake<IDraftHelpPageService>());

            // act
            var result = helpPageService.GetByAlternativeNameAsync(alternativeName).Result;

            // assert
            A.CallTo(() => repository.GetAsync(A<Expression<Func<HelpPageModel, bool>>>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }
    }
}
