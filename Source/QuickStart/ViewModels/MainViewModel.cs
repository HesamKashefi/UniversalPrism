using UniversalPrism.Core.Mvvm;

namespace QuickStart.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string title = "Main View";
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }
    }
}
