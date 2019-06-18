namespace DFC.App.Help.IntegrationTests.Models
{
    /// <summary>
    /// Used to supply Cosmos DB connection values from app settings
    /// </summary>
    public class CosmosDbConnection
    {
        /// <summary>
        /// Cosmos DB - Connection string
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Cosmos DB - Database Id
        /// </summary>
        public string DatabaseId { get; set; }

        /// <summary>
        /// Cosmos DB - Collection Id
        /// </summary>
        public string CollectionId { get; set; }

        /// <summary>
        /// Cosmos DB - Partition Key
        /// </summary>
        public string PartitionKey { get; set; }
    }
}
