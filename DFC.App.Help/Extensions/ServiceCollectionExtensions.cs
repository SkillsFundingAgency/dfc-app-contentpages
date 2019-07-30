using DFC.App.Help.Policies.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Net.Http;
using System.Net.Mime;

namespace DFC.App.Help.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHttpClient<TClient, TImplementation, TClientOptions>(
            this IServiceCollection services,
            IConfiguration configuration,
            string configurationSectionName)
            where TClient : class
            where TImplementation : class, TClient
            where TClientOptions : HttpClientOptions, new() =>
            services
                .Configure<TClientOptions>(configuration.GetSection(configurationSectionName))
                .AddHttpClient<TClient, TImplementation>()
                .ConfigureHttpClient((sp, options) =>
                {
                    var httpClientOptions = sp
                        .GetRequiredService<IOptions<TClientOptions>>()
                        .Value;
                    options.BaseAddress = httpClientOptions.BaseAddress;
                    options.Timeout = httpClientOptions.Timeout;
                    options.DefaultRequestHeaders.Add(HeaderNames.Accept, MediaTypeNames.Text.Html);
                })
                .ConfigurePrimaryHttpMessageHandler(() =>
                {
                    return new HttpClientHandler()
                    {
                        AllowAutoRedirect = false,
                    };
                })
                .Services;
    }
}