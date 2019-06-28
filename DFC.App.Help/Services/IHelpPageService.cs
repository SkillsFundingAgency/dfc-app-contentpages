using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DFC.App.Help.Models.Cosmos;

namespace DFC.App.Help.Services
{
    public interface IHelpPageService
    {
        Task<List<HelpPageModel>> GetListAsync();

        Task<HelpPageModel> GetByIdAsync(Guid documentId);

        Task<HelpPageModel> GetByNameAsync(string name);

        Task<HelpPageModel> GetByAlternativeNameAsync(string name);

        Task<HelpPageModel> CreateAsync(HelpPageModel helpPageModel);

        Task<HelpPageModel> ReplaceAsync(HelpPageModel helpPageModel);

        Task<bool> DeleteAsync(Guid documentId);
    }
}
