using System.Collections.Generic;
using UniversalPrism.Interactivity;
using Windows.UI.Xaml.Controls;

namespace QuickStart.Dialogs
{
    public sealed partial class ConfirmationDialog : ContentDialog, INavigationAwareDialog
    {
        public ConfirmationDialog()
        {
            this.InitializeComponent();
        }

        public void OnNavigatedTo(Dictionary<string, object> parameters)
        {
        }
    }
}
