using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace DFC.App.Help.Cosmos.Client
{
    public static class DocumentDBClient
    {
        private static DocumentClient _documentClient;
        private static string _connectionString;
        private static string _databaseId;
        private static string _collectionId;
        private static string _partitionKey;

        public static DocumentClient CreateDocumentClient()
        {
            if (_documentClient != null)
            {
                return _documentClient;
            }

            _connectionString = Environment.GetEnvironmentVariable(Models.Cosmos.EnvironmentVariableNames.CosmosConnectionString);
            _databaseId = Environment.GetEnvironmentVariable(Models.Cosmos.EnvironmentVariableNames.CosmosDatabaseId);
            _collectionId = Environment.GetEnvironmentVariable(Models.Cosmos.EnvironmentVariableNames.CosmosCollectionId);
            _partitionKey = Environment.GetEnvironmentVariable(Models.Cosmos.EnvironmentVariableNames.CosmosPartitionKey);

            _documentClient = InitialiseDocumentClient();

            CreateDatabaseIfNotExistsAsync().Wait();
            CreateCollectionIfNotExistsAsync().Wait();

            return _documentClient;
        }

        private static DocumentClient InitialiseDocumentClient()
        {
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                throw new ArgumentNullException();
            }

            var endPoint = _connectionString.Split(new[] { "AccountEndpoint=" }, StringSplitOptions.None)[1]
                .Split(';')[0]
                .Trim();

            if (string.IsNullOrWhiteSpace(endPoint))
            {
                throw new ArgumentNullException();
            }

            var key = _connectionString.Split(new[] { "AccountKey=" }, StringSplitOptions.None)[1]
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
                await _documentClient.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(_databaseId));
            }
            catch (Microsoft.Azure.Documents.DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await _documentClient.CreateDatabaseAsync(new Microsoft.Azure.Documents.Database { Id = _databaseId });
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
                await _documentClient.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId));
            }
            catch (Microsoft.Azure.Documents.DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {

                    var pkDef = new PartitionKeyDefinition
                    {
                        Paths = new Collection<string>() { _partitionKey }
                    };

                    await _documentClient.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(_databaseId),
                        new Microsoft.Azure.Documents.DocumentCollection { Id = _collectionId, PartitionKey = pkDef },
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
