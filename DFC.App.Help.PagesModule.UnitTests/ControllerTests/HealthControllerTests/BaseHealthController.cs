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
        protected readonly IHelpPageService fakeHelpPageService;
        protected readonly ILogger<HealthController> fakeLogger;

        public BaseHealthController()
        {
            fakeHelpPageService = A.Fake<IHelpPageService>();
            fakeLogger = A.Fake<ILogger<HealthController>>();
        }

        protected HealthController BuildHealthController()
        {
            var httpContext = new DefaultHttpContext();

            httpContext.Request.Headers[HeaderNames.Accept] = MediaTypeNames.Application.Json;

            var controller = new HealthController(fakeLogger, fakeHelpPageService)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext,
                }
            };

            return controller;
        }
    }
}
