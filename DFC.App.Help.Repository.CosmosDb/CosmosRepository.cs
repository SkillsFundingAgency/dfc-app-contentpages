﻿using DFC.App.Help.Data;
using DFC.App.Help.Data.Contracts;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.Help.Repository.CosmosDb
{
    public class CosmosRepository<T> : ICosmosRepository<T>
        where T : IDataModel
    {
        private readonly CosmosDbConnection cosmosDbConnection;
        private readonly IDocumentClient documentClient;

        public CosmosRepository(CosmosDbConnection cosmosDbConnection, IDocumentClient documentClient)
        {
            this.cosmosDbConnection = cosmosDbConnection;
            this.documentClient = documentClient;

            CreateDatabaseIfNotExistsAsync().Wait();
            CreateCollectionIfNotExistsAsync().Wait();
        }

        private Uri DocumentCollectionUri => UriFactory.CreateDocumentCollectionUri(cosmosDbConnection.DatabaseId, cosmosDbConnection.CollectionId);

        public async Task<bool> PingAsync()
        {
            var query = documentClient.CreateDocumentQuery<T>(DocumentCollectionUri, new FeedOptions { MaxItemCount = 1, EnableCrossPartitionQuery = true })
                                       .AsDocumentQuery();

            if (query == null)
            {
                return false;
            }

            var models = await query.ExecuteNextAsync<T>().ConfigureAwait(false);
            var firstModel = models.FirstOrDefault();

            return firstModel != null;
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> where)
        {
            var query = documentClient.CreateDocumentQuery<T>(DocumentCollectionUri, new FeedOptions { MaxItemCount = 1, EnableCrossPartitionQuery = true })
                                       .Where(where)
                                       .AsDocumentQuery();

            if (query == null)
            {
                return default(T);
            }

            var models = await query.ExecuteNextAsync<T>().ConfigureAwait(false);

            if (models != null && models.Count > 0)
            {
                return models.FirstOrDefault();
            }

            return default(T);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var query = documentClient.CreateDocumentQuery<T>(DocumentCollectionUri, new FeedOptions { EnableCrossPartitionQuery = true })
                                       .AsDocumentQuery();

            var models = new List<T>();

            while (query.HasMoreResults)
            {
                var result = await query.ExecuteNextAsync<T>().ConfigureAwait(false);

                models.AddRange(result);
            }

            return models.Any() ? models : null;
        }

        public async Task<HttpStatusCode> CreateAsync(T model)
        {
            var result = await documentClient.CreateDocumentAsync(DocumentCollectionUri, model).ConfigureAwait(false);

            return result.StatusCode;
        }

        public async Task<HttpStatusCode> UpdateAsync(Guid documentId, T model)
        {
            var documentUri = CreateDocumentUri(documentId);

            var result = await documentClient.ReplaceDocumentAsync(documentUri, model).ConfigureAwait(false);

            return result.StatusCode;
        }

        public async Task<HttpStatusCode> DeleteAsync(Guid documentId)
        {
            var documentUri = CreateDocumentUri(documentId);

            var result = await documentClient.DeleteDocumentAsync(documentUri, new RequestOptions() { PartitionKey = new PartitionKey(Undefined.Value) }).ConfigureAwait(false);

            return result.StatusCode;
        }

        private async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                await documentClient.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(cosmosDbConnection.DatabaseId)).ConfigureAwait(false);
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await documentClient.CreateDatabaseAsync(new Database { Id = cosmosDbConnection.DatabaseId }).ConfigureAwait(false);
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
                await documentClient.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(cosmosDbConnection.DatabaseId, cosmosDbConnection.CollectionId)).ConfigureAwait(false);
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    var pkDef = new PartitionKeyDefinition
                    {
                        Paths = new Collection<string>() { cosmosDbConnection.PartitionKey },
                    };

                    await documentClient.CreateDocumentCollectionAsync(
                                UriFactory.CreateDatabaseUri(cosmosDbConnection.DatabaseId),
                                new DocumentCollection { Id = cosmosDbConnection.CollectionId, PartitionKey = pkDef },
                                new RequestOptions { OfferThroughput = 1000 }).ConfigureAwait(false);
                }
                else
                {
                    throw;
                }
            }
        }

        private Uri CreateDocumentUri(Guid documentId)
        {
            return UriFactory.CreateDocumentUri(cosmosDbConnection.DatabaseId, cosmosDbConnection.CollectionId, documentId.ToString());
        }
    }
}
