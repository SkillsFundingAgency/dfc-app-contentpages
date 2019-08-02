using DFC.App.Help.Data;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using Xunit;

namespace DFC.App.Help.PagesModule.UnitTests.ControllerTests.PagesControllerTests
{
    public class PagesControllerHelpCreateOrUpdateTests : BasePagesController
    {
        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async void PagesControllerHelpCreateOrUpdateReturnsSuccessForCreate(string mediaTypeName)
        {
            // Arrange
            var helpPageModel = A.Fake<HelpPageModel>();
            HelpPageModel existingHelpPageModel = null;
            var createdHelpPageModel = A.Fake<HelpPageModel>();
            var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => FakeHelpPageService.GetByIdAsync(A<Guid>.Ignored)).Returns(existingHelpPageModel);
            A.CallTo(() => FakeHelpPageService.CreateAsync(A<HelpPageModel>.Ignored)).Returns(createdHelpPageModel);

            // Act
            var result = await controller.HelpCreateOrUpdate(helpPageModel).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeHelpPageService.GetByIdAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeHelpPageService.CreateAsync(A<HelpPageModel>.Ignored)).MustHaveHappenedOnceExactly();

            var okResult = Assert.IsType<CreatedAtActionResult>(result);

            A.Equals((int)HttpStatusCode.Created, okResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async void PagesControllerHelpCreateOrUpdateReturnsSuccessForUpdate(string mediaTypeName)
        {
            // Arrange
            var helpPageModel = A.Fake<HelpPageModel>();
            var existingHelpPageModel = A.Fake<HelpPageModel>();
            HelpPageModel updatedHelpPageModel = null;
            var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => FakeHelpPageService.GetByIdAsync(A<Guid>.Ignored)).Returns(existingHelpPageModel);
            A.CallTo(() => FakeHelpPageService.ReplaceAsync(A<HelpPageModel>.Ignored)).Returns(updatedHelpPageModel);

            // Act
            var result = await controller.HelpCreateOrUpdate(helpPageModel).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeHelpPageService.GetByIdAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeHelpPageService.ReplaceAsync(A<HelpPageModel>.Ignored)).MustHaveHappenedOnceExactly();

            var okResult = Assert.IsType<OkObjectResult>(result);

            A.Equals((int)HttpStatusCode.OK, okResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async void PagesControllerHelpCreateOrUpdateReturnsBadResultWhenModelIsNull(string mediaTypeName)
        {
            // Arrange
            HelpPageModel helpPageModel = null;
            var controller = BuildPagesController(mediaTypeName);

            // Act
            var result = await controller.HelpCreateOrUpdate(helpPageModel).ConfigureAwait(false);

            // Assert
            var statusResult = Assert.IsType<BadRequestResult>(result);

            A.Equals((int)HttpStatusCode.BadRequest, statusResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async void PagesControllerHelpCreateOrUpdateReturnsBadResultWhenModelIsInvalid(string mediaTypeName)
        {
            // Arrange
            var helpPageModel = new HelpPageModel();
            var controller = BuildPagesController(mediaTypeName);

            controller.ModelState.AddModelError(string.Empty, "Model is not valid");

            // Act
            var result = await controller.HelpCreateOrUpdate(helpPageModel).ConfigureAwait(false);

            // Assert
            var statusResult = Assert.IsType<BadRequestObjectResult>(result);

            A.Equals((int)HttpStatusCode.BadRequest, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}
