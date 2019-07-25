using System.Threading.Tasks;

namespace DFC.App.Help.Repository.SitefinityApi.Services
{
    public interface ITokenService
    {
        Task<string> GetAccessTokenAsync();

        void SetAccessToken(string accessToken);
    }
}