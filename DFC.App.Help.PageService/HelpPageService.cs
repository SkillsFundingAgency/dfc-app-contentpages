using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DFC.App.Help.Data;
using DFC.App.Help.Data.Contracts;

namespace DFC.App.Help.PageService
{
    public class HelpPageService : IHelpPageService
    {
        private readonly IRepository<HelpPageModel> _documentDbProvider;

        public HelpPageService(IRepository<HelpPageModel> documentDbProvider)
        {
            _documentDbProvider = documentDbProvider;
        }

        public async Task<bool> PingAsync()
        {
            var results = await _documentDbProvider.PingAsync();

            return results;
        }

        public async Task<IEnumerable<HelpPageModel>> GetAllAsync()
        {
            var results = await _documentDbProvider.GetAllAsync();

            return results;
        }

        public async Task<HelpPageModel> GetByIdAsync(Guid documentId)
        {
            var result = await _documentDbProvider.GetAsync(d => d.DocumentId == documentId);

            return result;
        }

        public async Task<HelpPageModel> GetByNameAsync(string canonicalName)
        {
            var result = await _documentDbProvider.GetAsync(d => d.CanonicalName == canonicalName.ToLower());

            return result;
        }

        public async Task<HelpPageModel> GetByAlternativeNameAsync(string alternativeName)
        {
            var result = await _documentDbProvider.GetAsync(d => d.AlternativeNames.Contains(alternativeName.ToLower()));

            return result;
        }

        public async Task<HelpPageModel> CreateAsync(HelpPageModel helpPageModel)
        {
            var result = await _documentDbProvider.CreateAsync(helpPageModel);

            if (result == HttpStatusCode.Created)
            {
                return await GetByIdAsync(helpPageModel.DocumentId);
            }

            return null;
        }

        public async Task<HelpPageModel> ReplaceAsync(HelpPageModel helpPageModel)
        {
            var result = await _documentDbProvider.UpdateAsync(helpPageModel.DocumentId, helpPageModel);

            if (result == HttpStatusCode.OK)
            {
                return await GetByIdAsync(helpPageModel.DocumentId);
            }

            return null;
        }

        public async Task<bool> DeleteAsync(Guid documentId)
        {
            var result = await _documentDbProvider.DeleteAsync(documentId);

            return result == HttpStatusCode.NoContent ? true : false;
        }

    }
}
