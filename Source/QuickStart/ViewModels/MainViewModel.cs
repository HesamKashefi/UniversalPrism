using UniversalPrism.Core.Mvvm;

namespace QuickStart.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string _title = "Main View";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
    }
}
