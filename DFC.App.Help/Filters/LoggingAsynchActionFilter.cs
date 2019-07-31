using DFC.App.Help.Framework;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DFC.App.Help.Filters
{
    public class LoggingAsynchActionFilter : IAsyncActionFilter
    {
        private readonly ICorrelationIdProvider correlationIdProvider;
        private readonly ILogger<LoggingAsynchActionFilter> logger;

        public LoggingAsynchActionFilter(ICorrelationIdProvider correlationIdProvider, ILogger<LoggingAsynchActionFilter> logger)
        {
            this.correlationIdProvider = correlationIdProvider;
            this.logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            var correlationId = correlationIdProvider.GetId();

            logger.LogInformation($"CorrelationId:{correlationId} Executing {context.ActionDescriptor.DisplayName}");

            var executed = await next().ConfigureAwait(false);

            if (executed.Exception != null)
            {
                logger.LogError(executed.Exception, $"CorrelationId:{correlationId} Executed with error {executed.Exception}");
            }

            logger.LogInformation($"CorrelationId:{correlationId} Executed successfully {context.ActionDescriptor.DisplayName}");
        }
    }
}