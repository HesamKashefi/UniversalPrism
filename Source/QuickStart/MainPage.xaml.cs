using QuickStart.Views;
using UniversalPrism.Core.Container;
using UniversalPrism.View.Regions;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace QuickStart
{
    public sealed partial class MainPage : Page
    {
        private IContainerProvider container;

        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is IContainerProvider containerProvider)
            {
                this.container = containerProvider;
            }
        }

        private void MainPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var regionManager = container.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("Main", typeof(MainView));
        }
    }
}
