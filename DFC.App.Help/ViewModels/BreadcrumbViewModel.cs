using System.Collections.Generic;

namespace DFC.App.Help.ViewModels
{
    public class BreadcrumbViewModel : BaseViewModel
    {
        public IList<BreadcrumbPathViewModel> Paths { get; set; }
    }
}
