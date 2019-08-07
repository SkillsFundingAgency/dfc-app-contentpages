using DFC.App.Help.Data;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace DFC.App.Help.IntegrationTests.ControllerTests
{
    public static class DataSeeding
    {
        public static void SeedDefaultArticle(CustomWebApplicationFactory<DFC.App.Help.Startup> factory, string article)
        {
            const string url = "/pages";
            var helpPageModels = new List<HelpPageModel>()
            {
                new HelpPageModel()
                {
                    DocumentId = Guid.Parse("63DEA97E-B61C-4C14-85DC-1BD08EA20DC8"),
                    CanonicalName = article,
                    IncludeInSitemap = true,
                    LastReviewed = DateTime.UtcNow,
                },
                new HelpPageModel()
                {
                    DocumentId = Guid.Parse("C16B389D-91AD-4F3D-B485-9F7EE953AFE4"),
                    CanonicalName = "in-sitemap",
                    IncludeInSitemap = true,
                    LastReviewed = DateTime.UtcNow,
                },
                new HelpPageModel()
                {
                    DocumentId = Guid.Parse("C0103C26-E7C9-4008-BF66-1B2DB192177E"),
                    CanonicalName = "not-in-sitemap",
                    IncludeInSitemap = false,
                    LastReviewed = DateTime.UtcNow,
                },
            };

            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            var client = factory.CreateClient();

            client.DefaultRequestHeaders.Accept.Clear();

            helpPageModels.ForEach(f => client.PostAsync(url, f, new JsonMediaTypeFormatter()));
        }
    }
}
