using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.App.ContentPages.Data.Contracts
{
    public interface IContentPageService
    {
        Task<bool> PingAsync();

        Task<IEnumerable<ContentPageModel>> GetAllAsync();

        Task<ContentPageModel> GetByIdAsync(Guid documentId);

        Task<ContentPageModel> GetByNameAsync(string canonicalName);

        Task<ContentPageModel> GetByAlternativeNameAsync(string alternativeName);

        Task<ContentPageModel> CreateAsync(ContentPageModel contentPageModel);

        Task<ContentPageModel> ReplaceAsync(ContentPageModel contentPageModel);

        Task<bool> DeleteAsync(Guid documentId);
    }
}