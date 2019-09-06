using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using QuickStart.ViewModels;

namespace QuickStart.Views
{
    public sealed partial class ViewAView : UserControl
    {
        public ViewAView()
        {
            this.InitializeComponent();
        }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is ViewAViewModel vm)
            {
                await vm.ShowDialogAsync();
            }
        }
    }
}
