using DFC.App.Help.Controllers;
using DFC.App.Help.Data.Contracts;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.Net.Mime;

namespace DFC.App.Help.PagesModule.UnitTests.ControllerTests.PagesControllerTests
{
    public class BasePagesController
    {
        public BasePagesController()
        {
            FakeHelpPageService = A.Fake<IHelpPageService>();
            FakeMapper = A.Fake<AutoMapper.IMapper>();
        }

        public static IEnumerable<object[]> HtmlMediaTypes => new List<object[]>
        {
            new string[] { "*/*" },
            new string[] { MediaTypeNames.Text.Html },
        };

        public static IEnumerable<object[]> InvalidMediaTypes => new List<object[]>
        {
            new string[] { MediaTypeNames.Text.Plain },
        };

        public static IEnumerable<object[]> JsonMediaTypes => new List<object[]>
        {
            new string[] { MediaTypeNames.Application.Json },
        };

        protected IHelpPageService FakeHelpPageService { get; }

        protected AutoMapper.IMapper FakeMapper { get; }

        protected PagesController BuildPagesController(string mediaTypeName)
        {
            var httpContext = new DefaultHttpContext();

            httpContext.Request.Headers[HeaderNames.Accept] = mediaTypeName;

            var controller = new PagesController(FakeHelpPageService, FakeMapper)
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
