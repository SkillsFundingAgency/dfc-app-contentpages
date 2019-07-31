using DFC.App.Help.Data.Contracts;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Threading.Tasks;

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
            var helpPageService = ServiceProvider.GetService<IHelpPageService>();

            // act
            var result = await helpPageService.PingAsync().ConfigureAwait(false);

            // assert
            result.Should().Be(true);
        }
    }
}
