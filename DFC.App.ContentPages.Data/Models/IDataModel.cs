﻿using DFC.App.ContentPages.Data.Attributes;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.ContentPages.Data.Models
{
    public interface IDataModel
    {
        [Guid]
        [Required]
        [JsonProperty(PropertyName = "id")]
        Guid DocumentId { get; set; }

        [JsonProperty(PropertyName = "_etag")]
        string Etag { get; set; }

        [Required]
        string PartitionKey { get; }
    }
}
