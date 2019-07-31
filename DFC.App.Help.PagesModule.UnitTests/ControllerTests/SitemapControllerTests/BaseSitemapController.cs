using DFC.App.Help.Controllers;
using DFC.App.Help.Data.Contracts;
using FakeItEasy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System.Net.Mime;

namespace DFC.App.Help.PagesModule.UnitTests.ControllerTests.SitemapControllerTests
{
    public class BaseSitemapController
    {
        protected readonly ILogger<SitemapController> fakeLogger;
        protected readonly IHelpPageService fakeHelpPageService;

        public BaseSitemapController()
        {
            fakeLogger = A.Fake<ILogger<SitemapController>>();
            fakeHelpPageService = A.Fake<IHelpPageService>();
        }

        protected SitemapController BuildSitemapController()
        {
            var controller = new SitemapController(fakeLogger, fakeHelpPageService)
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
