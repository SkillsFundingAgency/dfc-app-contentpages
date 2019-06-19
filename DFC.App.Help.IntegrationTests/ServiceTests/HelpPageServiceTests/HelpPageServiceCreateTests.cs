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
                Name = name + "_" + Guid.NewGuid().ToString()
            };
            var helpPageService = _serviceProvider.GetService<Services.IHelpPageService>();

            // act
            var createdHelpPageModel = await helpPageService.CreateAsync(helpPageModel);
            helpPageModel.DocumentId = createdHelpPageModel.DocumentId;

            var result = await helpPageService.GetByIdAsync(helpPageModel.DocumentId.Value);

            // assert
            result.Should().NotBeNull();
            result.DocumentId.Should().Be(helpPageModel.DocumentId);
            result.Name.Should().Be(helpPageModel.Name);
        }
    }
}
