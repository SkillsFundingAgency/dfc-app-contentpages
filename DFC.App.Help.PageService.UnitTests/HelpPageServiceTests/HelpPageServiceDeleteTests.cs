using DFC.App.Help.Data;
using DFC.App.Help.Data.Contracts;
using FakeItEasy;
using System;
using System.Net;
using Xunit;

namespace DFC.App.Help.PageService.UnitTests.HelpPageServiceTests
{
    public class HelpPageServiceDeleteTests
    {
        [Fact]
        public void HelpPageServiceDeleteReturnsSuccessWhenHelpPageDeleted()
        {
            // arrange
            Guid documentId = Guid.NewGuid();
            var repository = A.Fake<ICosmosRepository<HelpPageModel>>();
            var expectedResult = true;

            A.CallTo(() => repository.DeleteAsync(documentId)).Returns(HttpStatusCode.NoContent);

            var helpPageService = new HelpPageService(repository, A.Fake<IDraftHelpPageService>());

            // act
            var result = helpPageService.DeleteAsync(documentId).Result;

            // assert
            A.CallTo(() => repository.DeleteAsync(documentId)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public void HelpPageServiceDeleteReturnsNullWhenHelpPageNotDeleted()
        {
            // arrange
            Guid documentId = Guid.NewGuid();
            var repository = A.Fake<ICosmosRepository<HelpPageModel>>();
            var expectedResult = false;

            A.CallTo(() => repository.DeleteAsync(documentId)).Returns(HttpStatusCode.BadRequest);

            var helpPageService = new HelpPageService(repository, A.Fake<IDraftHelpPageService>());

            // act
            var result = helpPageService.DeleteAsync(documentId).Result;

            // assert
            A.CallTo(() => repository.DeleteAsync(documentId)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public void HelpPageServiceDeleteReturnsFalseWhenMissingRepository()
        {
            // arrange
            Guid documentId = Guid.NewGuid();
            var repository = A.Dummy<ICosmosRepository<HelpPageModel>>();
            var helpPageModel = A.Fake<HelpPageModel>();
            var expectedResult = false;

            A.CallTo(() => repository.DeleteAsync(documentId)).Returns(HttpStatusCode.FailedDependency);

            var helpPageService = new HelpPageService(repository, A.Fake<IDraftHelpPageService>());

            // act
            var result = helpPageService.DeleteAsync(documentId).Result;

            // assert
            A.CallTo(() => repository.DeleteAsync(documentId)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }
    }
}
