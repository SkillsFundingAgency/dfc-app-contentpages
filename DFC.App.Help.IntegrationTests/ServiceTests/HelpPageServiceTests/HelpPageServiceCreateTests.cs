using System;
using System.Threading.Tasks;
using DFC.App.Help.Models.Cosmos;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DFC.App.Help.IntegrationTests.ServiceTests.HelpPageServiceTests
{
    [TestFixture]
    public class HelpPageServiceCreateTests : BaseHelpPageServiceTests
    {
        [Test]
        [Category("HelpPageService.Create")]
        public async Task HelpPageService_Create_ReturnsSuccess_WhenHelpPageCreated()
        {
            // arrange
            const string name = ValidNameValue + "_Create";
            var helpPageModel = new HelpPageModel()
            {
                CanonicalName = name + "_" + Guid.NewGuid().ToString(),
                DocumentId = Guid.NewGuid()
            };
            var helpPageService = _serviceProvider.GetService<Services.IHelpPageService>();

            // act
            await helpPageService.CreateAsync(helpPageModel);

            var result = await helpPageService.GetByIdAsync(helpPageModel.DocumentId);

            // assert
            result.Should().NotBeNull();
            result.DocumentId.Should().Be(helpPageModel.DocumentId);
            result.CanonicalName.Should().Be(helpPageModel.CanonicalName);
        }
    }
}
