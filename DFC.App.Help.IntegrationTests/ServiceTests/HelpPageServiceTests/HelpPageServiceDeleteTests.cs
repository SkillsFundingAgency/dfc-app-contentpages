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
    public class HelpPageServiceDeleteTests : BaseHelpPageServiceTests
    {
        [Test]
        [Category("HelpPageService.Delete")]
        public async Task HelpPageService_Delete_ReturnsSuccess_WhenHelpPageDeleted()
        {
            // arrange
            const string name = ValidNameValue + "_Delete";
            var helpPageModel = new HelpPageModel()
            {
                CanonicalName = name + "_" + Guid.NewGuid().ToString()
            };
            var helpPageService = _serviceProvider.GetService<Services.IHelpPageService>();

            var createdHelpPageModel = await helpPageService.CreateAsync(helpPageModel);

            // act
            var result = await helpPageService.DeleteAsync(createdHelpPageModel.DocumentId);

            // assert
            result.Should().BeTrue();
        }

        [Test]
        [Category("HelpPageService.Delete")]
        public void HelpPageService_Delete_ReturnsException_WhenHelpPageDoeNotExist()
        {
            // arrange
            var helpPageService = _serviceProvider.GetService<Services.IHelpPageService>();

            // act
            Assert.ThrowsAsync<DocumentClientException>(() => helpPageService.DeleteAsync(Guid.NewGuid()));

            // assert
        }

    }
}
