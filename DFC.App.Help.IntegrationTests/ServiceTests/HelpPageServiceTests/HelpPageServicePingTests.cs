using System.Threading.Tasks;
using DFC.App.Help.Data.Contracts;
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
        public async Task HelpPageServicePingReturnsSuccess()
        {
            // arrange
            var helpPageService = _serviceProvider.GetService<IHelpPageService>();

            // act
            var result = await helpPageService.PingAsync();

            // assert
            result.Should().Be(true);
        }
    }
}
