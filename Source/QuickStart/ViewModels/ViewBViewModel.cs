using System.Threading.Tasks;
using UniversalPrism.Core.Mvvm;
using UniversalPrism.Interactivity;

namespace QuickStart.ViewModels
{
    public class ViewBViewModel : ViewModelBase
    {
        private readonly IDialogService dialogService;

        public ViewBViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;
        }

        private string title = "View B";
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public async Task ShowDialogAsync()
        {
            var result = await dialogService.ShowDialogAsync("confirmation");
        }
    }
}
