using System.Collections.Generic;
using System.Threading.Tasks;
using UniversalPrism.Core.Mvvm;
using UniversalPrism.Interactivity;

namespace QuickStart.ViewModels
{
    public class ViewBViewModel : ViewModelBase
    {
        private readonly IDialogService _dialogService;

        public ViewBViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }

        private string _title = "View B";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public async Task ShowDialogAsync()
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("Id", 1993);
            var result = await _dialogService.ShowDialogAsync("confirmation", parameters);
        }
    }
}
