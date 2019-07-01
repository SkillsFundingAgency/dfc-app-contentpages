using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DFC.App.Help.Models.Cosmos;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DFC.App.Help.IntegrationTests.ServiceTests.HelpPageServiceTests
{
    [TestFixture]
    public class HelpPageServiceGetAllTests : BaseHelpPageServiceTests
    {
        [Test]
        [Category("HelpPageService.GetAllList")]
        public async Task HelpPageService_GetAllList_ReturnsSuccess_WhenHelpPagesExist()
        {
            // arrange
            const string name = ValidNameValue + "_GetList";
            var helpPageModels = new List<HelpPageModel>() {
                new HelpPageModel() {
                    CanonicalName = name + "_1",
                    DocumentId=Guid.NewGuid()
                },
                new HelpPageModel()
                {
                    CanonicalName = name + "_2",
                    DocumentId=Guid.NewGuid()
                }
            };
            var helpPageService = _serviceProvider.GetService<Services.IHelpPageService>();

            helpPageModels.ForEach(async f => _ = await helpPageService.CreateAsync(f));

            // act
            var results = await helpPageService.GetListAsync();

            // assert
            results.Should().NotBeNull();
            results.Count.Should().BeGreaterOrEqualTo(helpPageModels.Count);
            results.Should().Contain(x => helpPageModels.Any(y => y.DocumentId == x.DocumentId));
            results[0].CanonicalName.Should().NotBeNull();
            results[1].CanonicalName.Should().NotBeNull();
        }
    }
}
