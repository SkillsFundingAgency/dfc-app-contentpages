using DFC.App.Help.Data;
using DFC.App.Help.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.Help.PageService
{
    public class HelpPageService : IHelpPageService
    {
        private readonly IRepository<HelpPageModel> documentDbProvider;

        public HelpPageService(IRepository<HelpPageModel> documentDbProvider)
        {
            this.documentDbProvider = documentDbProvider;
        }

        public async Task<bool> PingAsync()
        {
            var results = await documentDbProvider.PingAsync().ConfigureAwait(false);

            return results;
        }

        public async Task<IEnumerable<HelpPageModel>> GetAllAsync()
        {
            var results = await documentDbProvider.GetAllAsync().ConfigureAwait(false);

            return results;
        }

        public async Task<HelpPageModel> GetByIdAsync(Guid documentId)
        {
            var result = await documentDbProvider.GetAsync(d => d.DocumentId == documentId).ConfigureAwait(false);

            return result;
        }

        public async Task<HelpPageModel> GetByNameAsync(string canonicalName)
        {
            var result = await documentDbProvider.GetAsync(d => d.CanonicalName == canonicalName.ToLower()).ConfigureAwait(false);

            return result;
        }

        public async Task<HelpPageModel> GetByAlternativeNameAsync(string alternativeName)
        {
            var result = await documentDbProvider.GetAsync(d => d.AlternativeNames.Contains(alternativeName.ToLower())).ConfigureAwait(false);

            return result;
        }

        public async Task<HelpPageModel> CreateAsync(HelpPageModel helpPageModel)
        {
            var result = await documentDbProvider.CreateAsync(helpPageModel).ConfigureAwait(false);

            if (result == HttpStatusCode.Created)
            {
                return await GetByIdAsync(helpPageModel.DocumentId).ConfigureAwait(false);
            }

            return null;
        }

        public async Task<HelpPageModel> ReplaceAsync(HelpPageModel helpPageModel)
        {
            var result = await documentDbProvider.UpdateAsync(helpPageModel.DocumentId, helpPageModel).ConfigureAwait(false);

            if (result == HttpStatusCode.OK)
            {
                return await GetByIdAsync(helpPageModel.DocumentId).ConfigureAwait(false);
            }

            return null;
        }

        public async Task<bool> DeleteAsync(Guid documentId)
        {
            var result = await documentDbProvider.DeleteAsync(documentId).ConfigureAwait(false);

            return result == HttpStatusCode.NoContent ? true : false;
        }
    }
}
