using DFC.App.ContentPages.Data;
using DFC.App.ContentPages.Data.Contracts;
using FakeItEasy;
using System.Collections.Generic;
using Xunit;

namespace DFC.App.ContentPages.PageService.UnitTests.ContentPageServiceTests
{
    [Trait("Category", "Page Service Unit Tests")]
    public class ContentPageServiceGetAllTests
    {
        [Fact]
        public void ContentPageServiceGetAllListReturnsSuccess()
        {
            // arrange
            var repository = A.Fake<ICosmosRepository<ContentPageModel>>();
            var expectedResults = A.CollectionOfFake<ContentPageModel>(2);

            A.CallTo(() => repository.GetAllAsync()).Returns(expectedResults);

            var contentPageService = new ContentPageService(repository);

            // act
            var results = contentPageService.GetAllAsync().Result;

            // assert
            A.CallTo(() => repository.GetAllAsync()).MustHaveHappenedOnceExactly();
            A.Equals(results, expectedResults);
        }

        [Fact]
        public void ContentPageServiceGetAllListReturnsNullWhenMissingRepository()
        {
            // arrange
            var repository = A.Dummy<ICosmosRepository<ContentPageModel>>();
            IEnumerable<ContentPageModel> expectedResults = null;

            A.CallTo(() => repository.GetAllAsync()).Returns(expectedResults);

            var contentPageService = new ContentPageService(repository);

            // act
            var results = contentPageService.GetAllAsync().Result;

            // assert
            A.CallTo(() => repository.GetAllAsync()).MustHaveHappenedOnceExactly();
            A.Equals(results, expectedResults);
        }
    }
}
