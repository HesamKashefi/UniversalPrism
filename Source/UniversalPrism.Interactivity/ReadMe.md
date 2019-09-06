# Universal Prism's Interactivity 

This library adds ability to show dialogs from View Models.

## Register Services

In the `App.Xaml.cs` :

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        base.RegisterTypes(containerRegistry);
                
        // Register dialog service
        containerRegistry.Register<IDialogService, DialogService>();

        //Register your custom dialog
        containerRegistry.RegisterContentDialog<ConfirmationDialog>("confirmation");
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

You can simply call `dialogService.ShowDialogAsync` with the regitered dialog name

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