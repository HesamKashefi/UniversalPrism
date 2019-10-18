using UniversalPrism.Core.Mvvm;

namespace QuickStart.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private string _title = "Main View";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
    }
}
