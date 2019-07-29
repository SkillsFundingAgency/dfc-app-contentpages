using DFC.App.Help.Data;
using DFC.App.Help.Data.Contracts;
using FakeItEasy;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.Help.PageService.UnitTests.HelpPageServiceTests
{
    public class HelpPageServiceGetByNameTests
    {
        private const string canonicalName = "name1";
        private readonly IRepository<HelpPageModel> repository;
        private readonly IDraftHelpPageService draftHelpPageService;
        private readonly IHelpPageService helpPageService;

        public HelpPageServiceGetByNameTests()
        {
            this.repository = A.Fake<IRepository<HelpPageModel>>();
            this.draftHelpPageService = A.Fake<IDraftHelpPageService>();
            this.helpPageService = new HelpPageService(repository, draftHelpPageService);
        }

        [Fact]
        public async Task HelpPageServiceGetByNameReturnsSuccess()
        {
            // arrange
            var expectedResult = A.Fake<HelpPageModel>();
            A.CallTo(() => repository.GetAsync(A<Expression<Func<HelpPageModel, bool>>>.Ignored)).Returns(expectedResult);

            // act
            var result = await helpPageService.GetByNameAsync(canonicalName).ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.GetAsync(A<Expression<Func<HelpPageModel, bool>>>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Equal(result, expectedResult);
        }

        [Fact]
        public async Task HelpPageServiceGetByNameReturnsNullWhenMissingRepository()
        {
            // arrange
            A.CallTo(() => repository.GetAsync(A<Expression<Func<HelpPageModel, bool>>>.Ignored)).Returns((HelpPageModel)null);

            // act
            var result = await helpPageService.GetByNameAsync(canonicalName).ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.GetAsync(A<Expression<Func<HelpPageModel, bool>>>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Null(result);
        }

        [Fact]
        public async Task HelpPageServiceGetByNameCallsDraftServiceWhenIsDraftIsTrue()
        {
            // arrange
            var fakeModel = new HelpPageModel { Content = "TestContent" };
            A.CallTo(() => draftHelpPageService.GetSitefinityData(A<string>.Ignored)).Returns(fakeModel);

            // act
            var result = await helpPageService.GetByNameAsync(canonicalName, true).ConfigureAwait(false);

            // assert
            A.CallTo(() => draftHelpPageService.GetSitefinityData(canonicalName)).MustHaveHappenedOnceExactly();
            Assert.Equal(result, fakeModel);
        }
    }
}