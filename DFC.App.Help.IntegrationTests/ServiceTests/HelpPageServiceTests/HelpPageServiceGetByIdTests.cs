using DFC.App.Help.Data;
using DFC.App.Help.Data.Contracts;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace DFC.App.Help.IntegrationTests.ServiceTests.HelpPageServiceTests
{
    [TestFixture]
    public class HelpPageServiceGetByIdTests : BaseHelpPageServiceTests
    {
        [Test]
        [Category("HelpPageService.GetById")]
        public async Task HelpPageServiceGetByIdReturnsSuccessWhenHelpPageExists()
        {
            // arrange
            const string name = ValidNameValue + "_GetById";
            var helpPageModel = new HelpPageModel()
            {
                CanonicalName = name + "_" + Guid.NewGuid().ToString(),
                DocumentId = Guid.NewGuid(),
            };
            var helpPageService = ServiceProvider.GetService<IHelpPageService>();

            await helpPageService.CreateAsync(helpPageModel).ConfigureAwait(false);

            // act
            var result = await helpPageService.GetByIdAsync(helpPageModel.DocumentId).ConfigureAwait(false);

            // assert
            result.DocumentId.Should().Be(helpPageModel.DocumentId);
            result.Should().NotBeNull();
            result.CanonicalName.Should().Be(helpPageModel.CanonicalName);
        }

        [Test]
        [Category("HelpPageService.GetById")]
        public async Task HelpPageServiceGetByIdReturnsNullWhenHelpPageDoesNotExist()
        {
            // arrange
            var helpPageService = ServiceProvider.GetService<IHelpPageService>();

            // act
            var result = await helpPageService.GetByIdAsync(Guid.NewGuid()).ConfigureAwait(false);

            // assert
            result.Should().BeNull();
        }
    }
}
