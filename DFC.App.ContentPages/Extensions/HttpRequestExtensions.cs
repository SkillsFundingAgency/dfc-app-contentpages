﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DFC.App.ContentPages.Extensions
{
    public static class HttpRequestExtensions
    {
        public static Uri GetBaseAddress(this HttpRequest request, IUrlHelper urlHelper = null)
        {
            if (request != null)
            {
                if (request.Headers.TryGetValue("x-forwarded-proto", out var forwardedProtocol)
                    && request.Headers.TryGetValue("x-original-host", out var originalHost))
                {
                    return new Uri($"{forwardedProtocol}://{originalHost}");
                }

                return string.IsNullOrWhiteSpace(request.Scheme) ? null : new Uri($"{request.Scheme}://{request.Host}{urlHelper?.Content("~")}");
            }

            return null;
        }
    }
}