using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DFC.App.Help.Cosmos.Client;
using DFC.App.Help.Cosmos.Helper;
using DFC.App.Help.Models.Cosmos;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace DFC.App.Help.Cosmos.Provider
{
    public class DocumentDBProvider : IDocumentDBProvider
    {
        public async Task<HelpPageModel> GetHelpPageByNameAsync(string name)
        {
            var client = DocumentDBClient.CreateDocumentClient();

            if (client == null)
            {
                return null;
            }

            var collectionUri = DocumentDBHelper.CreateDocumentCollectionUri();

            var query = client.CreateDocumentQuery<HelpPageModel>(collectionUri, new FeedOptions { MaxItemCount = 1, EnableCrossPartitionQuery = true })
                              .Where(so => so.Name == name.ToLower() || so.Urls.Contains(name.ToLower()))
                              .AsDocumentQuery();

            if (query == null)
            {
                return null;
            }

            var helpPageModels = await query.ExecuteNextAsync<HelpPageModel>();

            return helpPageModels?.FirstOrDefault();
        }

        public async Task<HelpPageModel> GetHelpPageByIdAsync(Guid documentId)
        {
            var client = DocumentDBClient.CreateDocumentClient();

            if (client == null)
            {
                return null;
            }

            var collectionUri = DocumentDBHelper.CreateDocumentCollectionUri();

            var query = client.CreateDocumentQuery<HelpPageModel>(collectionUri, new FeedOptions { MaxItemCount = 1, EnableCrossPartitionQuery = true })
                              .Where(so => so.DocumentId == documentId)
                              .AsDocumentQuery();

            if (query == null)
            {
                return null;
            }

            var helpPageModels = await query.ExecuteNextAsync<HelpPageModel>();

            return helpPageModels?.FirstOrDefault();
        }

        public async Task<List<HelpPageModel>> GetAllHelpPageAsync()
        {
            var client = DocumentDBClient.CreateDocumentClient();

            if (client == null)
            {
                return null;
            }

            var collectionUri = DocumentDBHelper.CreateDocumentCollectionUri();

            var query = client.CreateDocumentQuery<HelpPageModel>(collectionUri, new FeedOptions { EnableCrossPartitionQuery = true })
                              .AsDocumentQuery();

            var helpPageModels = new List<HelpPageModel>();

            while (query.HasMoreResults)
            {
                var response = await query.ExecuteNextAsync<HelpPageModel>();

                helpPageModels.AddRange(response);
            }

            return helpPageModels.Any() ? helpPageModels : null;
        }

        public async Task<ResourceResponse<Document>> CreateHelpPageAsync(HelpPageModel helpPageModel)
        {
            var client = DocumentDBClient.CreateDocumentClient();

            if (client == null)
            {
                return null;
            }

            var collectionUri = DocumentDBHelper.CreateDocumentCollectionUri();

            var response = await client.CreateDocumentAsync(collectionUri, helpPageModel);

            return response;
        }

        public async Task<ResourceResponse<Document>> UpdateHelpPageAsync(HelpPageModel helpPageModel)
        {
            var client = DocumentDBClient.CreateDocumentClient();

            if (client == null)
            {
                return null;
            }

            var documentUri = DocumentDBHelper.CreateDocumentUri(helpPageModel.DocumentId);

            var response = await client.ReplaceDocumentAsync(documentUri, helpPageModel);

            return response;
        }

        public async Task<ResourceResponse<Document>> DeleteHelpPageAsync(Guid documentId)
        {
            var client = DocumentDBClient.CreateDocumentClient();

            if (client == null)
            {
                return null;
            }

            var documentUri = DocumentDBHelper.CreateDocumentUri(documentId);

            var response = await client.DeleteDocumentAsync(documentUri, new RequestOptions() { PartitionKey = new PartitionKey(Undefined.Value) });

            return response;
        }
    }
}
