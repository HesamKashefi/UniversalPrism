using CommonServiceLocator;
using SimpleMvvm.Core.Container;
using SimpleMvvm.Unity;
using SimpleMvvm.View;
using SimpleMvvm.View.Regions.Navigation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace QuickStart
{
    sealed partial class App : ApplicationBase
    {
        public App()
        {
            this.InitializeComponent();
        }

        protected override IContainerExtension CreateContainerExtension()
        {
            return new UnityContainerExtension();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterRequiredTypes(containerRegistry);
            containerRegistry.RegisterSingleton<IRegionNavigationContentLoader, UnityRegionNavigationContentLoader>();
            containerRegistry.RegisterSingleton<IServiceLocator, UnityServiceLocatorAdapter>();
        }

        protected override DependencyObject CreateShell()
        {
            if (!(Window.Current.Content is Frame rootFrame))
            {
                rootFrame = new Frame();
            }
            Window.Current.Content = rootFrame;
            return rootFrame;
        }

        private DependencyObject shell;
        protected override void InitializeShell(DependencyObject shellObj)
        {
            base.InitializeShell(shellObj);
            shell = shellObj;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            (shell as Frame)?.Navigate(typeof(MainPage));
            Window.Current.Activate();
        }
    }
}
