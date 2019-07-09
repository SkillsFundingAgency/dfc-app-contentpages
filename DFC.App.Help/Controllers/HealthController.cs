using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using DFC.App.Help.Data.Contracts;
using DFC.App.Help.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DFC.App.Help.Controllers
{
    public class HealthController : BaseController
    {
        private readonly ILogger<HealthController> _logger;
        private readonly IHelpPageService _helpPageService;

        public HealthController(ILogger<HealthController> logger, IHelpPageService helpPageService, AutoMapper.IMapper mapper) : base(mapper)
        {
            _logger = logger;
            _helpPageService = helpPageService;
        }

        [HttpGet]
        [Route("pages/contents/health")]
        public async Task<IActionResult> Health()
        {
            const string ResourceName = "Document store";
            string message;
            bool isHealthy = false;

            _logger.LogInformation($"{nameof(Health)} has been called");

            try
            {
                isHealthy = await _helpPageService.PingAsync();

                if (isHealthy)
                {
                    message = $"{ResourceName} is available";

                    _logger.LogInformation($"{nameof(Health)} responded with: {message}");
                }
                else
                {
                    message = $"Ping to {ResourceName} has failed";

                    _logger.LogError($"{nameof(Health)}: {message}");
                }
            }
            catch (Exception ex)
            {
                message = $"{ResourceName} exception: {ex.Message}";

                _logger.LogError(ex, $"{nameof(Health)}: {message}");
            }

            var viewModel = new HealthViewModel()
            {
                HealthItems = new List<HealthItemViewModel>()
                {
                    new HealthItemViewModel()
                    {
                        Service = ResourceName,
                        Message = message
                    }
                }
            };

            if (isHealthy)
            {
                return NegotiateContentResult(viewModel);
            }

            return StatusCode((int)HttpStatusCode.ServiceUnavailable);
        }

        [HttpGet]
        [Route("health/ping")]
        public IActionResult Ping()
        {
            _logger.LogInformation($"{nameof(Ping)} has been called");

            return Ok();
        }
    }
}
