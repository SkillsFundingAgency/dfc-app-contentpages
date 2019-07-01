using System;
using System.Threading.Tasks;
using DFC.App.Help.Models.Cosmos;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DFC.App.Help.IntegrationTests.ServiceTests.HelpPageServiceTests
{
    [TestFixture]
    public class HelpPageServiceGetByIdTests : BaseHelpPageServiceTests
    {
        [Test]
        [Category("HelpPageService.GetById")]
        public async Task HelpPageService_GetById_ReturnsSuccess_WhenHelpPageExists()
        {
            // arrange
            const string name = ValidNameValue + "_GetById";
            var helpPageModel = new HelpPageModel()
            {
                CanonicalName = name + "_" + Guid.NewGuid().ToString(),
                DocumentId = Guid.NewGuid()
            };
            var helpPageService = _serviceProvider.GetService<Services.IHelpPageService>();

            await helpPageService.CreateAsync(helpPageModel);
            
            // act
            var result = await helpPageService.GetByIdAsync(helpPageModel.DocumentId);

            // assert
            result.DocumentId.Should().Be(helpPageModel.DocumentId);
            result.Should().NotBeNull();
            result.CanonicalName.Should().Be(helpPageModel.CanonicalName);
        }

        [Test]
        [Category("HelpPageService.GetById")]
        public async Task HelpPageService_GetById_ReturnsNull_WhenHelpPageDoesNotExist()
        {
            // arrange
            var helpPageService = _serviceProvider.GetService<Services.IHelpPageService>();

            // act
            var result = await helpPageService.GetByIdAsync(Guid.NewGuid());

            // assert
            result.Should().BeNull();
        }
    }
}
