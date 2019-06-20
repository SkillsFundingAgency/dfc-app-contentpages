using Newtonsoft.Json;

namespace DFC.App.Help.ViewModels
{
    public abstract class BaseViewModel
    {
        [JsonIgnore]
        public string Title { get; set; } = "Unknown Help title";
    }
}
