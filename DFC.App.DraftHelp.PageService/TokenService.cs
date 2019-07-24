using IdentityModel.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.App.DraftHelp.PageService
{
    public class TokenService : ITokenService
    {
        private readonly SitefinityAPIConnectionSettings settings;
        private readonly ILogger<TokenService> logger;
        private string accessToken;

        public TokenService(SitefinityAPIConnectionSettings settings, ILogger<TokenService> logger)
        {
            this.settings = settings;
            this.logger = logger;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            if (!string.IsNullOrEmpty(this.accessToken))
            {
                return accessToken;
            }

            using (var httpClientLocal = new HttpClient())
            {
                var disco = await httpClientLocal.GetDiscoveryDocumentAsync($"{settings.SiteFinityApiUrlbase}/{settings.TokenEndpoint}").ConfigureAwait(false);
                if (disco.IsError)
                {
                    logger.LogInformation($"Token client {settings.TokenEndpoint} called with client {settings.ClientId} failed with error {disco.Error}.");
                    throw new ApplicationException("Couldn't get access token. Error: " + disco.Error);
                }

                var tokenResponse = await httpClientLocal.RequestPasswordTokenAsync(
                    new PasswordTokenRequest
                    {
                        Address = disco.TokenEndpoint,
                        ClientId = settings.ClientId,
                        ClientSecret = settings.ClientSecret,
                        Scope = settings.Scopes,
                        UserName = settings.Username,
                        Password = settings.Password,
                        Parameters = AdditionalParameters,
                    })
                .ConfigureAwait(false);

                if (tokenResponse.IsError)
                {
                    logger.LogInformation($"Token client {settings.TokenEndpoint} called with client {settings.ClientId} failed with error {tokenResponse.Error}.");
                    throw new ApplicationException("Couldn't get access token. Error: " + tokenResponse.Error);
                }

                return tokenResponse.AccessToken;
            }
        }

        private static readonly Dictionary<string, string> AdditionalParameters = new Dictionary<string, string>()
        {
            { "membershipProvider", "Default" },
        };

        public void SetAccessToken(string accessTokenSet) => this.accessToken = accessTokenSet;
    }
}