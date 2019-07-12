using System.Linq;
using Microsoft.AspNetCore.Http;

namespace DFC.App.Help.Framework
{
    public class CorrelationIdProvider : ICorrelationIdProvider
    {
        private const string CorrelationId = "DssCorrelationId";

        private readonly IHttpContextAccessor _httpContextAccessor;

        public CorrelationIdProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Get()
        {
            var result = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                result = _httpContextAccessor.HttpContext.Request.Headers[CorrelationId].FirstOrDefault();
            }
            return result;
        }
    }
}
