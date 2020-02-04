using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContentPages.IntegrationTests.ControllerTests.PagesControllerTests
{
    [Trait("Category", "Integration")]
    public class PagesControllerRouteTests : IClassFixture<CustomWebApplicationFactory<DFC.App.ContentPages.Startup>>
    {
        private const string DefaultHelpArticleName = "help";
        private readonly CustomWebApplicationFactory<DFC.App.ContentPages.Startup> factory;

        public PagesControllerRouteTests(CustomWebApplicationFactory<DFC.App.ContentPages.Startup> factory)
        {
            this.factory = factory;

            DataSeeding.SeedDefaultArticle(factory, Controllers.PagesController.CategoryNameForHelp, Controllers.PagesController.CategoryNameForAlert);
        }

        public static IEnumerable<object[]> PagesContentRouteData => new List<object[]>
        {
            new object[] { "/pages" },
            new object[] { $"/pages/{Controllers.PagesController.CategoryNameForHelp}/{DefaultHelpArticleName}" },
            new object[] { $"/pages/{Controllers.PagesController.CategoryNameForHelp}/{DefaultHelpArticleName}/htmlhead" },
            new object[] { $"/pages/{Controllers.PagesController.CategoryNameForHelp}/htmlhead" },
            new object[] { $"/pages/{Controllers.PagesController.CategoryNameForHelp}/{DefaultHelpArticleName}/breadcrumb" },
            new object[] { $"/pages/{Controllers.PagesController.CategoryNameForHelp}/breadcrumb" },
            new object[] { $"/pages/{Controllers.PagesController.CategoryNameForHelp}/{DefaultHelpArticleName}/contents" },
            new object[] { $"/pages/{Controllers.PagesController.CategoryNameForHelp}/contents" },
        };

        public static IEnumerable<object[]> PagesNoContentRouteData => new List<object[]>
        {
            new object[] { $"/pages/{Controllers.PagesController.CategoryNameForHelp}/{DefaultHelpArticleName}/bodytop" },
            new object[] { $"/pages/{Controllers.PagesController.CategoryNameForHelp}/bodytop" },
            new object[] { $"/pages/{Controllers.PagesController.CategoryNameForHelp}/{DefaultHelpArticleName}/bodyfooter" },
            new object[] { $"/pages/{Controllers.PagesController.CategoryNameForHelp}/bodyfooter" },
        };

        [Theory]
        [MemberData(nameof(PagesContentRouteData))]
        public async Task GetPagesHtmlContentEndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var uri = new Uri(url, UriKind.Relative);
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(MediaTypeNames.Text.Html));

            // Act
            var response = await client.GetAsync(uri).ConfigureAwait(false);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal($"{MediaTypeNames.Text.Html}; charset={Encoding.UTF8.WebName}", response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [MemberData(nameof(PagesContentRouteData))]
        public async Task GetPagesJsonContentEndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var uri = new Uri(url, UriKind.Relative);
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

            // Act
            var response = await client.GetAsync(uri).ConfigureAwait(false);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal($"{MediaTypeNames.Application.Json}; charset={Encoding.UTF8.WebName}", response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [MemberData(nameof(PagesNoContentRouteData))]
        public async Task GetPagesEndpointsReturnSuccessAndNoContent(string url)
        {
            // Arrange
            var uri = new Uri(url, UriKind.Relative);
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Accept.Clear();

            // Act
            var response = await client.GetAsync(uri).ConfigureAwait(false);

            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task DeletePagesEndpointsReturnNotFound()
        {
            // Arrange
            var uri = new Uri($"/pages/{Guid.NewGuid()}", UriKind.Relative);
            var client = factory.CreateClient();

            client.DefaultRequestHeaders.Accept.Clear();

            // Act
            var response = await client.DeleteAsync(uri).ConfigureAwait(false);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}