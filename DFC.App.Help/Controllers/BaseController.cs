using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace DFC.App.Help.Controllers
{
    public abstract class BaseController : Controller
    {
        protected IActionResult NegotiateContentResult(object viewModel)
        {
            if (Request.Headers.Keys.Contains(HeaderNames.Accept))
            {
                var acceptHeader = Request.Headers[HeaderNames.Accept].ToString().ToLower();

                if (acceptHeader == MediaTypeNames.Application.Json)
                {
                    return Ok(viewModel);
                }

                if (acceptHeader != MediaTypeNames.Text.Html && acceptHeader != "*/*")
                {
                    return StatusCode((int)HttpStatusCode.NotAcceptable);
                }
            }

            return View(viewModel);
        }
    }
}