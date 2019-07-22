using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Linq;
using System.Net;
using System.Net.Mime;

namespace DFC.App.Help.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly AutoMapper.IMapper mapper;

        public BaseController(AutoMapper.IMapper mapper)
        {
            this.mapper = mapper;
        }

        protected IActionResult NegotiateContentResult(object viewModel, object dataModel = null)
        {
            if (Request.Headers.Keys.Contains(HeaderNames.Accept))
            {
                var acceptHeaders = Request.Headers[HeaderNames.Accept].ToString().ToLower().Split(';');

                foreach (var acceptHeader in acceptHeaders)
                {
                    var items = acceptHeader.Split(',');

                    if (items.Contains(MediaTypeNames.Application.Json))
                    {
                        return Ok(dataModel ?? viewModel);
                    }

                    if (items.Contains(MediaTypeNames.Text.Html) || items.Contains("*/*"))
                    {
                        return View(viewModel);
                    }
                }
            }

            return StatusCode((int)HttpStatusCode.NotAcceptable);
        }
    }
}