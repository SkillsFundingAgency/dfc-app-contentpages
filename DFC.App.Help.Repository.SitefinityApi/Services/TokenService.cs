using DFC.App.DraftHelp.PageService;
using IdentityModel.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.App.Help.Repository.SitefinityApi.Services
{
    public class TokenService : ITokenService
    {
        private readonly SitefinityAPIConnectionSettings settings;
        private readonly ILogger<TokenService> logger;
        private readonly HttpClient httpClient;
        private string accessToken;

        public TokenService(SitefinityAPIConnectionSettings settings, ILogger<TokenService> logger, HttpClient httpClient)
        {
            this.settings = settings;
            this.logger = logger;
            this.httpClient = httpClient;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            if (!string.IsNullOrEmpty(this.accessToken))
            {
                return accessToken;
            }

            var disco = await httpClient.GetDiscoveryDocumentAsync($"{settings.SitefinityApiUrlBase}/{settings.AuthTokenEndpoint}").ConfigureAwait(false);
            if (disco.IsError)
            {
                logger.LogInformation($"Token client {settings.AuthTokenEndpoint} called with client {settings.ClientId} failed with error {disco.Error}.");
                throw new ApplicationException("Couldn't get Discovery document. Error: " + disco.Error);
            }

            var tokenResponse = await RequestToken(disco).ConfigureAwait(false);
            if (tokenResponse.IsError)
            {
                logger.LogInformation($"Token client {settings.AuthTokenEndpoint} called with client {settings.ClientId} failed with error {tokenResponse.Error}.");
                throw new ApplicationException("Couldn't get access token. Error: " + tokenResponse.Error);
            }

            return tokenResponse.AccessToken;
        }

        private async Task<TokenResponse> RequestToken(DiscoveryResponse disco)
        {
            return await httpClient.RequestPasswordTokenAsync(
                new PasswordTokenRequest
                {
                    Address = disco.TokenEndpoint,
                    ClientId = settings.ClientId,
                    ClientSecret = settings.ClientSecret,
                    Scope = settings.Scopes,
                    UserName = settings.Username,
                    Password = settings.Password,
                    Parameters = new Dictionary<string, string>()
                    {
                        { "membershipProvider", "Default" },
                    },
                })
                .ConfigureAwait(false);
        }

        public void SetAccessToken(string accessTokenSet) => this.accessToken = accessTokenSet;
    }
}