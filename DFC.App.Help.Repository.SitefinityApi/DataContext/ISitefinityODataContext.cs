using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.App.Help.Repository.SitefinityApi.DataContext
{
    public interface ISitefinityODataContext
    {
        Task<HttpClient> GetHttpClientAsync();
    }
}