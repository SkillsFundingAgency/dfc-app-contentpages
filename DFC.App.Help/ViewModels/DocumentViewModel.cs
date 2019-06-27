using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Html;

namespace DFC.App.Help.ViewModels
{
    public class DocumentViewModel
    {
        public BreadcrumbViewModel Breadcrumb { get; set; }

        public Guid? DocumentId { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        [Display(Name = "Include In SiteMap")]
        public bool IncludeInSitemap { get; set; }

        public string Description { get; set; }

        public string Keywords { get; set; }

        public HtmlString Content { get; set; }

        [Display(Name = "Last Reviewed")]
        public DateTime LastReviewed { get; set; }

        [Display(Name = "Last Published")]
        public DateTime? LastPublished { get; set; }

        public string[] Urls { get; set; }
    }
}
