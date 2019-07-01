using System;
using System.Threading.Tasks;
using DFC.App.Help.Models.Cosmos;
using FluentAssertions;
using Microsoft.Azure.Documents;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DFC.App.Help.IntegrationTests.ServiceTests.HelpPageServiceTests
{
    [TestFixture]
    public class HelpPageServiceUpdateTests : BaseHelpPageServiceTests
    {
        [Test]
        [Category("HelpPageService.Update")]
        public async Task HelpPageService_Update_ReturnsSuccess_WhenHelpPageUpdated()
        {
            // arrange
            const string name = ValidNameValue + "_Update";
            var helpPageModel = new HelpPageModel()
            {
                CanonicalName = name + "_" + Guid.NewGuid().ToString(),
                DocumentId = Guid.NewGuid()
            };
            var helpPageService = _serviceProvider.GetService<Services.IHelpPageService>();

            var createdHelpPageModel = await helpPageService.CreateAsync(helpPageModel);

            createdHelpPageModel.CanonicalName = createdHelpPageModel.CanonicalName.ToUpper();
            createdHelpPageModel.BreadcrumbTitle = createdHelpPageModel.CanonicalName;

            // act
            var result = await helpPageService.ReplaceAsync(createdHelpPageModel);

            // assert
            result.Should().NotBeNull();
            result.DocumentId.Should().Be(createdHelpPageModel.DocumentId);
            result.CanonicalName.Should().Be(createdHelpPageModel.CanonicalName);
            result.BreadcrumbTitle.Should().Be(createdHelpPageModel.BreadcrumbTitle);
        }

        [Test]
        [Category("HelpPageService.Update")]
        public void HelpPageService_Update_ReturnsException_WhenHelpPageDoeNotExist()
        {
            // arrange
            const string name = ValidNameValue + "_Update";
            var helpPageModel = new HelpPageModel()
            {
                DocumentId = Guid.NewGuid(),
                CanonicalName = name + "_" + Guid.NewGuid().ToString()
            };
            var helpPageService = _serviceProvider.GetService<Services.IHelpPageService>();

            // act
            Assert.ThrowsAsync<DocumentClientException>(() => helpPageService.ReplaceAsync(helpPageModel));

            // assert
        }

    }
}
