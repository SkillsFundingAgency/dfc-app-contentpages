using DFC.App.Help.Data;
using DFC.App.Help.Data.Contracts;
using DFC.App.Help.Repository.CosmosDb;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;

namespace DFC.App.Help.PageService.IntegrationTests.ServiceTests
{
    [TestFixture]
    public abstract class BaseServiceTests
    {
        private IServiceProvider serviceProvider;

        public IServiceProvider ServiceProvider => serviceProvider;

        #region Tests initialisations and cleanup

        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection().AddLogging();

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var cosmosDbConnection = configuration.GetSection(Startup.CosmosDbConfigAppSettings).Get<CosmosDbConnection>();
            var documentClient = new DocumentClient(new Uri(cosmosDbConnection.EndpointUrl), cosmosDbConnection.AccessKey);

            services.AddSingleton<IConfiguration>(configuration);
            services.AddSingleton(cosmosDbConnection);
            services.AddSingleton<IDocumentClient>(documentClient);
            services.AddSingleton<ICosmosRepository<HelpPageModel>, CosmosRepository<HelpPageModel>>();
            services.AddScoped<IHelpPageService, HelpPageService>();

            serviceProvider = services.BuildServiceProvider();
        }

        [TearDown]
        public void TearDown()
        {
            serviceProvider = null;
        }

        #endregion Tests initialisations and cleanup
    }
}