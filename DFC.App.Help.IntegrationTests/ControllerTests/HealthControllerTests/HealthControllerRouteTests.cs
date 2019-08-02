using Microsoft.AspNetCore.Mvc.Testing;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.Help.IntegrationTests.ControllerTests.HealthControllerTests
{
    public class HealthControllerRouteTests : IClassFixture<WebApplicationFactory<DFC.App.Help.Startup>>
    {
        private readonly WebApplicationFactory<DFC.App.Help.Startup> factory;

        public HealthControllerRouteTests(WebApplicationFactory<DFC.App.Help.Startup> factory)
        {
            this.factory = factory;
        }

        public static IEnumerable<object[]> HealthContentRouteData => new List<object[]>
        {
            new object[] { "/pages/health" },
        };

        public static IEnumerable<object[]> HealthOkRouteData => new List<object[]>
        {
            new object[] { "/health/ping" },
        };

        [Theory]
        [MemberData(nameof(HealthContentRouteData))]
        public async Task GeHealthHtmlContentEndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(MediaTypeNames.Text.Html));

            // Act
            var response = await client.GetAsync(url).ConfigureAwait(false);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal($"{MediaTypeNames.Text.Html}; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [MemberData(nameof(HealthOkRouteData))]
        public async Task GeHealthOkEndpointsReturnSuccess(string url)
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Accept.Clear();

            // Act
            var response = await client.GetAsync(url).ConfigureAwait(false);

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}