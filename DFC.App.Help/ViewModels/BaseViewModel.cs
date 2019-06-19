using Microsoft.AspNetCore.Html;

namespace DFC.App.Help.ViewModels
{
    public abstract class BaseViewModel
    {
        public string Title { get; set; } = "Unknown Help title";

        public HtmlString Contents { get; set; } = new HtmlString("Unknown Help content");
    }
}
