using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.App.Help.Models.Cosmos
{
	public class HelpPageModel
	{
        [Newtonsoft.Json.JsonProperty(PropertyName = "id")]
        public Guid? DocumentId { get; set; }

		public string Name { get; set; }

		public string Title { get; set; }

		public bool IncludeInSitemap { get; set; }

		public MetatagsModel Metatags { get; set; }

		public string Contents { get; set; }

		public DateTime LastReviewed { get; set; }

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
