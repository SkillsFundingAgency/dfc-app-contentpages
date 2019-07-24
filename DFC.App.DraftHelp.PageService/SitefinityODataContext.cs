using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DFC.App.DraftHelp.PageService
{
    public class SitefinityODataContext : IOdataContext
    {
        private const string ContentType = "application/json";
        private readonly ITokenService tokenService;

        public SitefinityODataContext(ITokenService tokenService)
        {
            this.tokenService = tokenService;
        }

        public async Task<HttpClient> GetHttpClientAsync()
        {
            var accessToken = await tokenService.GetAccessTokenAsync().ConfigureAwait(false);

            var httpClient = new HttpClient();
            httpClient.SetBearerToken(accessToken);
            httpClient.DefaultRequestHeaders.Add("X-SF-Service-Request", "true");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ContentType));

            return httpClient;
        }
    }
}