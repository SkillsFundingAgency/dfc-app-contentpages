using DFC.App.Help.Controllers;
using DFC.App.Help.Models.Cosmos;
using DFC.App.Help.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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
        public async Task ShouldReturn_Created_IfItDoesntExists()
        {
            var newHelpModelToCreate = new HelpPageModel();
            newHelpModelToCreate.DocumentId = Guid.NewGuid();
            newHelpModelToCreate.CanonicalName = "canonicalname1";
            newHelpModelToCreate.Content = "content1";

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
        public async Task ShouldReturn_OK_IfItExists()
        {
            var helpModelToUpdate = new HelpPageModel();
            helpModelToUpdate.DocumentId = Guid.NewGuid();
            helpModelToUpdate.CanonicalName = "canonicalname1";
            helpModelToUpdate.Content = "content1";

            _helpPageService.Setup(x => x.GetByIdAsync(helpModelToUpdate.DocumentId)).ReturnsAsync(helpModelToUpdate);

            var replacedHelpModel = new HelpPageModel();
            _helpPageService.Setup(x => x.GetByIdAsync(helpModelToUpdate.DocumentId)).ReturnsAsync(replacedHelpModel);

            var actionResponse = await _controller.HelpCreateOrUpdate(helpModelToUpdate);

            var typedResponse = actionResponse as OkObjectResult;
            typedResponse.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }
    }
}
