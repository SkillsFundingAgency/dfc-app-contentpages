using DFC.App.ContentPages.Data;
using DFC.App.ContentPages.Data.Contracts;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.App.ContentPages.PageService.IntegrationTests.ServiceTests.ContentPageServiceTests
{
    [TestFixture]
    public class ContentPageServiceGetByAlternativeNameTests : BaseContentPageServiceTests
    {
        [Test]
        [Category("ContentPageService.GetByAlternativeName")]
        public async Task ContentPageServiceGetByAlternativeNameReturnsSuccessWhenContentPageExists()
        {
            // arrange
            var name = ValidNameValue + "_GetByAlternativeName".ToLowerInvariant();
            var alternativeName = ValidAlternativeNameValue + "_" + Guid.NewGuid().ToString().ToLowerInvariant();
            var contentPageModel = new ContentPageModel()
            {
                CanonicalName = name + "_" + Guid.NewGuid().ToString(),
                DocumentId = Guid.NewGuid(),
                LastReviewed = DateTime.UtcNow,
                AlternativeNames = new[] { alternativeName },
            };
            var contentPageService = ServiceProvider.GetService<IContentPageService>();

            await contentPageService.CreateAsync(contentPageModel).ConfigureAwait(false);

            // act
            var result = await contentPageService.GetByAlternativeNameAsync(contentPageModel.AlternativeNames.First()).ConfigureAwait(false);

            // assert
            result.DocumentId.Should().Be(contentPageModel.DocumentId);
            result.CanonicalName.Should().Be(contentPageModel.CanonicalName);
        }

        [Test]
        [Category("ContentPageService.GetByAlternativeName")]
        public async Task ContentPageServiceGetByAlternativeNameReturnsArgumentNullExceptionWhenNullIsUsed()
        {
            // arrange
            var contentPageService = ServiceProvider.GetService<IContentPageService>();

            // act
            var exceptionResult = Assert.ThrowsAsync<ArgumentNullException>(async () => await contentPageService.GetByAlternativeNameAsync(null).ConfigureAwait(false));

            //Assert
            Assert.AreEqual("Value cannot be null.\r\nParameter name: alternativeName", exceptionResult.Message);
        }

        [Test]
        [Category("ContentPageService.GetByAlternativeName")]
        public async Task ContentPageServiceGetByAlternativeNameReturnsNullWhenContentPageDoesNotExist()
        {
            // arrange
            var contentPageService = ServiceProvider.GetService<IContentPageService>();

            // act
            var result = await contentPageService.GetByAlternativeNameAsync(Guid.NewGuid().ToString()).ConfigureAwait(false);

            // assert
            result.Should().BeNull();
        }
    }
}
