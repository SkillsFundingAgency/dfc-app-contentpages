using System.Threading.Tasks;

namespace DFC.App.DraftHelp.PageService
{
    public interface ITokenService
    {
        Task<string> GetAccessTokenAsync();

        void SetAccessToken(string accessTokenSet);
    }
}