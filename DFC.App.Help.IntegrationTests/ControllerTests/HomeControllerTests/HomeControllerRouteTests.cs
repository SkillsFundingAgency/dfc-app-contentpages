using Microsoft.AspNetCore.Mvc.Testing;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.Help.IntegrationTests.ControllerTests.HomeControllerTests
{
    public class HomeControllerRouteTests : IClassFixture<WebApplicationFactory<DFC.App.Help.Startup>>
    {
        private readonly WebApplicationFactory<DFC.App.Help.Startup> factory;

        public HomeControllerRouteTests(WebApplicationFactory<DFC.App.Help.Startup> factory)
        {
            this.factory = factory;
        }

        public static IEnumerable<object[]> HomeContentRouteData => new List<object[]>
        {
            new object[] { "/" },
            new object[] { "/Home" },
            new object[] { "/Home/Error" },
        };

        [Theory]
        [MemberData(nameof(HomeContentRouteData))]
        public async Task GeHomeHtmlContentEndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Accept.Clear();

            // Act
            var response = await client.GetAsync(url).ConfigureAwait(false);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal($"{MediaTypeNames.Text.Html}; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }
    }
}