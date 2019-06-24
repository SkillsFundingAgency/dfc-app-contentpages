using System.Collections.Generic;
using Microsoft.AspNetCore.Html;

namespace DFC.App.Help.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<IndexDocumentViewModel> Documents { get; set; }
    }
}
