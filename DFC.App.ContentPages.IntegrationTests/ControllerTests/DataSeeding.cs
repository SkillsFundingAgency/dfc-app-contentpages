using DFC.App.ContentPages.Data.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace DFC.App.ContentPages.IntegrationTests.ControllerTests
{
    public static class DataSeeding
    {
        public static void SeedDefaultArticle(CustomWebApplicationFactory<DFC.App.ContentPages.Startup> factory, string category1, string category2)
        {
            const string url = "/pages";
            var contentPageModels = new List<ContentPageModel>()
            {
                new ContentPageModel()
                {
                    DocumentId = Guid.Parse("9244BFF6-BA0C-40DB-AD52-A293C37441B1"),
                    Category = category1,
                    CanonicalName = category1,
                    IncludeInSitemap = true,
                    Content = "<h1>A document ({0})</h1>",
                    LastReviewed = DateTime.UtcNow,
                },
                new ContentPageModel()
                {
                    DocumentId = Guid.Parse("C16B389D-91AD-4F3D-B485-9F7EE953AFE4"),
                    Category = category1,
                    CanonicalName = "in-sitemap",
                    IncludeInSitemap = true,
                    Content = "<h1>A document ({0})</h1>",
                    LastReviewed = DateTime.UtcNow,
                },
                new ContentPageModel()
                {
                    DocumentId = Guid.Parse("C0103C26-E7C9-4008-BF66-1B2DB192177E"),
                    Category = category1,
                    CanonicalName = "not-in-sitemap",
                    IncludeInSitemap = false,
                    Content = "<h1>A document ({0})</h1>",
                    LastReviewed = DateTime.UtcNow,
                },
                new ContentPageModel()
                {
                    DocumentId = Guid.Parse("ACA3E7A6-5D5A-4E44-B2DC-A0399CF6F56A"),
                    Category = category1,
                    CanonicalName = "not-in-sitemap",
                    IncludeInSitemap = false,
                    Content = "<h1>A document ({0})</h1>",
                    LastReviewed = DateTime.UtcNow,
                },
                new ContentPageModel()
                {
                    DocumentId = Guid.Parse("22382F85-03CF-4F47-875A-21C6DA3DFD53"),
                    Category = category2,
                    CanonicalName = category2,
                    IncludeInSitemap = true,
                    Content = "<h1>A document ({0})</h1>",
                    LastReviewed = DateTime.UtcNow,
                },
                new ContentPageModel()
                {
                    DocumentId = Guid.Parse("8B038A4D-0E9A-48BB-86F4-8F110603A063"),
                    Category = category2,
                    CanonicalName = "in-sitemap",
                    IncludeInSitemap = true,
                    Content = "<h1>A document ({0})</h1>",
                    LastReviewed = DateTime.UtcNow,
                },
                new ContentPageModel()
                {
                    DocumentId = Guid.Parse("37A194E0-F249-4E5A-B930-82A7B3703A01"),
                    Category = category2,
                    CanonicalName = "not-in-sitemap",
                    IncludeInSitemap = false,
                    Content = "<h1>A document ({0})</h1>",
                    LastReviewed = DateTime.UtcNow,
                },
            };

            var client = factory?.CreateClient();

            client.DefaultRequestHeaders.Accept.Clear();

            contentPageModels.ForEach(f => client.PostAsync(url, f, new JsonMediaTypeFormatter()));
        }
    }
}
