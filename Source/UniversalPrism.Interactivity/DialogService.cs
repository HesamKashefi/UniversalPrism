using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using UniversalPrism.Core.Container;
using System.Collections.Generic;

namespace UniversalPrism.Interactivity
{
    public class DialogService : IDialogService
    {
        #region Dependencies
        private readonly IContainerExtension container; 
        #endregion

        #region Ctor
        public DialogService(IContainerExtension container)
        {
            this.container = container;
        }
        #endregion

        #region Implementation Of IDialogService
        public async Task<ContentDialogResult> ShowDialogAsync(string name)
        {
            var dialog = GetDialog(name);
            return await dialog.ShowAsync();
        }

        public async Task<ContentDialogResult> ShowDialogAsync(string name, Dictionary<string, object> parameters)
        {
            var dialog = GetDialog(name);
            if (dialog is IDataAwareDialog theDialog)
            {
                theDialog.OnNavigatedTo(parameters);
            }
            else
            {
                throw new InvalidOperationException($"The dialog with name : '{name}', does not implement '{nameof(IDataAwareDialog)}'");
            }
            return await dialog.ShowAsync();
        }

        public async Task<ContentDialogResult> ShowDialogAsync(string name, object dataContext, Dictionary<string, object> parameters)
        {
            var dialog = GetDialog(name);
            if(dialog is IDataAwareDialog theDialog)
            {
                theDialog.DataContext = dataContext;
                theDialog.OnNavigatedTo(parameters);
            }
            else
            {
                throw new InvalidOperationException($"The dialog with name : '{name}', does not implement '{nameof(IDataAwareDialog)}'");
            }
            return await dialog.ShowAsync();
        }

        public async Task<ContentDialogResult> ShowDialogAsync(DialogArgs dialogArgs)
        {
            var dialog = new ContentDialog
            {
                Title = dialogArgs.Title,
                Content = dialogArgs.Content,

                PrimaryButtonCommand = dialogArgs.PrimaryButtonCommand,
                PrimaryButtonCommandParameter = dialogArgs.PrimaryButtonCommandParameter,

                SecondaryButtonCommand = dialogArgs.SecondaryButtonCommand,
                SecondaryButtonCommandParameter = dialogArgs.SecondaryButtonCommandParameter,

                CloseButtonCommand = dialogArgs.CloseButtonCommand,
                CloseButtonCommandParameter = dialogArgs.CloseButtonCommandParameter
            };

            if (!string.IsNullOrWhiteSpace(dialogArgs.PrimaryButtonText))
            {
                dialog.PrimaryButtonText = dialogArgs.PrimaryButtonText;
            }

            if (!string.IsNullOrWhiteSpace(dialogArgs.SecondaryButtonText))
            {
                dialog.SecondaryButtonText = dialogArgs.SecondaryButtonText;
            }

            if (!string.IsNullOrWhiteSpace(dialogArgs.CloseButtonText))
            {
                dialog.CloseButtonText = dialogArgs.CloseButtonText;
            }

            return await dialog.ShowAsync();
        }
        #endregion

        private ContentDialog GetDialog(string name)
        {
            var type = DialogContainerExtensions.GetType(name);
            return (ContentDialog)container.Resolve(type, name);
        }
    }
}
