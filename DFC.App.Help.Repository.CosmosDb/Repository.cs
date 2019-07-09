using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using DFC.App.Help.Data;
using DFC.App.Help.Data.Contracts;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace DFC.App.Help.Repository.CosmosDb
{
    public class Repository<T> : IRepository<T>
        where T : IDataModel
    {
        private readonly CosmosDbConnection _cosmosDbConnection;
        private readonly DocumentClient _documentClient;

        private Uri DocumentCollectionUri => UriFactory.CreateDocumentCollectionUri(_cosmosDbConnection.DatabaseId, _cosmosDbConnection.CollectionId);

        public Repository(CosmosDbConnection cosmosDbConnection)
        {
            _cosmosDbConnection = cosmosDbConnection;
            _documentClient = new DocumentClient(new Uri(_cosmosDbConnection.EndpointUrl), _cosmosDbConnection.AccessKey);

            CreateDatabaseIfNotExistsAsync().Wait();
            CreateCollectionIfNotExistsAsync().Wait();
        }

        private Uri CreateDocumentUri(Guid documentId)
        {
            return UriFactory.CreateDocumentUri(_cosmosDbConnection.DatabaseId, _cosmosDbConnection.CollectionId, documentId.ToString());
        }

        public async Task<bool> PingAsync()
        {
            if (_documentClient == null)
            {
                return false;
            }

            var query = _documentClient.CreateDocumentQuery<T>(DocumentCollectionUri, new FeedOptions { MaxItemCount = 1, EnableCrossPartitionQuery = true })
                                       .AsDocumentQuery();

            if (query == null)
            {
                return false;
            }

            var models = await query.ExecuteNextAsync<T>();
            var firstModel = models.FirstOrDefault();

            return (firstModel != null);
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> where)
        {
            if (_documentClient == null)
            {
                return default(T);
            }

            var query = _documentClient.CreateDocumentQuery<T>(DocumentCollectionUri, new FeedOptions { MaxItemCount = 1, EnableCrossPartitionQuery = true })
                                       .Where(where)
                                       .AsDocumentQuery();

            if (query == null)
            {
                return default(T);
            }

            var models = await query.ExecuteNextAsync<T>();

            if (models != null && models.Count > 0)
            {
                return models.FirstOrDefault();
            }

            return default(T);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if (_documentClient == null)
            {
                return null;
            }

            var query = _documentClient.CreateDocumentQuery<T>(DocumentCollectionUri, new FeedOptions { EnableCrossPartitionQuery = true })
                                       .AsDocumentQuery();

            var models = new List<T>();

            while (query.HasMoreResults)
            {
                var result = await query.ExecuteNextAsync<T>();

                models.AddRange(result);
            }

            return models.Any() ? models : null;
        }

        public async Task<HttpStatusCode> CreateAsync(T model)
        {
            if (_documentClient == null)
            {
                return HttpStatusCode.FailedDependency;
            }

            var result = await _documentClient.CreateDocumentAsync(DocumentCollectionUri, model);

            return result.StatusCode;
        }

        public async Task<HttpStatusCode> UpdateAsync(Guid documentId, T model)
        {
            if (_documentClient == null)
            {
                return HttpStatusCode.FailedDependency;
            }

            var documentUri = CreateDocumentUri(documentId);

            var result = await _documentClient.ReplaceDocumentAsync(documentUri, model);

            return result.StatusCode;
        }

        public async Task<HttpStatusCode> DeleteAsync(Guid documentId)
        {
            if (_documentClient == null)
            {
                return HttpStatusCode.FailedDependency;
            }

            var documentUri = CreateDocumentUri(documentId);

            var result = await _documentClient.DeleteDocumentAsync(documentUri, new RequestOptions() { PartitionKey = new PartitionKey(Undefined.Value) });

            return result.StatusCode;
        }

        private async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                await _documentClient.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(_cosmosDbConnection.DatabaseId));
            }
            catch (Microsoft.Azure.Documents.DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await _documentClient.CreateDatabaseAsync(new Microsoft.Azure.Documents.Database { Id = _cosmosDbConnection.DatabaseId });
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task CreateCollectionIfNotExistsAsync()
        {
            try
            {
                await _documentClient.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(_cosmosDbConnection.DatabaseId, _cosmosDbConnection.CollectionId));
            }
            catch (Microsoft.Azure.Documents.DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {

                    var pkDef = new PartitionKeyDefinition
                    {
                        Paths = new Collection<string>() { _cosmosDbConnection.PartitionKey }
                    };

                    await _documentClient.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(_cosmosDbConnection.DatabaseId),
                        new Microsoft.Azure.Documents.DocumentCollection { Id = _cosmosDbConnection.CollectionId, PartitionKey = pkDef },
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
