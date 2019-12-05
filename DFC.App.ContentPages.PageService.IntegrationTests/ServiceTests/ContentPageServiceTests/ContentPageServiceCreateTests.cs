using DFC.App.ContentPages.Data;
using DFC.App.ContentPages.Data.Contracts;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace DFC.App.ContentPages.PageService.IntegrationTests.ServiceTests.ContentPageServiceTests
{
    [TestFixture]
    public class ContentPageServiceCreateTests : BaseContentPageServiceTests
    {
        [Test]
        [Category("ContentPageService.Create")]
        public async Task ContentPageServiceCreateReturnsSuccessWhenContentPageCreated()
        {
            // arrange
            const string name = ValidNameValue + "_Create";
            var contentPageModel = new ContentPageModel()
            {
                CanonicalName = name + "_" + Guid.NewGuid().ToString(),
                DocumentId = Guid.NewGuid(),
                LastReviewed = DateTime.UtcNow,
            };
            var contentPageService = ServiceProvider.GetService<IContentPageService>();

            // act
            await contentPageService.CreateAsync(contentPageModel).ConfigureAwait(false);

            var result = await contentPageService.GetByIdAsync(contentPageModel.DocumentId).ConfigureAwait(false);

            // assert
            result.Should().NotBeNull();
            result.DocumentId.Should().Be(contentPageModel.DocumentId);
            result.CanonicalName.Should().Be(contentPageModel.CanonicalName);
        }

        [Test]
        [Category("ContentPageService.GetByAlternativeName")]
        public void ContentPageServiceCreateReturnsArgumentNullExceptionWhenNullIsUsed()
        {
            // arrange
            var contentPageService = ServiceProvider.GetService<IContentPageService>();

            // act
            var exceptionResult = Assert.ThrowsAsync<ArgumentNullException>(async () => await contentPageService.CreateAsync(null).ConfigureAwait(false));

            //Assert
            Assert.AreEqual("Value cannot be null.\r\nParameter name: contentPageModel", exceptionResult.Message);
        }
    }
}
