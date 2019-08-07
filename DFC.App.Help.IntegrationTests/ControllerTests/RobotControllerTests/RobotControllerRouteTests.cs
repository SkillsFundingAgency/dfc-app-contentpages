﻿using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.Help.IntegrationTests.ControllerTests.RobotControllerTests
{
    [Trait("Category", "Integration")]
    public class RobotControllerRouteTests : IClassFixture<CustomWebApplicationFactory<DFC.App.Help.Startup>>
    {
        private readonly CustomWebApplicationFactory<DFC.App.Help.Startup> factory;

        public RobotControllerRouteTests(CustomWebApplicationFactory<DFC.App.Help.Startup> factory)
        {
            this.factory = factory;

            DataSeeding.SeedDefaultArticle(factory, Controllers.PagesController.DefaultArticleName);
        }

        public static IEnumerable<object[]> RobotRouteData => new List<object[]>
        {
            new object[] { "/robots.txt" },
        };

        [Theory]
        [MemberData(nameof(RobotRouteData))]
        public async Task GetRobotTextContentEndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var uri = new Uri(url, UriKind.Relative);
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(MediaTypeNames.Text.Plain));

            // Act
            var response = await client.GetAsync(uri).ConfigureAwait(false);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(MediaTypeNames.Text.Plain, response.Content.Headers.ContentType.ToString());
        }
    }
}