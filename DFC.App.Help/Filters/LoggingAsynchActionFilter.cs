using System;
using System.Threading.Tasks;
using DFC.App.Help.Framework;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace DFC.App.Help.Filters
{
    public class LoggingAsynchActionFilter : IAsyncActionFilter
    {
        private readonly ICorrelationIdProvider _correlationIdProvider;
        private readonly ILogger<LoggingAsynchActionFilter> _logger;

        public LoggingAsynchActionFilter(ICorrelationIdProvider correlationIdProvider, ILogger<LoggingAsynchActionFilter> logger)
        {
            _correlationIdProvider = correlationIdProvider;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var correlationId = _correlationIdProvider.Get();

            _logger.LogInformation($"CorrelationId:{correlationId} Executing {context.ActionDescriptor.DisplayName}");
            var executed = await next();
            if (executed.Exception != null)
            {
                _logger.LogError(executed.Exception, $"CorrelationId:{correlationId} Executed with error {executed.Exception}");
            }
            _logger.LogInformation($"CorrelationId:{correlationId} Executed successfully {context.ActionDescriptor.DisplayName}");
        }
    }

}

