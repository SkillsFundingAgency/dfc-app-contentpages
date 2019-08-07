using DFC.App.Help.Data;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.Help.IntegrationTests.ControllerTests.PagesControllerTests
{
    [Trait("Category", "Integration")]
    public class PagesControllerRouteTests : IClassFixture<CustomWebApplicationFactory<DFC.App.Help.Startup>>
    {
        private readonly CustomWebApplicationFactory<DFC.App.Help.Startup> factory;

        public PagesControllerRouteTests(CustomWebApplicationFactory<DFC.App.Help.Startup> factory)
        {
            this.factory = factory;

            DataSeeding.SeedDefaultArticle(factory, Controllers.PagesController.DefaultArticleName);
        }

        public static IEnumerable<object[]> DraftRouteData => new List<object[]>
        {
            new object[] { "/draft/htmlhead" },
            new object[] { $"/draft/{DFC.App.Help.Controllers.PagesController.DefaultArticleName}/htmlhead" },
            new object[] { "/draft/breadcrumb" },
            new object[] { $"/draft/{DFC.App.Help.Controllers.PagesController.DefaultArticleName}/breadcrumb" },
            new object[] { "/draft/contents" },
            new object[] { $"/draft/{DFC.App.Help.Controllers.PagesController.DefaultArticleName}/contents" },
        };

        public static IEnumerable<object[]> NonDraftRouteData => new List<object[]>
        {
            new object[] { "/pages" },
            new object[] { $"/pages/{DFC.App.Help.Controllers.PagesController.DefaultArticleName}" },
            new object[] { "/pages/htmlhead" },
            new object[] { $"/pages/{DFC.App.Help.Controllers.PagesController.DefaultArticleName}/htmlhead" },
            new object[] { "/pages/breadcrumb" },
            new object[] { $"/pages/{DFC.App.Help.Controllers.PagesController.DefaultArticleName}/breadcrumb" },
            new object[] { "/pages/contents" },
            new object[] { $"/pages/{DFC.App.Help.Controllers.PagesController.DefaultArticleName}/contents" },
        };

        public static IEnumerable<object[]> DraftNoContentRouteData => new List<object[]>
        {
            new object[] { "/draft/bodytop" },
            new object[] { $"/draft/{DFC.App.Help.Controllers.PagesController.DefaultArticleName}/bodytop" },
            new object[] { "/draft/bodyfooter" },
            new object[] { $"/draft/{DFC.App.Help.Controllers.PagesController.DefaultArticleName}/bodyfooter" },
        };

        public static IEnumerable<object[]> NonDraftNoContentRouteData => new List<object[]>
        {
            new object[] { "/pages/bodytop" },
            new object[] { $"/pages/{DFC.App.Help.Controllers.PagesController.DefaultArticleName}/bodytop" },
            new object[] { "/pages/bodyfooter" },
            new object[] { $"/pages/{DFC.App.Help.Controllers.PagesController.DefaultArticleName}/bodyfooter" },
        };

        [Theory]
        [MemberData(nameof(DraftRouteData))]
        public async Task GetDraftHtmlContentEndpointsReturnSuccessAndCorrectContentType(string url)
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
        [MemberData(nameof(DraftRouteData))]
        public async Task GetDraftJsonContentEndpointsReturnSuccessAndCorrectContentType(string url)
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
        [MemberData(nameof(NonDraftRouteData))]
        public async Task GetHelpHtmlContentEndpointsReturnSuccessAndCorrectContentType(string url)
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
        [MemberData(nameof(NonDraftRouteData))]
        public async Task GetHelpJsonContentEndpointsReturnSuccessAndCorrectContentType(string url)
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
        [MemberData(nameof(DraftNoContentRouteData))]
        public async Task GetDraftEndpointsReturnSuccessNoContent(string url)
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

        [Theory]
        [MemberData(nameof(NonDraftNoContentRouteData))]
        public async Task GetHelpEndpointsReturnSuccessAndNoContent(string url)
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
        public async Task PostHelpEndpointsReturnCreated()
        {
            // Arrange
            const string url = "/pages";
            var helpPageModel = new HelpPageModel()
            {
                DocumentId = Guid.NewGuid(),
                CanonicalName = Guid.NewGuid().ToString().ToLowerInvariant(),
                LastReviewed = DateTime.UtcNow,
            };
            var client = factory.CreateClient();

            client.DefaultRequestHeaders.Accept.Clear();

            // Act
            var response = await client.PostAsync(url, helpPageModel, new JsonMediaTypeFormatter()).ConfigureAwait(false);

            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task PuttHelpEndpointsReturnOk()
        {
            // Arrange
            const string url = "/pages";
            var helpPageModel = new HelpPageModel()
            {
                DocumentId = Guid.NewGuid(),
                CanonicalName = Guid.NewGuid().ToString().ToLowerInvariant(),
                LastReviewed = DateTime.UtcNow,
            };
            var client = factory.CreateClient();

            client.DefaultRequestHeaders.Accept.Clear();

            _ = await client.PostAsync(url, helpPageModel, new JsonMediaTypeFormatter()).ConfigureAwait(false);

            // Act
            var response = await client.PutAsync(url, helpPageModel, new JsonMediaTypeFormatter()).ConfigureAwait(false);

            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeleteHelpEndpointsReturnNotFound()
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