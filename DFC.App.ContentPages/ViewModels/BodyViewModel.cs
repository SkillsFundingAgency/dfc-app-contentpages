using Microsoft.AspNetCore.Html;

namespace DFC.App.ContentPages.ViewModels
{
    public class BodyViewModel
    {
        public HtmlString Content { get; set; } = new HtmlString("Unknown Help content");
    }
}
