using DFC.App.Help.Controllers;
using DFC.App.Help.Data.Contracts;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System.Net.Mime;

namespace DFC.App.Help.PagesModule.UnitTests.ControllerTests.HealthControllerTests
{
    public class BaseHealthController
    {
        public BaseHealthController()
        {
            FakeHelpPageService = A.Fake<IHelpPageService>();
            FakeLogger = A.Fake<ILogger<HealthController>>();
        }

        protected IHelpPageService FakeHelpPageService { get; }

        protected ILogger<HealthController> FakeLogger { get; }

        protected HealthController BuildHealthController()
        {
            var httpContext = new DefaultHttpContext();

            httpContext.Request.Headers[HeaderNames.Accept] = MediaTypeNames.Application.Json;

            var controller = new HealthController(FakeLogger, FakeHelpPageService)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext,
                },
            };

            return controller;
        }
    }
}
