using System;
using Microsoft.Azure.Documents.Client;

namespace DFC.App.Help.Cosmos.Helper
{
    public static class DocumentDBHelper
    {
        public static Models.Cosmos.CosmosDbConnection CosmosDbConnection;
        private static Uri _documentCollectionUri;

        public static Uri CreateDocumentCollectionUri()
        {
            if (_documentCollectionUri != null)
            {
                return _documentCollectionUri;
            }

            _documentCollectionUri = UriFactory.CreateDocumentCollectionUri(CosmosDbConnection.DatabaseId, CosmosDbConnection.CollectionId);

            return _documentCollectionUri;
        }

        public static Uri CreateDocumentUri(Guid RegionId)
        {
            return UriFactory.CreateDocumentUri(CosmosDbConnection.DatabaseId, CosmosDbConnection.CollectionId, RegionId.ToString());
        }

    }
}
