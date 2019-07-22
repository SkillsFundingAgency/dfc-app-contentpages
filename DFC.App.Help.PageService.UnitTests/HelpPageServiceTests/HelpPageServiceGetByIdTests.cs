using System;
using System.Linq.Expressions;
using DFC.App.Help.Data;
using DFC.App.Help.Data.Contracts;
using FakeItEasy;
using Xunit;

namespace DFC.App.Help.PageService.UnitTests.HelpPageServiceTests
{
    public class HelpPageServiceGetByIdTests
    {
        [Fact]
        public void HelpPageServiceGetByIdReturnsSuccess()
        {
            // arrange
            Guid documentId = Guid.NewGuid();
            var repository = A.Fake<IRepository<HelpPageModel>>();
            var expectedResult = A.Fake<HelpPageModel>();

            A.CallTo(() => repository.GetAsync(A<Expression<Func<HelpPageModel, bool>>>.Ignored)).Returns(expectedResult);

            var helpPageService = new HelpPageService(repository, A.Fake<IDraftHelpPageService>());

            // act
            var result = helpPageService.GetByIdAsync(documentId).Result;

            // assert
            A.CallTo(() => repository.GetAsync(A<Expression<Func<HelpPageModel, bool>>>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public void HelpPageServiceGetByIdReturnsNullWhenMissingRepository()
        {
            // arrange
            Guid documentId = Guid.NewGuid();
            var repository = A.Fake<IRepository<HelpPageModel>>();
            HelpPageModel expectedResult = null;

            A.CallTo(() => repository.GetAsync(A<Expression<Func<HelpPageModel, bool>>>.Ignored)).Returns(expectedResult);

            var helpPageService = new HelpPageService(repository, A.Fake<IDraftHelpPageService>());

            // act
            var result = helpPageService.GetByIdAsync(documentId).Result;

            // assert
            A.CallTo(() => repository.GetAsync(A<Expression<Func<HelpPageModel, bool>>>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }
    }
}
