﻿using DFC.App.ContentPages.Data.Attributes;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.ContentPages.Data
{
    public class ContentPageModel : IDataModel
    {
        [Guid]
        [JsonProperty(PropertyName = "id")]
        public Guid DocumentId { get; set; }

        [JsonProperty(PropertyName = "_etag")]
        public string Etag { get; set; }

        [Required]
        [LowerCase]
        [UrlPath]
        public string Category { get; set; }

        [Required]
        [LowerCase]
        [UrlPath]
        public string CanonicalName { get; set; }

        public long SequenceNumber { get; set; }

        public string PartitionKey => Category;

        [Display(Name = "Breadcrumb Title")]
        public string BreadcrumbTitle { get; set; }

        [Display(Name = "Include In SiteMap")]
        public bool IncludeInSitemap { get; set; }

        public MetaTagsModel MetaTags { get; set; }

        public string Content { get; set; }

        [Display(Name = "Last Reviewed")]
        public DateTime LastReviewed { get; set; }

        [UrlPath]
        [LowerCase]
        public string[] AlternativeNames { get; set; }
    }
}
