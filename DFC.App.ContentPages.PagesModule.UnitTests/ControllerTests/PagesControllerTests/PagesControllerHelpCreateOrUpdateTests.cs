using DFC.App.ContentPages.Data;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using Xunit;

namespace DFC.App.ContentPages.PagesModule.UnitTests.ControllerTests.PagesControllerTests
{
    [Trait("Category", "Pages Controller Unit Tests")]
    public class PagesControllerHelpCreateOrUpdateTests : BasePagesController
    {
        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async void PagesControllerHelpCreateOrUpdateReturnsSuccessForCreate(string mediaTypeName)
        {
            // Arrange
            var contentPageModel = A.Fake<ContentPageModel>();
            ContentPageModel existingcontentPageModel = null;
            var createdcontentPageModel = A.Fake<ContentPageModel>();
            var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => FakeContentPageService.GetByIdAsync(A<Guid>.Ignored)).Returns(existingcontentPageModel);
            A.CallTo(() => FakeContentPageService.CreateAsync(A<ContentPageModel>.Ignored)).Returns(createdcontentPageModel);

            // Act
            var result = await controller.HelpCreateOrUpdate(contentPageModel).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeContentPageService.GetByIdAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeContentPageService.CreateAsync(A<ContentPageModel>.Ignored)).MustHaveHappenedOnceExactly();

            var okResult = Assert.IsType<CreatedAtActionResult>(result);

            A.Equals((int)HttpStatusCode.Created, okResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async void PagesControllerHelpCreateOrUpdateReturnsSuccessForUpdate(string mediaTypeName)
        {
            // Arrange
            var contentPageModel = A.Fake<ContentPageModel>();
            var existingcontentPageModel = A.Fake<ContentPageModel>();
            ContentPageModel updatedcontentPageModel = null;
            var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => FakeContentPageService.GetByIdAsync(A<Guid>.Ignored)).Returns(existingcontentPageModel);
            A.CallTo(() => FakeContentPageService.ReplaceAsync(A<ContentPageModel>.Ignored)).Returns(updatedcontentPageModel);

            // Act
            var result = await controller.HelpCreateOrUpdate(contentPageModel).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeContentPageService.GetByIdAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeContentPageService.ReplaceAsync(A<ContentPageModel>.Ignored)).MustHaveHappenedOnceExactly();

            var okResult = Assert.IsType<OkObjectResult>(result);

            A.Equals((int)HttpStatusCode.OK, okResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async void PagesControllerHelpCreateOrUpdateReturnsBadResultWhenModelIsNull(string mediaTypeName)
        {
            // Arrange
            ContentPageModel contentPageModel = null;
            var controller = BuildPagesController(mediaTypeName);

            // Act
            var result = await controller.HelpCreateOrUpdate(contentPageModel).ConfigureAwait(false);

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
            var contentPageModel = new ContentPageModel();
            var controller = BuildPagesController(mediaTypeName);

            controller.ModelState.AddModelError(string.Empty, "Model is not valid");

            // Act
            var result = await controller.HelpCreateOrUpdate(contentPageModel).ConfigureAwait(false);

            // Assert
            var statusResult = Assert.IsType<BadRequestObjectResult>(result);

            A.Equals((int)HttpStatusCode.BadRequest, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}
