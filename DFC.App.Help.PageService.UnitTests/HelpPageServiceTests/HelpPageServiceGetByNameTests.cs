using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DFC.App.Help.Data;
using DFC.App.Help.Data.Contracts;
using FakeItEasy;
using Xunit;

namespace DFC.App.Help.PageService.UnitTests.HelpPageServiceTests
{
    public class HelpPageServiceGetByNameTests
    {
        [Fact]
        public void HelpPageServiceGetByNameReturnsSuccess()
        {
            // arrange
            const string canonicalName = "name1";
            var repository = A.Fake<IRepository<HelpPageModel>>();
            var expectedResult = A.Fake<HelpPageModel>();

            A.CallTo(() => repository.GetAsync(A<Expression<Func<HelpPageModel, bool>>>.Ignored)).Returns(expectedResult);

            var helpPageService = new HelpPageService(repository);

            // act
            var result = helpPageService.GetByNameAsync(canonicalName).Result;

            // assert
            A.CallTo(() => repository.GetAsync(A<Expression<Func<HelpPageModel, bool>>>.Ignored)).MustHaveHappened();
            A.Equals(result, expectedResult);
        }
    }
}
