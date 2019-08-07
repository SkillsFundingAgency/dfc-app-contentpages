using DFC.App.Help.Controllers;
using DFC.App.Help.Data;
using DFC.App.Help.Data.Contracts;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.Help.UnitTests.Controllers.Pages
{
    [TestFixture]
    [Category("HelpAppCreate.Tests")]
    public class CreateTests
    {
        private PagesController controller;
        private Mock<IHelpPageService> helpPageService;
        private Mock<AutoMapper.IMapper> mapper;

        [SetUp]
        public void SetUp()
        {
            helpPageService = new Mock<IHelpPageService>();
            mapper = new Mock<AutoMapper.IMapper>();

            controller = new PagesController(helpPageService.Object, mapper.Object);
        }

        [Test]
        public async Task ShouldReturnCreatedIfItDoesntExists()
        {
            var newHelpModelToCreate = new HelpPageModel
            {
                DocumentId = Guid.NewGuid(),
                CanonicalName = "canonicalname1",
                LastReviewed = DateTime.UtcNow,
                Content = "content1",
            };

            helpPageService.Setup(x => x.GetByIdAsync(newHelpModelToCreate.DocumentId)).ReturnsAsync(default(HelpPageModel));

            var createdHelpModel = new HelpPageModel();
            helpPageService.Setup(x => x.CreateAsync(newHelpModelToCreate)).ReturnsAsync(createdHelpModel);

            var actionResponse = await controller.HelpCreateOrUpdate(newHelpModelToCreate).ConfigureAwait(false);

            var typedResponse = actionResponse as CreatedAtActionResult;
            typedResponse.ActionName.Should().Be("Document");
            typedResponse.ControllerName.Should().Be("Pages");
            typedResponse.StatusCode.Should().Be((int)HttpStatusCode.Created);
        }

        [Test]
        public async Task ShouldReturnOkIfItExists()
        {
            var helpModelToUpdate = new HelpPageModel
            {
                DocumentId = Guid.NewGuid(),
                CanonicalName = "canonicalname1",
                LastReviewed = DateTime.UtcNow,
                Content = "content1",
            };

            helpPageService.Setup(x => x.GetByIdAsync(helpModelToUpdate.DocumentId)).ReturnsAsync(helpModelToUpdate);

            var replacedHelpModel = new HelpPageModel();
            helpPageService.Setup(x => x.GetByIdAsync(helpModelToUpdate.DocumentId)).ReturnsAsync(replacedHelpModel);

            var actionResponse = await controller.HelpCreateOrUpdate(helpModelToUpdate).ConfigureAwait(false);

            var typedResponse = actionResponse as OkObjectResult;
            typedResponse.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }
    }
}
