using System.Threading.Tasks;
using UniversalPrism.Core.Mvvm;
using UniversalPrism.Interactivity;

namespace QuickStart.ViewModels
{
    public class ViewAViewModel : ViewModelBase
    {
        private readonly IDialogService _dialogService;

        public ViewAViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }

        private string _title = "View A";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public async Task ShowDialogAsync()
        {
            var result = await _dialogService.ShowDialogAsync(new DialogArgs
            {
                Title = "Load",
                Content = "Dialog request succeeded",
                PrimaryButtonText = "Ok",
                SecondaryButtonText = "Cancel"
            });
        }
    }
}
