using System;
using DFC.App.Help.Data;
using DFC.App.Help.Data.Common;
using DFC.App.Help.Data.Contracts;
using DFC.App.Help.PageService;
using DFC.App.Help.Repository.CosmosDb;
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

            var cosmosDbConnection = configuration.GetSection(DFC.App.Help.Startup.CosmosDbConfigAppSettings).Get<CosmosDbConnection>();

            services.AddSingleton<CosmosDbConnection>(cosmosDbConnection);
            services.AddSingleton<IRepository<HelpPageModel>, Repository<HelpPageModel>>();
            services.AddScoped<IHelpPageService, HelpPageService>();

            _serviceProvider = services.BuildServiceProvider();
        }

        [TearDown]
        public void TearDown()
        {
            _serviceProvider = null;
        }

        #endregion

    }
}
