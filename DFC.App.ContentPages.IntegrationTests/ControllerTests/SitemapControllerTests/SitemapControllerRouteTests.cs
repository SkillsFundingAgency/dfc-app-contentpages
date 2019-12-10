using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContentPages.IntegrationTests.ControllerTests.SitemapControllerTests
{
    [Trait("Category", "Integration")]
    public class SitemapControllerRouteTests : IClassFixture<CustomWebApplicationFactory<DFC.App.ContentPages.Startup>>
    {
        private readonly CustomWebApplicationFactory<DFC.App.ContentPages.Startup> factory;

        public SitemapControllerRouteTests(CustomWebApplicationFactory<DFC.App.ContentPages.Startup> factory)
        {
            this.factory = factory;

            DataSeeding.SeedDefaultArticle(factory, Controllers.PagesController.CategoryNameForHelp, Controllers.PagesController.CategoryNameForAlert);
        }

        public static IEnumerable<object[]> SitemapRouteData => new List<object[]>
        {
            new object[] { $"/{Controllers.PagesController.CategoryNameForHelp}/sitemap.xml" },
            new object[] { $"/{Controllers.PagesController.CategoryNameForAlert}/sitemap.xml" },
        };

        [Theory]
        [MemberData(nameof(SitemapRouteData))]
        public async Task GetSitemapXmlContentEndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var uri = new Uri(url, UriKind.Relative);
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Xml));

            // Act
            var response = await client.GetAsync(uri).ConfigureAwait(false);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(MediaTypeNames.Application.Xml, response.Content.Headers.ContentType.ToString());
        }
    }
}