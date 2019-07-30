using DFC.App.DraftHelp.PageService;
using DFC.App.Help.Data;
using DFC.App.Help.Data.Contracts;
using DFC.App.Help.Extensions;
using DFC.App.Help.PageService;
using DFC.App.Help.Policies.Options;
using DFC.App.Help.Repository.CosmosDb;
using DFC.App.Help.Repository.SitefinityApi.DataContext;
using DFC.App.Help.Repository.SitefinityApi.Services;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;

namespace DFC.App.Help.IntegrationTests.ServiceTests
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
            var sitefinityApiConnection = configuration.GetSection("SitefinityApi").Get<SitefinityAPIConnectionSettings>();
            var documentClient = new DocumentClient(new Uri(cosmosDbConnection.EndpointUrl), cosmosDbConnection.AccessKey);

            services.AddSingleton<IConfiguration>(configuration);
            services.AddSingleton(cosmosDbConnection);
            services.AddSingleton(sitefinityApiConnection);
            services.AddSingleton<IDocumentClient>(documentClient);
            services.AddSingleton<IRepository<HelpPageModel>, Repository<HelpPageModel>>();
            services.AddScoped<IHelpPageService, HelpPageService>();
            services.AddScoped<IDraftHelpPageService, DraftHelpPageService>();
            services.AddHttpClient<ISitefinityODataContext, SitefinityODataContext, HttpClientOptions>(configuration, nameof(HttpClientOptions));
            services.AddHttpClient<ITokenService, TokenService, HttpClientOptions>(configuration, nameof(HttpClientOptions));

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