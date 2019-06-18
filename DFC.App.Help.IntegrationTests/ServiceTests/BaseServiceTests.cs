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
            services.AddScoped<Services.IHelpPageService, Services.HelpPageService>();

            var cosmosDbConnection = configuration.GetSection("Configurations:CosmosDbConnections:HelpPages").Get<Help.Models.Cosmos.CosmosDbConnection>();

            _serviceProvider = services.BuildServiceProvider();

            // set the environment variables
            Help.Cosmos.Client.DocumentDBClient.CosmosDbConnection = cosmosDbConnection;
            Help.Cosmos.Helper.DocumentDBHelper.CosmosDbConnection = cosmosDbConnection;
        }

        [TearDown]
        public void TearDown()
        {
            _serviceProvider = null;
        }

        #endregion

    }
}
