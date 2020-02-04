using System;
using System.Linq;
using System.Threading;
using DFC.App.Help.Data;
using FakeItEasy;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Xunit;

namespace DFC.App.Help.Repository.CosmosDb.UnitTests.RepositoryTests
{
    public class RepositoryGetAllTests
    {
        [Fact]
        public void RepositoryGetAllListReturnsSuccess()
        {
            // arrange
            var cosmosDbConnection = new CosmosDbConnection
            {
                AccessKey = "AccessKey",
                CollectionId = "CollectionId",
                DatabaseId = "DatabaseId",
                EndpointUrl = "EndpointUrl",
                PartitionKey = "PartitionKey",
            };
            var documentClient = A.Fake<IDocumentClient>();
            var documentQueryable = A.Fake<IOrderedQueryable<HelpPageModel>>();
            var documentQuery = A.Fake<IDocumentQuery<HelpPageModel>>();
            var feedResponse = A.Fake<FeedResponse<HelpPageModel>>();
            var expectedResults = A.CollectionOfFake<HelpPageModel>(2);

            A.CallTo(() => documentClient.ReadDatabaseAsync(A<Uri>.Ignored, null)).Returns(A.Fake<ResourceResponse<Database>>());
            A.CallTo(() => documentClient.ReadDocumentCollectionAsync(A<Uri>.Ignored, null)).Returns(A.Fake<ResourceResponse<DocumentCollection>>());
            A.CallTo(() => documentClient.CreateDocumentQuery<HelpPageModel>(A<Uri>.Ignored, A<FeedOptions>.Ignored)).Returns(documentQueryable);
            A.CallTo(() => documentQueryable.AsDocumentQuery()).Returns(documentQuery);
            A.CallTo(() => documentQuery.HasMoreResults).ReturnsNextFromSequence(true, true, false);
            A.CallTo(() => documentQuery.ExecuteNextAsync<HelpPageModel>(default(CancellationToken))).Returns(feedResponse);

            var repository = new Repository<HelpPageModel>(cosmosDbConnection, documentClient);

            // act
            var results = repository.GetAllAsync().Result;

            // assert
            A.CallTo(() => documentClient.ReadDatabaseAsync(A<Uri>.Ignored, null)).MustHaveHappenedOnceExactly();
            A.CallTo(() => documentClient.ReadDocumentCollectionAsync(A<Uri>.Ignored, null)).MustHaveHappenedOnceExactly();
            A.CallTo(() => documentClient.CreateDocumentQuery<HelpPageModel>(A<Uri>.Ignored, A<FeedOptions>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => documentQuery.HasMoreResults).MustHaveHappened(3, Times.Exactly);
            A.CallTo(() => documentQuery.ExecuteNextAsync<HelpPageModel>(default(CancellationToken))).MustHaveHappened(2, Times.Exactly);
            A.Equals(results, expectedResults);
        }

    }
}
