using DFC.App.ContentPages.Data;
using DFC.App.ContentPages.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.ContentPages.PageService
{
    public class ContentPageService : IContentPageService
    {
        private readonly ICosmosRepository<ContentPageModel> repository;

        public ContentPageService(ICosmosRepository<ContentPageModel> repository)
        {
            this.repository = repository;
        }

        public async Task<bool> PingAsync()
        {
            return await repository.PingAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<ContentPageModel>> GetAllAsync()
        {
            return await repository.GetAllAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<ContentPageModel>> GetAllAsync(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                throw new ArgumentNullException(nameof(category));
            }

            return await repository.GetAllAsync(d => d.Category == category.ToLowerInvariant()).ConfigureAwait(false);
        }

        public async Task<ContentPageModel> GetByIdAsync(Guid documentId)
        {
            return await repository.GetAsync(d => d.DocumentId == documentId).ConfigureAwait(false);
        }

        public async Task<ContentPageModel> GetByNameAsync(string category, string canonicalName)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                throw new ArgumentNullException(nameof(category));
            }

            if (string.IsNullOrWhiteSpace(canonicalName))
            {
                throw new ArgumentNullException(nameof(canonicalName));
            }

            return await repository.GetAsync(d => d.Category == category.ToLowerInvariant() && d.CanonicalName == canonicalName.ToLowerInvariant()).ConfigureAwait(false);
        }

        public async Task<ContentPageModel> GetByAlternativeNameAsync(string category, string alternativeName)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                throw new ArgumentNullException(nameof(category));
            }

            if (string.IsNullOrWhiteSpace(alternativeName))
            {
                throw new ArgumentNullException(nameof(alternativeName));
            }

            return await repository.GetAsync(d => d.Category == category.ToLowerInvariant() && d.AlternativeNames.Contains(alternativeName.ToLowerInvariant())).ConfigureAwait(false);
        }

        public async Task<HttpStatusCode> UpsertAsync(ContentPageModel contentPageModel)
        {
            if (contentPageModel == null)
            {
                throw new ArgumentNullException(nameof(contentPageModel));
            }

            return await repository.UpsertAsync(contentPageModel).ConfigureAwait(false);
        }

        public async Task<bool> DeleteAsync(Guid documentId)
        {
            var result = await repository.DeleteAsync(documentId).ConfigureAwait(false);

            return result == HttpStatusCode.NoContent;
        }
    }
}