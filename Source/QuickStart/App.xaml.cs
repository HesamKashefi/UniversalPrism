using System.Threading.Tasks;
using UniversalPrism.Unity;
using UniversalPrism.View;
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

        protected override DependencyObject CreateShell()
        {
            if (!(Window.Current.Content is Frame rootFrame))
            {
                rootFrame = new Frame();
            }
            return rootFrame;
        }

        protected override Task OnStartAsync(StartArgs startArgs)
        {
            var rootFrame = Shell as Frame;
            Window.Current.Content = rootFrame;

            //send container to the main page
            rootFrame.Navigate(typeof(MainPage), Container);

            Window.Current.Activate();
            return Task.CompletedTask;
        }
    }
}
