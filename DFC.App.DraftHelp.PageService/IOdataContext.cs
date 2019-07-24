using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.App.DraftHelp.PageService
{
    public interface IOdataContext
    {
        Task<HttpClient> GetHttpClientAsync();
    }
}