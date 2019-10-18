using QuickStart.Views;
using UniversalPrism.Core.Mvvm;
using UniversalPrism.View.Regions;

namespace QuickStart.ViewModels
{
    public class MainViewModel : BindableBase
    {
        public MainViewModel(IRegionManager regionManager)
        {
            regionManager.RegisterViewWithRegion("SubRegion", typeof(ViewAView));
            regionManager.RegisterViewWithRegion("SubRegion", typeof(ViewBView));
        }

        private string _title = "Main View";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
    }
}
