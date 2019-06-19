using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using DFC.App.Help.Cosmos.Provider;
using DFC.App.Help.Models.Cosmos;

namespace DFC.App.Help.Services
{
    public class HelpPageService : IHelpPageService
    {
        private readonly IDocumentDBProvider _documentDbProvider;

        public HelpPageService(IDocumentDBProvider documentDbProvider)
        {
            _documentDbProvider = documentDbProvider;
        }

        public async Task<List<HelpPageModel>> GetListAsync()
        {
            var results = await _documentDbProvider.GetAllHelpPageAsync();

            return results;
        }

        public async Task<HelpPageModel> GetByIdAsync(Guid documentId)
        {
            var result = await _documentDbProvider.GetHelpPageByIdAsync(documentId);

            return result;
        }

        public async Task<HelpPageModel> GetByNameAsync(string name)
        {
            var result = await _documentDbProvider.GetHelpPageByNameAsync(name);

            return result;
        }

        public async Task<HelpPageModel> CreateAsync(HelpPageModel helpPageModel)
        {
            helpPageModel.DocumentId = null;

            var response = await _documentDbProvider.CreateHelpPageAsync(helpPageModel);

            return response.StatusCode == HttpStatusCode.Created ? (dynamic)response.Resource : null;
        }

        public async Task<HelpPageModel> ReplaceAsync(HelpPageModel helpPageModel)
        {
            var response = await _documentDbProvider.UpdateHelpPageAsync(helpPageModel);

            return response.StatusCode == HttpStatusCode.OK ? (dynamic)response.Resource : null;
        }

        public async Task<bool> DeleteAsync(Guid documentId)
        {
            var response = await _documentDbProvider.DeleteHelpPageAsync(documentId);

            return response.StatusCode == HttpStatusCode.NoContent ? true : false;
        }

    }
}
