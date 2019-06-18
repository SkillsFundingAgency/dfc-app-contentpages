using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace DFC.App.Help.Cosmos.Client
{
    public static class DocumentDBClient
    {
        public static Models.Cosmos.CosmosDbConnection CosmosDbConnection;
        private static DocumentClient _documentClient;

        public static DocumentClient CreateDocumentClient()
        {
            if (_documentClient != null)
            {
                return _documentClient;
            }

            _documentClient = InitialiseDocumentClient();

            CreateDatabaseIfNotExistsAsync().Wait();
            CreateCollectionIfNotExistsAsync().Wait();

            return _documentClient;
        }

        private static DocumentClient InitialiseDocumentClient()
        {
            if (string.IsNullOrWhiteSpace(CosmosDbConnection.ConnectionString))
            {
                throw new ArgumentNullException();
            }

            var endPoint = CosmosDbConnection.ConnectionString.Split(new[] { "AccountEndpoint=" }, StringSplitOptions.None)[1]
                .Split(';')[0]
                .Trim();

            if (string.IsNullOrWhiteSpace(endPoint))
            {
                throw new ArgumentNullException();
            }

            var key = CosmosDbConnection.ConnectionString.Split(new[] { "AccountKey=" }, StringSplitOptions.None)[1]
                .Split(';')[0]
                .Trim();

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException();
            }

            return new DocumentClient(new Uri(endPoint), key);
        }

        private static async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                await _documentClient.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(CosmosDbConnection.DatabaseId));
            }
            catch (Microsoft.Azure.Documents.DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await _documentClient.CreateDatabaseAsync(new Microsoft.Azure.Documents.Database { Id = CosmosDbConnection.DatabaseId });
                }
                else
                {
                    throw;
                }
            }
        }

        private static async Task CreateCollectionIfNotExistsAsync()
        {
            try
            {
                await _documentClient.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(CosmosDbConnection.DatabaseId, CosmosDbConnection.CollectionId));
            }
            catch (Microsoft.Azure.Documents.DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {

                    var pkDef = new PartitionKeyDefinition
                    {
                        Paths = new Collection<string>() { CosmosDbConnection.PartitionKey }
                    };

                    await _documentClient.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(CosmosDbConnection.DatabaseId),
                        new Microsoft.Azure.Documents.DocumentCollection { Id = CosmosDbConnection.CollectionId, PartitionKey = pkDef },
                        new RequestOptions { OfferThroughput = 1000 });
                }
                else
                {
                    throw;
                }
            }
        }

    }
}
