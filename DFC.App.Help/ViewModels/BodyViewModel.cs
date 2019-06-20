using Microsoft.AspNetCore.Html;

namespace DFC.App.Help.ViewModels
{
    public class BodyViewModel : BaseViewModel
    {
        public HtmlString Contents { get; set; } = new HtmlString("Unknown Help content");
    }
}
