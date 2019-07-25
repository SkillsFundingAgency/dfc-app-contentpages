using DFC.App.Help.Data;
using DFC.App.Help.Data.Contracts;
using DFC.App.Help.Repository.SitefinityApi.DataContext;
using DFC.App.Help.Repository.SitefinityApi.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace DFC.App.DraftHelp.PageService
{
    public class DraftHelpPageService : IDraftHelpPageService
    {
        private readonly ISitefinityODataContext sitefinityODataContext;
        private readonly ITokenService tokenService;

        private readonly SitefinityAPIConnectionSettings settings;
        private readonly ILogger<DraftHelpPageService> logger;

        public DraftHelpPageService(ISitefinityODataContext sitefinityODataContext, ITokenService tokenService, ILogger<DraftHelpPageService> logger, SitefinityAPIConnectionSettings settings)
        {
            this.sitefinityODataContext = sitefinityODataContext;
            this.tokenService = tokenService;
            this.logger = logger;
            this.settings = settings;
        }

        public async Task<HelpPageModel> GetSitefinityData(string canonicalName)
        {
            var requestUri = new Uri($"{settings.SitefinityApiUrlBase}/{settings.SitefinityApiDataEndpoint}/{canonicalName}");

            try
            {
                return await GetData(requestUri).ConfigureAwait(false);
            }
            catch (UnauthorizedAccessException)
            {
                logger.LogInformation($"Access denied, access token expired - will retry with new token - '{requestUri}'.");
                tokenService.SetAccessToken(string.Empty);
                return await GetData(requestUri).ConfigureAwait(false);
            }
        }

        private async Task<HelpPageModel> GetData(Uri requestUri)
        {
            using (var client = await sitefinityODataContext.GetHttpClientAsync().ConfigureAwait(false))
            {
                logger.LogInformation($"Requested with url - '{requestUri}'.");

                var resultMessage = await client.GetAsync(requestUri).ConfigureAwait(false);
                if (resultMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAccessException(resultMessage.ReasonPhrase);
                }

                if (!resultMessage.IsSuccessStatusCode)
                {
                    return null;
                }

                var result = await client.GetStringAsync(requestUri).ConfigureAwait(false);
                return JsonConvert.DeserializeObject<HelpPageModel>(result);

            }
        }
    }
}