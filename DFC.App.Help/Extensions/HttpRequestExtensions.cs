using System;
using Microsoft.AspNetCore.Http;

namespace DFC.App.Help.Extensions
{
    public static class HttpRequestExtensions
    {
        public static bool IsDraftRequest(this HttpRequest request)
        {
            return request != null && request.Path.Value.StartsWith("/draft", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
