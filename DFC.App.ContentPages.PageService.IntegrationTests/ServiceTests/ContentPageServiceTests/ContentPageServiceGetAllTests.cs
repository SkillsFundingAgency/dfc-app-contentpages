using DFC.App.ContentPages.Data;
using DFC.App.ContentPages.Data.Contracts;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.App.ContentPages.PageService.IntegrationTests.ServiceTests.ContentPageServiceTests
{
    [TestFixture]
    public class ContentPageServiceGetAllTests : BaseContentPageServiceTests
    {
        [Test]
        [Category("ContentPageService.GetAllList")]
        public async Task ContentPageServiceGetAllListReturnsSuccessWhenContentPagesExist()
        {
            // arrange
            const string name = ValidNameValue + "_GetList";
            var contentPageModels = new List<ContentPageModel>()
            {
                new ContentPageModel()
                {
                    CanonicalName = name + "_1",
                    DocumentId = Guid.NewGuid(),
                    LastReviewed = DateTime.UtcNow,
                },
                new ContentPageModel()
                {
                    CanonicalName = name + "_2",
                    DocumentId = Guid.NewGuid(),
                    LastReviewed = DateTime.UtcNow,
                },
            };
            var contentPageService = ServiceProvider.GetService<IContentPageService>();

            contentPageModels.ForEach(async f => _ = await contentPageService.CreateAsync(f).ConfigureAwait(false));

            // act
            var results = await contentPageService.GetAllAsync().ConfigureAwait(false);
            var resultsList = results.ToList();

            // assert
            resultsList.Should().NotBeNull();
            resultsList.Count.Should().BeGreaterOrEqualTo(contentPageModels.Count);
            resultsList.Should().Contain(x => contentPageModels.Any(y => y.DocumentId == x.DocumentId));
            resultsList[0].CanonicalName.Should().NotBeNull();
            resultsList[1].CanonicalName.Should().NotBeNull();
        }
    }
}
