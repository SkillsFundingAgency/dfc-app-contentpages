using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Xunit;

namespace DFC.App.ContentPages.PagesModule.UnitTests.ControllerTests.PagesControllerTests
{
    [Trait("Category", "Pages Controller Unit Tests")]
    public class PagesControllerBodyFooterTests : BasePagesController
    {
        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public void PagesControllerBodyFooterJsonReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            const string category = "a-category";
            const string article = "an-article-name";
            var controller = BuildPagesController(mediaTypeName);

            // Act
            var result = controller.BodyFooter(category, article);

            // Assert
            var statusResult = Assert.IsType<NoContentResult>(result);

            A.Equals((int)HttpStatusCode.NoContent, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}
