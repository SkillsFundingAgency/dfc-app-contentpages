using System;
using System.Linq.Expressions;
using DFC.App.Help.Data;
using DFC.App.Help.Data.Contracts;
using FakeItEasy;
using Xunit;

namespace DFC.App.Help.PageService.UnitTests.HelpPageServiceTests
{
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
