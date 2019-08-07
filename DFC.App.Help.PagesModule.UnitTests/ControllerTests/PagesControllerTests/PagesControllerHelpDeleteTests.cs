using DFC.App.Help.Data;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using Xunit;

namespace DFC.App.Help.PagesModule.UnitTests.ControllerTests.PagesControllerTests
{
    [Trait("Category", "Pages Controller Unit Tests")]
    public class PagesControllerHelpDeleteTests : BasePagesController
    {
        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async void PagesControllerHelpDeleteReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            Guid documentId = Guid.NewGuid();
            var expectedResult = A.Fake<HelpPageModel>();
            var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => FakeHelpPageService.GetByIdAsync(A<Guid>.Ignored)).Returns(expectedResult);

            // Act
            var result = await controller.HelpDelete(documentId).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeHelpPageService.GetByIdAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();

            var okResult = Assert.IsType<OkResult>(result);

            A.Equals((int)HttpStatusCode.OK, okResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async void PagesControllerHelpDeleteReturnsNotFound(string mediaTypeName)
        {
            // Arrange
            Guid documentId = Guid.NewGuid();
            HelpPageModel expectedResult = null;
            var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => FakeHelpPageService.GetByIdAsync(A<Guid>.Ignored)).Returns(expectedResult);

            // Act
            var result = await controller.HelpDelete(documentId).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeHelpPageService.GetByIdAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();

            var statusResult = Assert.IsType<NotFoundResult>(result);

            A.Equals((int)HttpStatusCode.NotFound, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}
