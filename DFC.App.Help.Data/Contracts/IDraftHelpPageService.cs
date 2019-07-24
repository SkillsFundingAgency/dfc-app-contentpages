using System.Threading.Tasks;

namespace DFC.App.Help.Data.Contracts
{
    public interface IDraftHelpPageService
    {
        HelpPageModel GetDummyDataByName(string canonicalName);

        Task<HelpPageModel> GetDataAsync(string canonicalName);
    }
}