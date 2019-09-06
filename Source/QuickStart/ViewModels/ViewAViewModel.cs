using System.Threading.Tasks;
using UniversalPrism.Core.Mvvm;
using UniversalPrism.Interactivity;

namespace QuickStart.ViewModels
{
    public class ViewAViewModel : ViewModelBase
    {
        private readonly IDialogService dialogService;

        public ViewAViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;
        }

        private string title = "View A";
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
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
}
