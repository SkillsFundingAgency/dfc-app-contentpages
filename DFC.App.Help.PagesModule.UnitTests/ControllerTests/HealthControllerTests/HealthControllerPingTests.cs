using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Xunit;

namespace DFC.App.Help.PagesModule.UnitTests.ControllerTests.HealthControllerTests
{
    public class HealthControllerPingTests : BaseHealthController
    {
        [Fact]
        public void HealthControllerPingReturnsSuccess()
        {
            // Arrange
            var controller = BuildHealthController();

            // Act
            var result = controller.Ping();

            // Assert
            var statusResult = Assert.IsType<OkResult>(result);

            A.Equals((int)HttpStatusCode.OK, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}
