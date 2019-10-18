# Universal Prism

UniversalPrism is a framework for building loosely coupled, maintainable, and testable XAML applications in Windows 10 UWP.

After removal of `Prism.UWP` from `PrismLibrary`, I decided to fill this gap with `UniversalPrism` which not only replaces `Prism.UWP` but also adds `region` support to it, which is my favorite feature of `Prism.Wpf`.

I hope we can add this to original `Prism` when the time comes.

## Documentaion

`UniversalPrism` is a fork of `Prism` with footprints of `Template10`, so to understand how it works you can read `Prism`'s documents.

## Installation
    Install-Package UniversalPrism

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
            //register your types here to the container
            //example :
            //containerRegister.RegisterType<IMyService, MyService>();
        }

        protected override DependencyObject CreateShell()
        {
            var rm = Container.Resolve<RegionManager>();
            if (!(Window.Current.Content is ContentControl contentControl))
            {
                // Create root region control
                contentControl = new ContentControl
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    HorizontalContentAlignment = HorizontalAlignment.Stretch,
                    VerticalContentAlignment = VerticalAlignment.Stretch
                };
            }
            // remove root region if exists
            if (rm.Regions.ContainsRegionWithName("Root"))
            {
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
            // Navigate to your main view
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


## UniversalPrism.Interactivity

Wanna show Content Dialogs? Check out [UniversalPrism.Interactivity](https://github.com/HesamKashefi/UniversalPrism/tree/master/Source/UniversalPrism.Interactivity)

## Questions?
For general questions you can try [StackOverflow](www.stackoverflow.com) with `prism` + `universalprism` tags.

For bugs and feature request try [creating a new Issue or send a Pull Request](https://github.com/HesamKashefi/UniversalPrism/pulls).
