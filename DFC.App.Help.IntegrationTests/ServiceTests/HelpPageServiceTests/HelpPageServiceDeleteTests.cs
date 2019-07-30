using DFC.App.Help.Data;
using DFC.App.Help.Data.Contracts;
using FluentAssertions;
using Microsoft.Azure.Documents;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace DFC.App.Help.IntegrationTests.ServiceTests.HelpPageServiceTests
{
    [TestFixture]
    public class HelpPageServiceDeleteTests : BaseHelpPageServiceTests
    {
        [Test]
        [Category("HelpPageService.Delete")]
        public async Task HelpPageServiceDeleteReturnsSuccessWhenHelpPageDeleted()
        {
            // arrange
            const string name = ValidNameValue + "_Delete";
            var helpPageModel = new HelpPageModel()
            {
                CanonicalName = name + "_" + Guid.NewGuid().ToString(),
            };
            var helpPageService = ServiceProvider.GetService<IHelpPageService>();

            var createdHelpPageModel = await helpPageService.CreateAsync(helpPageModel).ConfigureAwait(false);

            // act
            var result = await helpPageService.DeleteAsync(createdHelpPageModel.DocumentId).ConfigureAwait(false);

            // assert
            result.Should().BeTrue();
        }

        [Test]
        [Category("HelpPageService.Delete")]
        public void HelpPageServiDeleteReturnsExceptionWhenHelpPageDoeNotExist()
        {
            // arrange
            var helpPageService = ServiceProvider.GetService<IHelpPageService>();

            // act
            Assert.ThrowsAsync<DocumentClientException>(() => helpPageService.DeleteAsync(Guid.NewGuid()));

            // assert
        }
    }
}
