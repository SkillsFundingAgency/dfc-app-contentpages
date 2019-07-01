using Microsoft.AspNetCore.Html;

namespace DFC.App.Help.ViewModels
{
    public class BodyViewModel 
    {
        public HtmlString Content { get; set; } = new HtmlString("Unknown Help content");
    }
}
