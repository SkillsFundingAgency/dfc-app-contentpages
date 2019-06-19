using System.Collections.Generic;
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
                    Name = name + "_1"
                },
                new HelpPageModel()
                {
                    Name = name + "_2"
                }
            };
            var helpPageService = _serviceProvider.GetService<Services.IHelpPageService>();

            helpPageModels.ForEach(async f => _ = await helpPageService.CreateAsync(f));

            // act
            var results = await helpPageService.GetListAsync();

            // assert
            results.Should().NotBeNull();
            results.Count.Should().BeGreaterOrEqualTo(helpPageModels.Count);
            results[0].DocumentId.Should().NotBeNull();
            results[0].Name.Should().NotBeNull();
            results[1].DocumentId.Should().NotBeNull();
            results[1].Name.Should().NotBeNull();
        }
    }
}
