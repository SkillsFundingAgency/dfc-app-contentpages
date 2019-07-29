using System.Threading.Tasks;

namespace DFC.App.Help.Data.Contracts
{
    public interface IDraftHelpPageService
    {
        Task<HelpPageModel> GetSitefinityData(string canonicalName);
    }
}