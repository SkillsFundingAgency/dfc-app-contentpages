using DFC.App.ContentPages.Data;
using DFC.App.ContentPages.Data.Contracts;
using FluentAssertions;
using Microsoft.Azure.Documents;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace DFC.App.ContentPages.PageService.IntegrationTests.ServiceTests.ContentPageServiceTests
{
    [TestFixture]
    public class ContentPageServiceUpdateTests : BaseContentPageServiceTests
    {
        [Test]
        [Category("ContentPageService.Update")]
        public async Task ContentPageServiceUpdateReturnsSuccessWhenContentPageUpdated()
        {
            // arrange
            const string name = ValidNameValue + "_Update";
            var contentPageModel = new ContentPageModel()
            {
                CanonicalName = name + "_" + Guid.NewGuid().ToString(),
                DocumentId = Guid.NewGuid(),
                LastReviewed = DateTime.UtcNow,
            };
            var contentPageService = ServiceProvider.GetService<IContentPageService>();

            var createdcontentPageModel = await contentPageService.CreateAsync(contentPageModel).ConfigureAwait(false);

            createdcontentPageModel.CanonicalName = createdcontentPageModel.CanonicalName.ToUpper(CultureInfo.CurrentCulture);
            createdcontentPageModel.BreadcrumbTitle = createdcontentPageModel.CanonicalName;

            // act
            var result = await contentPageService.ReplaceAsync(createdcontentPageModel).ConfigureAwait(false);

            // assert
            result.Should().NotBeNull();
            result.DocumentId.Should().Be(createdcontentPageModel.DocumentId);
            result.CanonicalName.Should().Be(createdcontentPageModel.CanonicalName);
            result.BreadcrumbTitle.Should().Be(createdcontentPageModel.BreadcrumbTitle);
        }

        [Test]
        [Category("ContentPageService.GetByAlternativeName")]
        public void ContentPageServiceUpdateReturnsArgumentNullExceptionWhenNullIsUsed()
        {
            // arrange
            var contentPageService = ServiceProvider.GetService<IContentPageService>();

            // act
            var exceptionResult = Assert.ThrowsAsync<ArgumentNullException>(async () => await contentPageService.ReplaceAsync(null).ConfigureAwait(false));

            //Assert
            Assert.AreEqual("Value cannot be null.\r\nParameter name: contentPageModel", exceptionResult.Message);
        }

        [Test]
        [Category("ContentPageService.Update")]
        public void ContentPageServiceUpdateReturnsExceptionWhenContentPageDoeNotExist()
        {
            // arrange
            const string name = ValidNameValue + "_Update";
            var contentPageModel = new ContentPageModel()
            {
                DocumentId = Guid.NewGuid(),
                CanonicalName = name + "_" + Guid.NewGuid().ToString(),
                LastReviewed = DateTime.UtcNow,
            };
            var contentPageService = ServiceProvider.GetService<IContentPageService>();

            // act
            Assert.ThrowsAsync<DocumentClientException>(() => contentPageService.ReplaceAsync(contentPageModel));

            // assert
        }
    }
}
