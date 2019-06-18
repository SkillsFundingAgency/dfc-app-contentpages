using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.App.Help.Models.Cosmos
{
    public static class EnvironmentVariableNames
    {
        public const string CosmosConnectionString = "CosmosSettings__ConnectionString";
        public const string CosmosDatabaseId = "CosmosSettings__DatabaseId";
        public const string CosmosCollectionId = "CosmosSettings__CollectionId";
        public const string CosmosPartitionKey = "CosmosSettings__PartitionKey";
    }
}
