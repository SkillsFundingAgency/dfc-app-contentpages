using DFC.App.ContentPages.Data.Contracts;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Threading.Tasks;

namespace DFC.App.ContentPages.PageService.IntegrationTests.ServiceTests.ContentPageServiceTests
{
    [TestFixture]
    public class ContentPageServicePingTests : BaseContentPageServiceTests
    {
        [Test]
        [Category("ContentPageService.Ping")]
        public async Task ContentPageServicePingReturnsSuccess()
        {
            // arrange
            var contentPageService = ServiceProvider.GetService<IContentPageService>();

            // act
            var result = await contentPageService.PingAsync().ConfigureAwait(false);

            // assert
            result.Should().Be(true);
        }
    }
}
