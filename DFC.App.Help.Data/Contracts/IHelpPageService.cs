using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.App.Help.Data.Contracts
{
    public interface IHelpPageService
    {
        Task<bool> PingAsync();

        Task<IEnumerable<HelpPageModel>> GetAllAsync();

        Task<HelpPageModel> GetByIdAsync(Guid documentId);

        Task<HelpPageModel> GetByNameAsync(string canonicalName);

        Task<HelpPageModel> GetByAlternativeNameAsync(string alternativeName);

        Task<HelpPageModel> CreateAsync(HelpPageModel helpPageModel);

        Task<HelpPageModel> ReplaceAsync(HelpPageModel helpPageModel);

        Task<bool> DeleteAsync(Guid documentId);
    }
}