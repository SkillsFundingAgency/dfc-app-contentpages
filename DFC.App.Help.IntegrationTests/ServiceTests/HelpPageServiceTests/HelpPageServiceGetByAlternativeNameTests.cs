using System;
using System.Linq;
using System.Threading.Tasks;
using DFC.App.Help.Data;
using DFC.App.Help.Data.Contracts;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DFC.App.Help.IntegrationTests.ServiceTests.HelpPageServiceTests
{
    [TestFixture]
    public class HelpPageServiceGetByAlternativeNameTests : BaseHelpPageServiceTests
    {
        [Test]
        [Category("HelpPageService.GetByAlternativeName")]
        public async Task HelpPageServiceGetByAlternativeNameReturnsSuccessWhenHelpPageExists()
        {
            // arrange
            var name = ValidNameValue + "_GetByAlternativeName".ToLower();
            var alternativeName = ValidAlternativeNameValue + "_" + Guid.NewGuid().ToString().ToLower();
            var helpPageModel = new HelpPageModel()
            {
                CanonicalName = name + "_" + Guid.NewGuid().ToString(),
                DocumentId = Guid.NewGuid(),
                AlternativeNames = new[] { alternativeName },
            };
            var helpPageService = _serviceProvider.GetService<IHelpPageService>();

            await helpPageService.CreateAsync(helpPageModel);

            // act
            var result = await helpPageService.GetByAlternativeNameAsync(helpPageModel.AlternativeNames.First());

            // assert
            result.DocumentId.Should().Be(helpPageModel.DocumentId);
            result.CanonicalName.Should().Be(helpPageModel.CanonicalName);
        }

        [Test]
        [Category("HelpPageService.GetByAlternativeName")]
        public async Task HelpPageServiceGetByAlternativeNameReturnsNullWhenHelpPageDoesNotExist()
        {
            // arrange
            var helpPageService = _serviceProvider.GetService<IHelpPageService>();

            // act
            var result = await helpPageService.GetByAlternativeNameAsync(Guid.NewGuid().ToString());

            // assert
            result.Should().BeNull();
        }
    }
}
