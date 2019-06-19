namespace DFC.App.Help.ViewModels
{
    public class BreadcrumbPathViewModel
    {
        public string Route { get; set; }
        public string Title { get; set; }
        public bool IsLastItem { get; set; } = false;
    }
}
