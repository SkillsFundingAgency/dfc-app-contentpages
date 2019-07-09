using System.Linq;
using DFC.App.Help.Data.Common;
using Microsoft.AspNetCore.Http;

namespace DFC.App.Help.Framework
{
    public class CorrelationIdProvider : ICorrelationIdProvider
    {
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
                result = _httpContextAccessor.HttpContext.Request.Headers[Constants.CorrelationId].FirstOrDefault();
            }
            return result;
        }
    }
}
