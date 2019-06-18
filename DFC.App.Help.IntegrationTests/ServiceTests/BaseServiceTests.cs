using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DFC.App.Help.IntegrationTests.ServiceTests
{
    [TestFixture]
    public abstract class BaseServiceTests
    {
        protected static IServiceProvider _serviceProvider;

        #region Tests initialisations and cleanup

        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection();

            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            services.AddSingleton<IConfiguration>(configuration);

            services.AddSingleton<Cosmos.Provider.IDocumentDBProvider, Cosmos.Provider.DocumentDBProvider>();
            services.AddSingleton<Services.IHelpPageService, Services.HelpPageService>();

            var helpPagesConfiguration = configuration.GetSection("Configurations:CosmosDbConnections:HelpPages").Get<Models.CosmosDbConnection>();

            _serviceProvider = services.BuildServiceProvider();

            // set the environment variables
            Environment.SetEnvironmentVariable(Help.Models.Cosmos.EnvironmentVariableNames.CosmosConnectionString, helpPagesConfiguration.ConnectionString);
            Environment.SetEnvironmentVariable(Help.Models.Cosmos.EnvironmentVariableNames.CosmosDatabaseId, helpPagesConfiguration.DatabaseId);
            Environment.SetEnvironmentVariable(Help.Models.Cosmos.EnvironmentVariableNames.CosmosCollectionId, helpPagesConfiguration.CollectionId);
            Environment.SetEnvironmentVariable(Help.Models.Cosmos.EnvironmentVariableNames.CosmosPartitionKey, helpPagesConfiguration.PartitionKey);
        }

        [TearDown]
        public void TearDown()
        {
            _serviceProvider = null;
        }

        #endregion

    }
}
