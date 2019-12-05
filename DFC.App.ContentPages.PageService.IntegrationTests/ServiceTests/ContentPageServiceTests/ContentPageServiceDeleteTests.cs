using DFC.App.ContentPages.Data;
using DFC.App.ContentPages.Data.Contracts;
using FluentAssertions;
using Microsoft.Azure.Documents;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace DFC.App.ContentPages.PageService.IntegrationTests.ServiceTests.ContentPageServiceTests
{
    [TestFixture]
    public class ContentPageServiceDeleteTests : BaseContentPageServiceTests
    {
        [Test]
        [Category("ContentPageService.Delete")]
        public async Task ContentPageServiceDeleteReturnsSuccessWhenContentPageDeleted()
        {
            // arrange
            const string name = ValidNameValue + "_Delete";
            var contentPageModel = new ContentPageModel()
            {
                CanonicalName = name + "_" + Guid.NewGuid().ToString(),
            };
            var contentPageService = ServiceProvider.GetService<IContentPageService>();

            var createdcontentPageModel = await contentPageService.CreateAsync(contentPageModel).ConfigureAwait(false);

            // act
            var result = await contentPageService.DeleteAsync(createdcontentPageModel.DocumentId).ConfigureAwait(false);

            // assert
            result.Should().BeTrue();
        }

        [Test]
        [Category("ContentPageService.Delete")]
        public void ContentPageServiDeleteReturnsExceptionWhenContentPageDoeNotExist()
        {
            // arrange
            var contentPageService = ServiceProvider.GetService<IContentPageService>();

            // act
            Assert.ThrowsAsync<DocumentClientException>(() => contentPageService.DeleteAsync(Guid.NewGuid()));

            // assert
        }
    }
}
