using DFC.App.ContentPages.Controllers;
using DFC.App.ContentPages.Data;
using DFC.App.ContentPages.Data.Contracts;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.ContentPages.UnitTests.Controllers.Pages
{
    [TestFixture]
    [Category("HelpAppCreate.Tests")]
    public class CreateTests
    {
        private PagesController controller;
        private Mock<IContentPageService> contentPageService;
        private Mock<AutoMapper.IMapper> mapper;

        [SetUp]
        public void SetUp()
        {
            contentPageService = new Mock<IContentPageService>();
            mapper = new Mock<AutoMapper.IMapper>();

            controller = new PagesController(contentPageService.Object, mapper.Object);
        }

        [Test]
        public async Task ShouldReturnCreatedIfItDoesntExists()
        {
            var newHelpModelToCreate = new ContentPageModel
            {
                DocumentId = Guid.NewGuid(),
                CanonicalName = "canonicalname1",
                LastReviewed = DateTime.UtcNow,
                Content = "content1",
            };

            contentPageService.Setup(x => x.GetByIdAsync(newHelpModelToCreate.DocumentId)).ReturnsAsync(default(ContentPageModel));

            var createdHelpModel = new ContentPageModel();
            contentPageService.Setup(x => x.CreateAsync(newHelpModelToCreate)).ReturnsAsync(createdHelpModel);

            var actionResponse = await controller.HelpCreateOrUpdate(newHelpModelToCreate).ConfigureAwait(false);

            var typedResponse = actionResponse as CreatedAtActionResult;
            typedResponse.ActionName.Should().Be("Document");
            typedResponse.ControllerName.Should().Be("Pages");
            typedResponse.StatusCode.Should().Be((int)HttpStatusCode.Created);
        }

        [Test]
        public async Task ShouldReturnOkIfItExists()
        {
            var helpModelToUpdate = new ContentPageModel
            {
                DocumentId = Guid.NewGuid(),
                CanonicalName = "canonicalname1",
                LastReviewed = DateTime.UtcNow,
                Content = "content1",
            };

            contentPageService.Setup(x => x.GetByIdAsync(helpModelToUpdate.DocumentId)).ReturnsAsync(helpModelToUpdate);

            var replacedHelpModel = new ContentPageModel();
            contentPageService.Setup(x => x.GetByIdAsync(helpModelToUpdate.DocumentId)).ReturnsAsync(replacedHelpModel);

            var actionResponse = await controller.HelpCreateOrUpdate(helpModelToUpdate).ConfigureAwait(false);

            var typedResponse = actionResponse as OkObjectResult;
            typedResponse.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }
    }
}
