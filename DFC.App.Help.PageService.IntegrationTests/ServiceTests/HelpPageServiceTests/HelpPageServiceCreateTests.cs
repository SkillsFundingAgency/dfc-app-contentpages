using DFC.App.Help.Data;
using DFC.App.Help.Data.Contracts;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace DFC.App.Help.PageService.IntegrationTests.ServiceTests.HelpPageServiceTests
{
    [TestFixture]
    public class HelpPageServiceCreateTests : BaseHelpPageServiceTests
    {
        [Test]
        [Category("HelpPageService.Create")]
        public async Task HelpPageServiceCreateReturnsSuccessWhenHelpPageCreated()
        {
            // arrange
            const string name = ValidNameValue + "_Create";
            var helpPageModel = new HelpPageModel()
            {
                CanonicalName = name + "_" + Guid.NewGuid().ToString(),
                DocumentId = Guid.NewGuid(),
                LastReviewed = DateTime.UtcNow,
            };
            var helpPageService = ServiceProvider.GetService<IHelpPageService>();

            // act
            await helpPageService.CreateAsync(helpPageModel).ConfigureAwait(false);

            var result = await helpPageService.GetByIdAsync(helpPageModel.DocumentId).ConfigureAwait(false);

            // assert
            result.Should().NotBeNull();
            result.DocumentId.Should().Be(helpPageModel.DocumentId);
            result.CanonicalName.Should().Be(helpPageModel.CanonicalName);
        }

        [Test]
        [Category("HelpPageService.GetByAlternativeName")]
        public void HelpPageServiceCreateReturnsArgumentNullExceptionWhenNullIsUsed()
        {
            // arrange
            var helpPageService = ServiceProvider.GetService<IHelpPageService>();

            // act
            var exceptionResult = Assert.ThrowsAsync<ArgumentNullException>(async () => await helpPageService.CreateAsync(null).ConfigureAwait(false));

            //Assert
            Assert.AreEqual("Value cannot be null.\r\nParameter name: helpPageModel", exceptionResult.Message);
        }
    }
}
