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
        private readonly IRepository<HelpPageModel> repository;
        private readonly IDraftHelpPageService draftHelpPageService;

        public HelpPageService(IRepository<HelpPageModel> repository, IDraftHelpPageService draftHelpPageService)
        {
            this.repository = repository;
            this.draftHelpPageService = draftHelpPageService;
        }

        public async Task<bool> PingAsync()
        {
            return await repository.PingAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<HelpPageModel>> GetAllAsync()
        {
            return await repository.GetAllAsync().ConfigureAwait(false);
        }

        public async Task<HelpPageModel> GetByIdAsync(Guid documentId)
        {
            return await repository.GetAsync(d => d.DocumentId == documentId).ConfigureAwait(false);
        }

        public async Task<HelpPageModel> GetByNameAsync(string canonicalName, bool isDraft = false)
        {
            return isDraft
                //? this.draftHelpPageService.GetDummyDataByName(canonicalName)
                ? await draftHelpPageService.GetDataAsync(canonicalName.ToLowerInvariant()).ConfigureAwait(false)
                : await repository.GetAsync(d => d.CanonicalName == canonicalName.ToLower()).ConfigureAwait(false);
        }

        public async Task<HelpPageModel> GetByAlternativeNameAsync(string alternativeName)
        {
            return await repository.GetAsync(d => d.AlternativeNames.Contains(alternativeName.ToLower())).ConfigureAwait(false);
        }

        public async Task<HelpPageModel> CreateAsync(HelpPageModel helpPageModel)
        {
            var result = await repository.CreateAsync(helpPageModel).ConfigureAwait(false);

            return result == HttpStatusCode.Created
                ? await GetByIdAsync(helpPageModel.DocumentId).ConfigureAwait(false)
                : null;
        }

        public async Task<HelpPageModel> ReplaceAsync(HelpPageModel helpPageModel)
        {
            var result = await repository.UpdateAsync(helpPageModel.DocumentId, helpPageModel).ConfigureAwait(false);

            return result == HttpStatusCode.OK
                ? await GetByIdAsync(helpPageModel.DocumentId).ConfigureAwait(false)
                : null;
        }

        public async Task<bool> DeleteAsync(Guid documentId)
        {
            var result = await repository.DeleteAsync(documentId).ConfigureAwait(false);

            return result == HttpStatusCode.NoContent;
        }
    }
}