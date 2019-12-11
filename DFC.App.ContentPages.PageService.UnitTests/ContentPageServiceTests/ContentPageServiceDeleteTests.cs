using DFC.App.ContentPages.Data.Contracts;
using DFC.App.ContentPages.Data.Models;
using FakeItEasy;
using System;
using System.Net;
using Xunit;

namespace DFC.App.ContentPages.PageService.UnitTests.ContentPageServiceTests
{
    [Trait("Category", "Page Service Unit Tests")]
    public class ContentPageServiceDeleteTests
    {
        [Fact]
        public void ContentPageServiceDeleteReturnsSuccessWhenContentPageDeleted()
        {
            // arrange
            Guid documentId = Guid.NewGuid();
            var repository = A.Fake<ICosmosRepository<ContentPageModel>>();
            var expectedResult = true;

            A.CallTo(() => repository.DeleteAsync(documentId)).Returns(HttpStatusCode.NoContent);

            var contentPageService = new ContentPageService(repository);

            // act
            var result = contentPageService.DeleteAsync(documentId).Result;

            // assert
            A.CallTo(() => repository.DeleteAsync(documentId)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public void ContentPageServiceDeleteReturnsNullWhenContentPageNotDeleted()
        {
            // arrange
            Guid documentId = Guid.NewGuid();
            var repository = A.Fake<ICosmosRepository<ContentPageModel>>();
            var expectedResult = false;

            A.CallTo(() => repository.DeleteAsync(documentId)).Returns(HttpStatusCode.BadRequest);

            var contentPageService = new ContentPageService(repository);

            // act
            var result = contentPageService.DeleteAsync(documentId).Result;

            // assert
            A.CallTo(() => repository.DeleteAsync(documentId)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public void ContentPageServiceDeleteReturnsFalseWhenMissingRepository()
        {
            // arrange
            Guid documentId = Guid.NewGuid();
            var repository = A.Dummy<ICosmosRepository<ContentPageModel>>();
            var expectedResult = false;

            A.CallTo(() => repository.DeleteAsync(documentId)).Returns(HttpStatusCode.FailedDependency);

            var contentPageService = new ContentPageService(repository);

            // act
            var result = contentPageService.DeleteAsync(documentId).Result;

            // assert
            A.CallTo(() => repository.DeleteAsync(documentId)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }
    }
}
