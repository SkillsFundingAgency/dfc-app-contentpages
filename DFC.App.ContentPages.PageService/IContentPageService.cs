using DFC.App.ContentPages.Data;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.ContentPages.PageService
{
    public interface IContentPageService
    {
        Task<bool> PingAsync();

        Task<IEnumerable<ContentPageModel>> GetAllAsync();

        Task<IEnumerable<ContentPageModel>> GetAllAsync(string category);

        Task<ContentPageModel> GetByIdAsync(Guid documentId);

        Task<ContentPageModel> GetByNameAsync(string category, string canonicalName);

        Task<ContentPageModel> GetByAlternativeNameAsync(string category, string alternativeName);

        Task<HttpStatusCode> UpsertAsync(ContentPageModel contentPageModel);

        Task<bool> DeleteAsync(Guid documentId);
    }
}