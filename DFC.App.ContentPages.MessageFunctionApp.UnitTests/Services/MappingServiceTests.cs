﻿using AutoMapper;
using DFC.App.ContentPages.Data.Enums;
using DFC.App.ContentPages.Data.Models;
using DFC.App.ContentPages.Data.ServiceBusModels;
using DFC.App.ContentPages.MessageFunctionApp.AutoMapperProfile;
using DFC.App.ContentPages.MessageFunctionApp.Services;
using FluentAssertions;
using Newtonsoft.Json;
using System;
using Xunit;

namespace DFC.App.ContentPages.MessageFunctionApp.UnitTests.Services
{
    [Trait("Messaging Function", "Mapping Service Tests")]
    public class MappingServiceTests
    {
        private const int SequenceNumber = 123;
        private const MessageContentType MessageContentTypeAlert = MessageContentType.Pages;
        private const string TestPageName = "Test Job name";
        private const string Title = "Title 1";
        private const bool IncludeInSitemap = true;
        private const string Description = "A description";
        private const string Keywords = "Some keywords";
        private const string Content = "<p>This is some content</p>";
        private static readonly string[] AlternativeNames = new string[] { "alt-name-1", "alt-name-2" };

        private static readonly DateTime LastModified = DateTime.UtcNow.AddDays(-1);
        private static readonly Guid ContentPageId = Guid.NewGuid();
        private static readonly string Category = MessageContentTypeAlert.ToString().ToLowerInvariant();

        private readonly IMappingService mappingService;

        public MappingServiceTests()
        {
            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile(new ContentPageProfile());
            });

            var mapper = new Mapper(config);

            mappingService = new MappingService(mapper);
        }

        [Fact]
        public void MapToContentPageModelWhenContentPageMessageSentThenItIsMappedCorrectly()
        {
            var fullJPMessage = BuildContentPageMessage();
            var message = JsonConvert.SerializeObject(fullJPMessage);
            var expectedResponse = BuildExpectedResponse();

            // Act
            var actualMappedModel = mappingService.MapToContentPageModel(message, SequenceNumber);

            // Assert
            expectedResponse.Should().BeEquivalentTo(actualMappedModel);
        }

        private static ContentPageMessage BuildContentPageMessage()
        {
            return new ContentPageMessage
            {
                ContentPageId = ContentPageId,
                Category = Category,
                CanonicalName = TestPageName,
                LastModified = LastModified,
                Title = Title,
                AlternativeNames = AlternativeNames,
                IncludeInSitemap = IncludeInSitemap,
                Description = Description,
                Keywords = Keywords,
                Content = Content,
            };
        }

        private static ContentPageModel BuildExpectedResponse()
        {
            return new ContentPageModel
            {
                CanonicalName = TestPageName,
                DocumentId = ContentPageId,
                SequenceNumber = SequenceNumber,
                Category = Category,
                Etag = null,
                BreadcrumbTitle = Title,
                IncludeInSitemap = IncludeInSitemap,
                AlternativeNames = AlternativeNames,
                Content = Content,
                LastReviewed = LastModified,
                MetaTags = new MetaTagsModel()
                {
                    Title = Title,
                    Description = Description,
                    Keywords = Keywords,
                },
            };
        }
    }
}
