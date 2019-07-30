using DFC.App.Help.Data;
using DFC.App.Help.Data.Contracts;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace DFC.App.Help.IntegrationTests.ServiceTests.HelpPageServiceTests
{
    [TestFixture]
    public class HelpPageServiceGetByNameTests : BaseHelpPageServiceTests
    {
        [Test]
        [Category("HelpPageService.GetByName")]
        public async Task HelpPageServiceGetByNameReturnsSuccessWhenHelpPageExists()
        {
            // arrange
            var helpPageModel = new HelpPageModel
            {
                CanonicalName = $"{ValidNameValue}_getbyname_{Guid.NewGuid().ToString()}",
                DocumentId = Guid.NewGuid(),
            };
            var helpPageService = ServiceProvider.GetService<IHelpPageService>();

            await helpPageService.CreateAsync(helpPageModel).ConfigureAwait(false);

            // act
            var result = await helpPageService.GetByNameAsync(helpPageModel.CanonicalName).ConfigureAwait(false);

            // assert
            Assert.True(result.DocumentId == helpPageModel.DocumentId
                        && result.CanonicalName.Equals(helpPageModel.CanonicalName, StringComparison.InvariantCulture));
        }

        [Test]
        [Category("HelpPageService.GetByName")]
        public async Task HelpPageServiceGetByNameReturnsNullWhenHelpPageDoesNotExist()
        {
            // Arrange
            var helpPageService = ServiceProvider.GetService<IHelpPageService>();

            // Act
            var result = await helpPageService.GetByNameAsync(Guid.NewGuid().ToString()).ConfigureAwait(false);

            //Assert
            Assert.Null(result);
        }

        [Test]
        [Category("HelpPageService.GetByName")]
        public async Task HelpPageServiceGetByNameReturnsModelFromSitefinityWhenIsDraftIsTrue()
        {
            // arrange
            var helpPageService = ServiceProvider.GetService<IHelpPageService>();

            // act
            var result = await helpPageService.GetByNameAsync("help", true).ConfigureAwait(false);

            // assert
            Assert.True(!string.IsNullOrEmpty(result.Content));
        }
    }
}