# Universal Prism

UniversalPrism is a framework for building loosely coupled, maintainable, and testable XAML applications in Windows 10 UWP.

After removal of `Prism.UWP` from `PrismLibrary`, I decided to fill this gap with `UniversalPrism` which not only replaces `Prism.UWP` but also adds `region` support to it, which is my favorite feature of `Prism.Wpf`.

I hope we can add this to original `Prism` when the time comes.

## Documentaion

`UniversalPrism` is a fork of `Prism` with footprints of `Template10`, so to understand how it works you can read `Prism`'s documents.

## Installation
    Install-Package UniversalPrism -Version 1.0.0-preview4

## Bootstrapping your app
Update your `app.xaml.cs` file
    
    sealed partial class App : UniversalPrism.Unity.UnityApplicationBase
    {
        public App()
        {
            InitializeComponent();
        }

        protected override void RegisterTypes(
            UniversalPrism.Core.Container.IContainerRegistry containerRegistry)
        {
            base.RegisterTypes(containerRegistry);
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
            var rootFrame = this.Shell as Frame;
            Window.Current.Content = rootFrame;

            //send container to the main page
            rootFrame.Navigate(typeof(MainPage), Container);

            Window.Current.Activate();
            return Task.CompletedTask;
        }
    }

## Register your views

After sending the `container` from last step to MainPage, you can get in your MainPage's `OnNavigatedTo` method and register your pages for navigation:
    
    public sealed partial class MainPage : Page
    {
        private  UniversalPrism.Core.Container.IContainerProvider container;

        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is  UniversalPrism.Core.Container.IContainerProvider containerProvider)
            {
                this.container = containerProvider;
            }
        }

        private void MainPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var regionManager = container.Resolve<UniversalPrism.View.Regions.IRegionManager>();
            regionManager.RegisterViewWithRegion("Main", typeof(MainView));
        }
    }

## UniversalPrism.Interactivity

Wanna show Content Dialogs? Check out [UniversalPrism.Interactivity](https://github.com/HesamKashefi/UniversalPrism/tree/master/Source/UniversalPrism.Interactivity)

## Questions?
For general questions you can try [StackOverflow](www.stackoverflow.com) with `prism` + `universalprism` tags.

For bugs and feature request try [creating a new Issue or send a Pull Request](https://github.com/HesamKashefi/UniversalPrism/pulls).
