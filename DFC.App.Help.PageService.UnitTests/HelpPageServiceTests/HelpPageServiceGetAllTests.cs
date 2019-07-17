using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DFC.App.Help.Data;
using DFC.App.Help.Data.Contracts;
using FakeItEasy;
using Xunit;

namespace DFC.App.Help.PageService.UnitTests.HelpPageServiceTests
{
    public class HelpPageServiceGetAllTests
    {
        [Fact]
        public void HelpPageServiceGetAllListReturnsSuccess()
        {
            // arrange
            var repository = A.Fake<IRepository<HelpPageModel>>();
            var expectedResults = A.CollectionOfFake<HelpPageModel>(2);

            A.CallTo(() => repository.GetAllAsync()).Returns(expectedResults);

            var helpPageService = new HelpPageService(repository);

            // act
            var results = helpPageService.GetAllAsync().Result;

            // assert
            A.CallTo(() => repository.GetAllAsync()).MustHaveHappened();
            A.Equals(results, expectedResults);
        }
    }
}
