using QuickStart.Dialogs;
using QuickStart.Views;
using System.Threading.Tasks;
using UniversalPrism.Core.Container;
using UniversalPrism.Interactivity;
using UniversalPrism.Unity;
using UniversalPrism.View;
using UniversalPrism.View.Regions;
using UniversalPrism.View.Regions.Navigation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace QuickStart
{
    sealed partial class App : UnityApplicationBase
    {
        public App()
        {
            InitializeComponent();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterTypes(containerRegistry);
            containerRegistry.AddDialogServices();
            containerRegistry.RegisterContentDialog<ConfirmationDialog>("confirmation");
        }

        protected override DependencyObject CreateShell()
        {
            var rm = Container.Resolve<RegionManager>();
            if (!(Window.Current.Content is ContentControl contentControl))
            {
                contentControl = new ContentControl
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    HorizontalContentAlignment = HorizontalAlignment.Stretch,
                    VerticalContentAlignment = VerticalAlignment.Stretch
                };
            }

            if (rm.Regions.ContainsRegionWithName("Root"))
            {
                // remove causes to remove inner regions that make resolves recreation of regions
                rm.Regions.Remove("Root");
            }
            RegionManager.SetRegionName(contentControl, "Root");
            return contentControl;
        }

        protected override void InitializeShell(DependencyObject appShell)
        {
            base.InitializeShell(appShell);
            if (appShell is ContentControl contentControl)
            {
                Window.Current.Content = contentControl;
            }
        }

        protected override Task OnStartAsync(StartArgs startArgs)
        {
            var regionManager = Container.Resolve<IRegionManager>();
            regionManager.AddToRegion("Root", Container.Resolve<MainView>());
            regionManager.RequestNavigate("Root", "MainView", new NavigationParameters
            {
                { "StartArgs", startArgs }
            });

            Window.Current.Activate();
            return Task.CompletedTask;
        }
    }
}
