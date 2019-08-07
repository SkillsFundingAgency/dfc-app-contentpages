using DFC.App.Help.Data;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.Help.PagesModule.UnitTests.ControllerTests.SitemapControllerTests
{
    [Trait("Category", "Sitemap Controller Unit Tests")]
    public class SitemapControllerSitemapTests : BaseSitemapController
    {
        [Fact]
        public async Task SitemapControllerSitemapReturnsSuccess()
        {
            // Arrange
            const int resultsCount = 3;
            var expectedResults = A.CollectionOfFake<HelpPageModel>(resultsCount);
            var controller = BuildSitemapController();

            expectedResults[0].IncludeInSitemap = true;
            expectedResults[0].CanonicalName = DFC.App.Help.Controllers.PagesController.DefaultArticleName;
            expectedResults[1].IncludeInSitemap = false;
            expectedResults[1].CanonicalName = "not-in-sitemap";
            expectedResults[2].IncludeInSitemap = true;
            expectedResults[2].CanonicalName = "in-sitemap";

            A.CallTo(() => FakeHelpPageService.GetAllAsync()).Returns(expectedResults);

            // Act
            var result = await controller.Sitemap().ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeHelpPageService.GetAllAsync()).MustHaveHappenedOnceExactly();

            var contentResult = Assert.IsType<ContentResult>(result);

            contentResult.ContentType.Should().Be(MediaTypeNames.Application.Xml);

            controller.Dispose();
        }

        [Fact]
        public async Task SitemapControllerSitemapReturnsSuccessWhenNoData()
        {
            // Arrange
            const int resultsCount = 0;
            var expectedResults = A.CollectionOfFake<HelpPageModel>(resultsCount);
            var controller = BuildSitemapController();

            A.CallTo(() => FakeHelpPageService.GetAllAsync()).Returns(expectedResults);

            // Act
            var result = await controller.Sitemap().ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeHelpPageService.GetAllAsync()).MustHaveHappenedOnceExactly();

            var contentResult = Assert.IsType<ContentResult>(result);

            contentResult.ContentType.Should().Be(MediaTypeNames.Application.Xml);

            controller.Dispose();
        }
    }
}
