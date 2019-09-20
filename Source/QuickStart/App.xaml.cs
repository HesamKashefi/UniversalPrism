using System.Threading.Tasks;
using UniversalPrism.Unity;
using UniversalPrism.View;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using QuickStart.Dialogs;
using QuickStart.Pages;
using UniversalPrism.Core.Container;
using UniversalPrism.Interactivity;

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
            if (!(Window.Current.Content is Frame rootFrame))
            {
                rootFrame = new Frame();
            }
            return rootFrame;
        }

        protected override void InitializeShell(DependencyObject appShell)
        {
            base.InitializeShell(appShell);
            if (Shell is Frame rootFrame)
            {
                Window.Current.Content = rootFrame;
                //Pass the container to the main page
                rootFrame.Navigate(typeof(MainPage), Container);
            }
        }

        protected override Task OnStartAsync(StartArgs startArgs)
        {
            Window.Current.Activate();
            return Task.CompletedTask;
        }
    }
}
