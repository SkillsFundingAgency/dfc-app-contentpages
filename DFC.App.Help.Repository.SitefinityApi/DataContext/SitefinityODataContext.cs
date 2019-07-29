using DFC.App.Help.Repository.SitefinityApi.Services;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DFC.App.Help.Repository.SitefinityApi.DataContext
{
    public class SitefinityODataContext : ISitefinityODataContext
    {
        private const string ContentType = "application/json";
        private readonly ITokenService tokenService;
        private readonly HttpClient httpClient;

        public SitefinityODataContext(ITokenService tokenService, HttpClient httpClient)
        {
            this.tokenService = tokenService;
            this.httpClient = httpClient;
        }

        public async Task<HttpClient> GetHttpClientAsync()
        {
            var accessToken = await tokenService.GetAccessTokenAsync().ConfigureAwait(false);

            httpClient.SetBearerToken(accessToken);
            httpClient.DefaultRequestHeaders.Add("X-SF-Service-Request", "true");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ContentType));

            return httpClient;
        }
    }
}