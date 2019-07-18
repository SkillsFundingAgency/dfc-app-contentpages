using DFC.App.Help.Data;
using DFC.App.Help.Data.Contracts;
using FakeItEasy;
using Xunit;

namespace DFC.App.Help.PageService.UnitTests.HelpPageServiceTests
{
    public class HelpPageServicePingTests
    {
        [Fact]
        public void HelpPageServicePingReturnsSuccess()
        {
            // arrange
            var repository = A.Fake<IRepository<HelpPageModel>>();
            var expectedResult = true;

            A.CallTo(() => repository.PingAsync()).Returns(expectedResult);

            var helpPageService = new HelpPageService(repository);

            // act
            var result = helpPageService.PingAsync().Result;

            // assert
            A.CallTo(() => repository.PingAsync()).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public void HelpPageServicePingReturnsFalseWhenMissingRepository()
        {
            // arrange
            var repository = A.Dummy<IRepository<HelpPageModel>>();
            var expectedResult = false;

            A.CallTo(() => repository.PingAsync()).Returns(expectedResult);

            var helpPageService = new HelpPageService(repository);

            // act
            var result = helpPageService.PingAsync().Result;

            // assert
            A.CallTo(() => repository.PingAsync()).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }
    }
}
