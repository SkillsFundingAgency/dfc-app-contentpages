using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.App.Help.Models
{
    public class BreadcrumbViewModel
    {
        public IEnumerable<string> Paths { get; set; }
        public string ThisLocation { get; set; }
    }
}
