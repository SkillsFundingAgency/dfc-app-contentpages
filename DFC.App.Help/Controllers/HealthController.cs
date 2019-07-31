using DFC.App.Help.Data.Contracts;
using DFC.App.Help.Extensions;
using DFC.App.Help.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.Help.Controllers
{
    public class HealthController : Controller
    {
        private readonly ILogger<HealthController> logger;
        private readonly IHelpPageService helpPageService;

        public HealthController(ILogger<HealthController> logger, IHelpPageService helpPageService)
        {
            this.logger = logger;
            this.helpPageService = helpPageService;
        }

        [HttpGet]
        [Route("pages/contents/health")]
        public async Task<IActionResult> Health()
        {
            const string ResourceName = "Document store";
            string message;

            logger.LogInformation($"{nameof(Health)} has been called");

            try
            {
                var isHealthy = await helpPageService.PingAsync().ConfigureAwait(false);

                if (isHealthy)
                {
                    message = $"{ResourceName} is available";
                    logger.LogInformation($"{nameof(Health)} responded with: {message}");

                    var viewModel = CreateHealthViewModel(ResourceName, message);
                    return this.NegotiateContentResult(viewModel);
                }

                message = $"Ping to {ResourceName} has failed";
                logger.LogError($"{nameof(Health)}: {message}");
            }
            catch (Exception ex)
            {
                message = $"{ResourceName} exception: {ex.Message}";
                logger.LogError(ex, $"{nameof(Health)}: {message}");
            }

            return StatusCode((int)HttpStatusCode.ServiceUnavailable);
        }

        [HttpGet]
        [Route("health/ping")]
        public IActionResult Ping()
        {
            logger.LogInformation($"{nameof(Ping)} has been called");

            return Ok();
        }

        private static HealthViewModel CreateHealthViewModel(string resourceName, string message)
        {
            return new HealthViewModel
            {
                HealthItems = new List<HealthItemViewModel>
                {
                    new HealthItemViewModel
                    {
                        Service = resourceName,
                        Message = message,
                    },
                },
            };
        }
    }
}