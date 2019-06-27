using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace DFC.App.Help.Models.Cosmos
{
    public class HelpPageModel
    {
        [JsonProperty(PropertyName = "id")]
        public Guid DocumentId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Title { get; set; }

        [Display(Name = "Page Title")]
        public string PageTitle { get; set; }

        [Display(Name = "Include In SiteMap")]
        public bool IncludeInSitemap { get; set; }

        public MetaTagsModel MetaTags { get; set; }

        public string Content { get; set; }

        [Display(Name = "Last Reviewed")]
        public DateTime LastReviewed { get; set; }

        [Display(Name = "Last Published")]
        public DateTime? LastPublished { get; set; }

        public string[] Urls { get; set; }
    }
}

//TODO: ian: delete the following

/*
 * 
 * 
		{
			name: url-leaf-eg-termsandconditions, //default url used in canonical url metatag
			title: Train driver,
			includeInSitemap: true | false, //Default should be false
			metatags: 
			{
				description: desc,
				keywords: keywords
			},
			contents:["html string", "html string"],
			lastReviewd: "published | modified date from sitefinity",
			urls: ["additional urls", "e.g. termsandconditions renamed to terms-and-conditions"]
		}


		Help respnses
		-------------

		get document from Cosmos where urls.contains(article name)

		body response with article.contents

		meta response with article.metatags



		Sitemap respnses
		----------------

		get all documents from Cosmos where includeInSitemap == true 
			output each item from urls[]


		end.

 * 
 * 
 */
