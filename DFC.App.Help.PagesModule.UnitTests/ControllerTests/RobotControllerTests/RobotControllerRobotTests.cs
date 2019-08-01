using DFC.App.Help.ViewModels;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Net.Mime;
using Xunit;

namespace DFC.App.Help.PagesModule.UnitTests.ControllerTests.RobotControllerTests
{
    public class RobotControllerRobotTests : BaseRobotController
    {
        [Fact]
        public  void RobotControllerRobotReturnsSuccess()
        {
            // Arrange
            var controller = BuildRobotController();

            // Act
            var result =  controller.Robot();

            // Assert
            var contentResult = Assert.IsType<ContentResult>(result);

            contentResult.ContentType.Should().Be(MediaTypeNames.Text.Plain);

            controller.Dispose();
        }
    }
}
