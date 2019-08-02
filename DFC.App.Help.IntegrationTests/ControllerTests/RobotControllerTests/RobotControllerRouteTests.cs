using Microsoft.AspNetCore.Mvc.Testing;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.Help.IntegrationTests.ControllerTests.RobotControllerTests
{
    public class RobotControllerRouteTests : IClassFixture<WebApplicationFactory<DFC.App.Help.Startup>>
    {
        private readonly WebApplicationFactory<DFC.App.Help.Startup> factory;

        public RobotControllerRouteTests(WebApplicationFactory<DFC.App.Help.Startup> factory)
        {
            this.factory = factory;
        }

        public static IEnumerable<object[]> RobotRouteData => new List<object[]>
        {
            new object[] {"/robot/robot" },
        };

        [Theory]
        [MemberData(nameof(RobotRouteData))]
        public async Task GeRobotTextContentEndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(MediaTypeNames.Text.Plain));

            // Act
            var response = await client.GetAsync(url).ConfigureAwait(false);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(MediaTypeNames.Text.Plain, response.Content.Headers.ContentType.ToString());
        }
    }
}