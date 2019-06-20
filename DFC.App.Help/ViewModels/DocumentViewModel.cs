using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;

namespace DFC.App.Help.ViewModels
{
    public class DocumentViewModel
    {
        public BreadcrumbViewModel Breadcrumb { get; set; }

        public Guid? DocumentId { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public bool IncludeInSitemap { get; set; }

        public string Description { get; set; }

        public string Keywords { get; set; }

        public HtmlString Contents { get; set; }

        public DateTime LastReviewed { get; set; }

        public string[] Urls { get; set; }
    }
}
