using SimpleMvvm.Unity;
using SimpleMvvm.View;
using System.Threading.Tasks;
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
            if (!(Window.Current.Content is Frame rootFrame))
            {
                rootFrame = new Frame();
            }
            rootFrame.Navigate(typeof(MainPage));
            Window.Current.Content = rootFrame;
            Window.Current.Activate();

            return Task.CompletedTask;
        }
    }
}
