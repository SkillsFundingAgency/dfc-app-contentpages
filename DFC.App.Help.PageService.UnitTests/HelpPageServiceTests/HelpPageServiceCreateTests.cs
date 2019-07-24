using System;
using System.Linq.Expressions;
using System.Net;
using DFC.App.Help.Data;
using DFC.App.Help.Data.Contracts;
using FakeItEasy;
using Xunit;

namespace DFC.App.Help.PageService.UnitTests.HelpPageServiceTests
{
    public class HelpPageServiceCreateTests
    {
        [Fact]
        public void HelpPageServiceCreateReturnsSuccessWhenHelpPageCreated()
        {
            // arrange
            var repository = A.Fake<IRepository<HelpPageModel>>();
            var helpPageModel = A.Fake<HelpPageModel>();
            var expectedResult = A.Fake<HelpPageModel>();

            A.CallTo(() => repository.CreateAsync(helpPageModel)).Returns(HttpStatusCode.Created);
            A.CallTo(() => repository.GetAsync(A<Expression<Func<HelpPageModel, bool>>>.Ignored)).Returns(expectedResult);

            var helpPageService = new HelpPageService(repository, A.Fake<IDraftHelpPageService>());

            // act
            var result = helpPageService.CreateAsync(helpPageModel).Result;

            // assert
            A.CallTo(() => repository.CreateAsync(helpPageModel)).MustHaveHappenedOnceExactly();
            A.CallTo(() => repository.GetAsync(A<Expression<Func<HelpPageModel, bool>>>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public void HelpPageServiceCreateReturnsNullWhenHelpPageNotCreated()
        {
            // arrange
            var repository = A.Fake<IRepository<HelpPageModel>>();
            var helpPageModel = A.Fake<HelpPageModel>();
            var expectedResult = A.Dummy<HelpPageModel>();

            A.CallTo(() => repository.CreateAsync(helpPageModel)).Returns(HttpStatusCode.BadRequest);

            var helpPageService = new HelpPageService(repository, A.Fake<IDraftHelpPageService>());

            // act
            var result = helpPageService.CreateAsync(helpPageModel).Result;

            // assert
            A.CallTo(() => repository.CreateAsync(helpPageModel)).MustHaveHappenedOnceExactly();
            A.CallTo(() => repository.GetAsync(A<Expression<Func<HelpPageModel, bool>>>.Ignored)).MustNotHaveHappened();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public void HelpPageServiceCreateReturnsNullWhenMissingRepository()
        {
            // arrange
            var repository = A.Dummy<IRepository<HelpPageModel>>();
            var helpPageModel = A.Fake<HelpPageModel>();
            HelpPageModel expectedResult = null;

            A.CallTo(() => repository.CreateAsync(helpPageModel)).Returns(HttpStatusCode.FailedDependency);

            var helpPageService = new HelpPageService(repository, A.Fake<IDraftHelpPageService>());

            // act
            var result = helpPageService.CreateAsync(helpPageModel).Result;

            // assert
            A.CallTo(() => repository.CreateAsync(helpPageModel)).MustHaveHappenedOnceExactly();
            A.CallTo(() => repository.GetAsync(A<Expression<Func<HelpPageModel, bool>>>.Ignored)).MustNotHaveHappened();
            A.Equals(result, expectedResult);
        }
    }
}
