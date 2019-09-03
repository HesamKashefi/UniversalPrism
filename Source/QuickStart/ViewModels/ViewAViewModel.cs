using UniversalPrism.Core.Mvvm;

namespace QuickStart.ViewModels
{
    public class ViewAViewModel : ViewModelBase
    {
        private string title = "View A";
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }
    }
}
