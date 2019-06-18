using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DFC.App.Help.Models.Cosmos;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace DFC.App.Help.Cosmos.Provider
{
    public interface IDocumentDBProvider
    {
        Task<HelpPageModel> GetHelpPageByNameAsync(string name);

        Task<HelpPageModel> GetHelpPageByIdAsync(Guid documentId);

        Task<List<HelpPageModel>> GetAllHelpPageAsync();

        Task<ResourceResponse<Document>> CreateHelpPageAsync(HelpPageModel helpPageModel);

        Task<ResourceResponse<Document>> UpdateHelpPageAsync(HelpPageModel helpPageModel);

        Task<ResourceResponse<Document>> DeleteHelpPageAsync(Guid documentId);
    }
}