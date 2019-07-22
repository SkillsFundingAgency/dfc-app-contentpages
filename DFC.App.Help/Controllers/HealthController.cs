using DFC.App.Help.Data.Contracts;
using DFC.App.Help.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.Help.Controllers
{
    public class HealthController : BaseController
    {
        private readonly ILogger<HealthController> logger;
        private readonly IHelpPageService helpPageService;

        public HealthController(ILogger<HealthController> logger, IHelpPageService helpPageService, AutoMapper.IMapper mapper) : base(mapper)
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
            bool isHealthy = false;

            logger.LogInformation($"{nameof(Health)} has been called");

            try
            {
                isHealthy = await helpPageService.PingAsync().ConfigureAwait(false);

                if (isHealthy)
                {
                    message = $"{ResourceName} is available";

                    logger.LogInformation($"{nameof(Health)} responded with: {message}");
                }
                else
                {
                    message = $"Ping to {ResourceName} has failed";

                    logger.LogError($"{nameof(Health)}: {message}");
                }
            }
            catch (Exception ex)
            {
                message = $"{ResourceName} exception: {ex.Message}";

                logger.LogError(ex, $"{nameof(Health)}: {message}");
            }

            var viewModel = new HealthViewModel()
            {
                HealthItems = new List<HealthItemViewModel>()
                {
                    new HealthItemViewModel()
                    {
                        Service = ResourceName,
                        Message = message,
                    },
                },
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
            logger.LogInformation($"{nameof(Ping)} has been called");

            return Ok();
        }
    }
}
