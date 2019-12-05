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
    public class ContentPageServiceGetByIdTests : BaseContentPageServiceTests
    {
        [Test]
        [Category("ContentPageService.GetById")]
        public async Task ContentPageServiceGetByIdReturnsSuccessWhenContentPageExists()
        {
            // arrange
            const string name = ValidNameValue + "_GetById";
            var contentPageModel = new ContentPageModel()
            {
                CanonicalName = name + "_" + Guid.NewGuid().ToString(),
                DocumentId = Guid.NewGuid(),
                LastReviewed = DateTime.UtcNow,
            };
            var contentPageService = ServiceProvider.GetService<IContentPageService>();

            await contentPageService.CreateAsync(contentPageModel).ConfigureAwait(false);

            // act
            var result = await contentPageService.GetByIdAsync(contentPageModel.DocumentId).ConfigureAwait(false);

            // assert
            result.DocumentId.Should().Be(contentPageModel.DocumentId);
            result.Should().NotBeNull();
            result.CanonicalName.Should().Be(contentPageModel.CanonicalName);
        }

        [Test]
        [Category("ContentPageService.GetById")]
        public async Task ContentPageServiceGetByIdReturnsNullWhenContentPageDoesNotExist()
        {
            // arrange
            var contentPageService = ServiceProvider.GetService<IContentPageService>();

            // act
            var result = await contentPageService.GetByIdAsync(Guid.NewGuid()).ConfigureAwait(false);

            // assert
            result.Should().BeNull();
        }
    }
}
