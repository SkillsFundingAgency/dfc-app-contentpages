using DFC.App.Help.Data;
using DFC.App.Help.Data.Contracts;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.App.Help.PageService.IntegrationTests.ServiceTests.HelpPageServiceTests
{
    [TestFixture]
    public class HelpPageServiceGetByAlternativeNameTests : BaseHelpPageServiceTests
    {
        [Test]
        [Category("HelpPageService.GetByAlternativeName")]
        public async Task HelpPageServiceGetByAlternativeNameReturnsSuccessWhenHelpPageExists()
        {
            // arrange
            var name = ValidNameValue + "_GetByAlternativeName".ToLowerInvariant();
            var alternativeName = ValidAlternativeNameValue + "_" + Guid.NewGuid().ToString().ToLowerInvariant();
            var helpPageModel = new HelpPageModel()
            {
                CanonicalName = name + "_" + Guid.NewGuid().ToString(),
                DocumentId = Guid.NewGuid(),
                LastReviewed = DateTime.UtcNow,
                AlternativeNames = new[] { alternativeName },
            };
            var helpPageService = ServiceProvider.GetService<IHelpPageService>();

            await helpPageService.CreateAsync(helpPageModel).ConfigureAwait(false);

            // act
            var result = await helpPageService.GetByAlternativeNameAsync(helpPageModel.AlternativeNames.First()).ConfigureAwait(false);

            // assert
            result.DocumentId.Should().Be(helpPageModel.DocumentId);
            result.CanonicalName.Should().Be(helpPageModel.CanonicalName);
        }

        [Test]
        [Category("HelpPageService.GetByAlternativeName")]
        public async Task HelpPageServiceGetByAlternativeNameReturnsNullWhenHelpPageDoesNotExist()
        {
            // arrange
            var helpPageService = ServiceProvider.GetService<IHelpPageService>();

            // act
            var result = await helpPageService.GetByAlternativeNameAsync(Guid.NewGuid().ToString()).ConfigureAwait(false);

            // assert
            result.Should().BeNull();
        }
    }
}
