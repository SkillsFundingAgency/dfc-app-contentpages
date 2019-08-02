using Microsoft.AspNetCore.Mvc.Testing;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.Help.IntegrationTests.ControllerTests.SitemapControllerTests
{
    public class SitemapControllerRouteTests : IClassFixture<WebApplicationFactory<DFC.App.Help.Startup>>
    {
        private readonly WebApplicationFactory<DFC.App.Help.Startup> factory;

        public SitemapControllerRouteTests(WebApplicationFactory<DFC.App.Help.Startup> factory)
        {
            this.factory = factory;
        }

        public static IEnumerable<object[]> SitemapRouteData => new List<object[]>
        {
            new object[] { "/Sitemap/Sitemap" },
        };

        [Theory]
        [MemberData(nameof(SitemapRouteData))]
        public async Task GeSitemapXmlContentEndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Xml));

            // Act
            var response = await client.GetAsync(url).ConfigureAwait(false);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(MediaTypeNames.Application.Xml, response.Content.Headers.ContentType.ToString());
        }
    }
}