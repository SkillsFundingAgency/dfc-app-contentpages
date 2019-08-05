using DFC.App.Help.Controllers;
using DFC.App.Help.Data.Contracts;
using FakeItEasy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System.Net.Mime;

namespace DFC.App.Help.PagesModule.UnitTests.ControllerTests.RobotControllerTests
{
    public class BaseRobotController
    {
        protected readonly ILogger<RobotController> fakeLogger;
        protected readonly IHostingEnvironment fakeHostingEnvironment;

        public BaseRobotController()
        {
            fakeLogger = A.Fake<ILogger<RobotController>>();
            fakeHostingEnvironment = A.Fake<IHostingEnvironment>();
        }

        protected RobotController BuildRobotController()
        {
            var controller = new RobotController(fakeLogger, fakeHostingEnvironment)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext(),
                }
            };

            return controller;
        }
    }
}
