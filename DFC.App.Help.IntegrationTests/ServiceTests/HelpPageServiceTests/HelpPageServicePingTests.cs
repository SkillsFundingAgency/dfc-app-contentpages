using System;
using System.Threading.Tasks;
using DFC.App.Help.Models.Cosmos;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DFC.App.Help.IntegrationTests.ServiceTests.HelpPageServiceTests
{
    [TestFixture]
    public class HelpPageServicePingTests : BaseHelpPageServiceTests
    {
        [Test]
        [Category("HelpPageService.Ping")]
        public async Task HelpPageService_Ping_ReturnsSuccess()
        {
            // arrange
            var helpPageService = _serviceProvider.GetService<Services.IHelpPageService>();

            // act
            var result = await helpPageService.PingAsync();

            // assert
            result.Should().Be(true);
        }
    }
}
