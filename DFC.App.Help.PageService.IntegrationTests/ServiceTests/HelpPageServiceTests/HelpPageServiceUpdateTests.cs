using DFC.App.Help.Data;
using DFC.App.Help.Data.Contracts;
using FluentAssertions;
using Microsoft.Azure.Documents;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace DFC.App.Help.PageService.IntegrationTests.ServiceTests.HelpPageServiceTests
{
    [TestFixture]
    public class HelpPageServiceUpdateTests : BaseHelpPageServiceTests
    {
        [Test]
        [Category("HelpPageService.Update")]
        public async Task HelpPageServiceUpdateReturnsSuccessWhenHelpPageUpdated()
        {
            // arrange
            const string name = ValidNameValue + "_Update";
            var helpPageModel = new HelpPageModel()
            {
                CanonicalName = name + "_" + Guid.NewGuid().ToString(),
                DocumentId = Guid.NewGuid(),
                LastReviewed = DateTime.UtcNow,
            };
            var helpPageService = ServiceProvider.GetService<IHelpPageService>();

            var createdHelpPageModel = await helpPageService.CreateAsync(helpPageModel).ConfigureAwait(false);

            createdHelpPageModel.CanonicalName = createdHelpPageModel.CanonicalName.ToUpper(CultureInfo.CurrentCulture);
            createdHelpPageModel.BreadcrumbTitle = createdHelpPageModel.CanonicalName;

            // act
            var result = await helpPageService.ReplaceAsync(createdHelpPageModel).ConfigureAwait(false);

            // assert
            result.Should().NotBeNull();
            result.DocumentId.Should().Be(createdHelpPageModel.DocumentId);
            result.CanonicalName.Should().Be(createdHelpPageModel.CanonicalName);
            result.BreadcrumbTitle.Should().Be(createdHelpPageModel.BreadcrumbTitle);
        }

        [Test]
        [Category("HelpPageService.Update")]
        public void HelpPageServiceUpdateReturnsExceptionWhenHelpPageDoeNotExist()
        {
            // arrange
            const string name = ValidNameValue + "_Update";
            var helpPageModel = new HelpPageModel()
            {
                DocumentId = Guid.NewGuid(),
                CanonicalName = name + "_" + Guid.NewGuid().ToString(),
                LastReviewed = DateTime.UtcNow,
            };
            var helpPageService = ServiceProvider.GetService<IHelpPageService>();

            // act
            Assert.ThrowsAsync<DocumentClientException>(() => helpPageService.ReplaceAsync(helpPageModel));

            // assert
        }
    }
}
