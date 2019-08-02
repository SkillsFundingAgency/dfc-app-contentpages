using DFC.App.Help.Data;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Mime;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.Help.IntegrationTests.ControllerTests.PagesControllerTests
{
    public class PagesControllerRouteTests : IClassFixture<WebApplicationFactory<DFC.App.Help.Startup>>
    {
        private readonly WebApplicationFactory<DFC.App.Help.Startup> factory;

        public PagesControllerRouteTests(WebApplicationFactory<DFC.App.Help.Startup> factory)
        {
            this.factory = factory;
        }

        public static IEnumerable<object[]> DraftRouteData => new List<object[]>
        {
            new object[] { "/draft/htmlhead" },
            new object[] { "/draft/help/htmlhead" },
            new object[] { "/draft/breadcrumb" },
            new object[] { "/draft/help/breadcrumb" },
            new object[] { "/draft/contents" },
            new object[] { "/draft/help/contents" },
        };

        public static IEnumerable<object[]> NonDraftRouteData => new List<object[]>
        {
            new object[] { "/pages" },
            new object[] { "/pages/help" },
            new object[] { "/pages/htmlhead" },
            new object[] { "/pages/help/htmlhead" },
            new object[] { "/pages/breadcrumb" },
            new object[] { "/pages/help/breadcrumb" },
            new object[] { "/pages/contents" },
            new object[] { "/pages/help/contents" },
        };

        public static IEnumerable<object[]> DraftNoContentRouteData => new List<object[]>
        {
            new object[] { "/draft/bodytop" },
            new object[] { "/draft/help/bodytop" },
            new object[] { "/draft/bodyfooter" },
            new object[] { "/draft/help/bodyfooter" },
        };

        public static IEnumerable<object[]> NonDraftNoContentRouteData => new List<object[]>
        {
            new object[] { "/pages/bodytop" },
            new object[] { "/pages/help/bodytop" },
            new object[] { "/pages/bodyfooter" },
            new object[] { "/pages/help/bodyfooter" },
        };

        [Theory]
        [MemberData(nameof(DraftRouteData))]
        public async Task GeDraftHtmlContentEndpointsReturnSuccessAndCorrectContentType(string url)
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
        [MemberData(nameof(DraftRouteData))]
        public async Task GetDraftJsonContentEndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

            // Act
            var response = await client.GetAsync(url).ConfigureAwait(false);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal($"{MediaTypeNames.Application.Json}; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [MemberData(nameof(NonDraftRouteData))]
        public async Task GetHelpHtmlContentEndpointsReturnSuccessAndCorrectContentType(string url)
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
        [MemberData(nameof(NonDraftRouteData))]
        public async Task GetHelpJsonContentEndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

            // Act
            var response = await client.GetAsync(url).ConfigureAwait(false);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal($"{MediaTypeNames.Application.Json}; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [MemberData(nameof(DraftNoContentRouteData))]
        public async Task GetDraftEndpointsReturnSuccessNoContent(string url)
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Accept.Clear();

            // Act
            var response = await client.GetAsync(url).ConfigureAwait(false);

            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Theory]
        [MemberData(nameof(NonDraftNoContentRouteData))]
        public async Task GetHelpEndpointsReturnSuccessAndNoContent(string url)
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Accept.Clear();

            // Act
            var response = await client.GetAsync(url).ConfigureAwait(false);

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
            string url = $"/pages/{Guid.NewGuid()}";
            var client = factory.CreateClient();

            client.DefaultRequestHeaders.Accept.Clear();

            // Act
            var response = await client.DeleteAsync(url).ConfigureAwait(false);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}