using UniversalPrism.Core.Mvvm;

namespace QuickStart.ViewModels
{
    public class ViewBViewModel : ViewModelBase
    {
        private string title = "View B";
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }
    }
}
