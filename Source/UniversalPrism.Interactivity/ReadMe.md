# Universal Prism's Interactivity 

This library adds ability to show dialogs from View Models.

## Installation

    Install-Package UniversalPrism.Interactivity -Version 1.0.0-preview6

## Register Services

In the `App.Xaml.cs` :

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
		...

        // Register dialog service
        containerRegistry.AddDialogServices();
    }
----

### Show a dialog

    public class MyViewModel : ViewModelBase
    {
        private readonly IDialogService dialogService;

        public MyViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;
        }

        public async Task ShowDialogAsync()
        {
            var result = await dialogService.ShowDialogAsync(new DialogArgs
            {
                Title = "Load",
                Content = "Dialog request succeeded",
                PrimaryButtonText = "Ok",
                SecondaryButtonText = "Cancel"
            });
        }
    }

----
### Show a custom dialog


In the `App.Xaml.cs` regiter your content dialog :

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
		...

        //Register your custom dialog
        containerRegistry.RegisterContentDialog<ConfirmationDialog>("confirmation");
    }

Then you can simply call `dialogService.ShowDialogAsync` with the regitered dialog name

    public class MyViewModel : ViewModelBase
    {
        private readonly IDialogService dialogService;

        public MyViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;
        }
        
        public async Task ShowDialogAsync()
        {
            var result = await dialogService.ShowDialogAsync("confirmation");
        }
    }

You can also send parameters with `ShowDialogAsync` overloads to the dialog if you implement `INavigationAwareDialog` in your dialog or it's DataContext.