using System;
using System.Net;
using System.Threading.Tasks;
using DFC.App.Help.Controllers;
using DFC.App.Help.Data;
using DFC.App.Help.Data.Contracts;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace DFC.App.Help.UnitTests.Controllers.Pages
{
    [TestFixture]
    public class CreateTests
    {
        private PagesController _controller;
        private Mock<IHelpPageService> _helpPageService;

        [SetUp]
        public void SetUp()
        {
            _helpPageService = new Mock<IHelpPageService>();

            _controller = new PagesController(_helpPageService.Object);
        }

        [Test]
        public async Task ShouldReturnCreatedIfItDoesntExists()
        {
            var newHelpModelToCreate = new HelpPageModel
            {
                DocumentId = Guid.NewGuid(),
                CanonicalName = "canonicalname1",
                Content = "content1"
            };

            _helpPageService.Setup(x => x.GetByIdAsync(newHelpModelToCreate.DocumentId)).ReturnsAsync(default(HelpPageModel));

            var createdHelpModel = new HelpPageModel();
            _helpPageService.Setup(x => x.CreateAsync(newHelpModelToCreate)).ReturnsAsync(createdHelpModel);

            var actionResponse = await _controller.HelpCreateOrUpdate(newHelpModelToCreate);

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
                Content = "content1"
            };

            _helpPageService.Setup(x => x.GetByIdAsync(helpModelToUpdate.DocumentId)).ReturnsAsync(helpModelToUpdate);

            var replacedHelpModel = new HelpPageModel();
            _helpPageService.Setup(x => x.GetByIdAsync(helpModelToUpdate.DocumentId)).ReturnsAsync(replacedHelpModel);

            var actionResponse = await _controller.HelpCreateOrUpdate(helpModelToUpdate);

            var typedResponse = actionResponse as OkObjectResult;
            typedResponse.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }
    }
}
