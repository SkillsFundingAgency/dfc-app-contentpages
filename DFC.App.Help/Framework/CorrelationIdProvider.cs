using Microsoft.AspNetCore.Http;
using System.Linq;

namespace DFC.App.Help.Framework
{
    public class CorrelationIdProvider : ICorrelationIdProvider
    {
        private const string CorrelationId = "DssCorrelationId";

        private readonly IHttpContextAccessor httpContextAccessor;

        public CorrelationIdProvider(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string Get()
        {
            var result = string.Empty;
            if (httpContextAccessor.HttpContext != null)
            {
                result = httpContextAccessor.HttpContext.Request.Headers[CorrelationId].FirstOrDefault();
            }

            return result;
        }
    }
}
