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
            const int resultsCount = 2;
            var expectedResults = A.CollectionOfFake<HelpPageModel>(resultsCount);
            var controller = BuildSitemapController();

            expectedResults.ToList().ForEach(f =>
            {
                f.IncludeInSitemap = true;
                f.CanonicalName = "an-article";
            });

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
