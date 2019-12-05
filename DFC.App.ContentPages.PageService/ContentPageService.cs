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

        public async Task<ContentPageModel> GetByIdAsync(Guid documentId)
        {
            return await repository.GetAsync(d => d.DocumentId == documentId).ConfigureAwait(false);
        }

        public async Task<ContentPageModel> GetByNameAsync(string canonicalName)
        {
            if (string.IsNullOrWhiteSpace(canonicalName))
            {
                throw new ArgumentNullException(nameof(canonicalName));
            }

            return await repository.GetAsync(d => d.CanonicalName == canonicalName.ToLowerInvariant()).ConfigureAwait(false);
        }

        public async Task<ContentPageModel> GetByAlternativeNameAsync(string alternativeName)
        {
            if (string.IsNullOrWhiteSpace(alternativeName))
            {
                throw new ArgumentNullException(nameof(alternativeName));
            }

            return await repository.GetAsync(d => d.AlternativeNames.Contains(alternativeName.ToLowerInvariant())).ConfigureAwait(false);
        }

        public async Task<ContentPageModel> CreateAsync(ContentPageModel contentPageModel)
        {
            if (contentPageModel == null)
            {
                throw new ArgumentNullException(nameof(contentPageModel));
            }

            var result = await repository.CreateAsync(contentPageModel).ConfigureAwait(false);

            return result == HttpStatusCode.Created
                ? await GetByIdAsync(contentPageModel.DocumentId).ConfigureAwait(false)
                : null;
        }

        public async Task<ContentPageModel> ReplaceAsync(ContentPageModel contentPageModel)
        {
            if (contentPageModel == null)
            {
                throw new ArgumentNullException(nameof(contentPageModel));
            }

            var result = await repository.UpdateAsync(contentPageModel.DocumentId, contentPageModel).ConfigureAwait(false);

            return result == HttpStatusCode.OK
                ? await GetByIdAsync(contentPageModel.DocumentId).ConfigureAwait(false)
                : null;
        }

        public async Task<bool> DeleteAsync(Guid documentId)
        {
            var result = await repository.DeleteAsync(documentId).ConfigureAwait(false);

            return result == HttpStatusCode.NoContent;
        }
    }
}