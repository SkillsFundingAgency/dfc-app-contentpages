using System.Threading.Tasks;

namespace DFC.App.Help.Data.Contracts
{
    public interface IDraftHelpPageService
    {
        HelpPageModel GetByName(string canonicalName);
    }
}