using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;

namespace DFC.App.Help.ViewModels
{
    public class DocumentViewModel
    {
        public string Title { get; set; }
        public HtmlString Contents { get; set; }
    }
}
