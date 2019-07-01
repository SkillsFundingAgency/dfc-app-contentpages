using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Newtonsoft.Json;

namespace DFC.App.Help.Models.Cosmos
{
    public class HelpPageModel : IValidatableObject
    {
        [JsonProperty(PropertyName = "id")]
        [Required]
        public Guid DocumentId { get; set; }

        [Required]
        [Display(Name = "Canonical Name")]
        public string CanonicalName { get; set; }

        [Display(Name = "Breadcrumb Title")]
        [Required]
        public string BreadcrumbTitle { get; set; }

        [Display(Name = "Include In SiteMap")]
        public bool IncludeInSitemap { get; set; }

        public MetaTagsModel MetaTags { get; set; }

        [Required]
        public string Content { get; set; }

        [Display(Name = "Last Reviewed")]
        [Required]
        public DateTime LastReviewed { get; set; }

        public string[] AlternativeNames { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();

            if (!string.IsNullOrWhiteSpace(CanonicalName) && CanonicalName.ToLower() != CanonicalName)
            {
                result.Add(new ValidationResult("The field name must be in lower case", new string[] { nameof(CanonicalName) }));
            }

            if (AlternativeNames.Any(x => x.ToLower() != x))
            {
                result.Add(new ValidationResult("The field url must only contains values that are in lower case ", new string[] { nameof(AlternativeNames) }));
            }

            return result;
        }
    }
}
