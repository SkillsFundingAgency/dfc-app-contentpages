using DFC.App.ContentPages.Data;
using DFC.App.ContentPages.Data.Contracts;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace DFC.App.ContentPages.PageService.IntegrationTests.ServiceTests.ContentPageServiceTests
{
    [TestFixture]
    public class ContentPageServiceGetByNameTests : BaseContentPageServiceTests
    {
        [Test]
        [Category("ContentPageService.GetByName")]
        public async Task ContentPageServiceGetByNameReturnsSuccessWhenContentPageExists()
        {
            // arrange
            var contentPageModel = new ContentPageModel
            {
                CanonicalName = $"{ValidNameValue}_getbyname_{Guid.NewGuid().ToString()}",
                DocumentId = Guid.NewGuid(),
                LastReviewed = DateTime.UtcNow,
            };
            var contentPageService = ServiceProvider.GetService<IContentPageService>();

            await contentPageService.CreateAsync(contentPageModel).ConfigureAwait(false);

            // act
            var result = await contentPageService.GetByNameAsync(contentPageModel.CanonicalName).ConfigureAwait(false);

            // assert
            Assert.True(result.DocumentId == contentPageModel.DocumentId);
            Assert.True(result.CanonicalName.Equals(contentPageModel.CanonicalName, StringComparison.OrdinalIgnoreCase));
        }

        [Test]
        [Category("ContentPageService.GetByName")]
        public void ContentPageServiceGetByNameReturnsArgumentNullExceptionWhenNullIsUsed()
        {
            // Arrange
            var contentPageService = ServiceProvider.GetService<IContentPageService>();

            // Act
            var exceptionResult = Assert.ThrowsAsync<ArgumentNullException>(async () => await contentPageService.GetByNameAsync(null).ConfigureAwait(false));

            //Assert
            Assert.AreEqual("Value cannot be null.\r\nParameter name: canonicalName", exceptionResult.Message);
        }

        [Test]
        [Category("ContentPageService.GetByName")]
        public async Task ContentPageServiceGetByNameReturnsNullWhenContentPageDoesNotExist()
        {
            // Arrange
            var contentPageService = ServiceProvider.GetService<IContentPageService>();

            // Act
            var result = await contentPageService.GetByNameAsync(Guid.NewGuid().ToString()).ConfigureAwait(false);

            //Assert
            Assert.Null(result);
        }
    }
}