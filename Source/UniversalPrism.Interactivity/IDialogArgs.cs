using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace UniversalPrism.Interactivity
{
    /// <summary>
    /// Properties of a <see cref="ContentDialog"/> that can be set
    /// </summary>
    public interface IDialogArgs
    {
        ICommand PrimaryButtonCommand { get; set; }
        object PrimaryButtonCommandParameter { get; set; }
        string PrimaryButtonText { get; set; }

        ICommand SecondaryButtonCommand { get; set; }
        object SecondaryButtonCommandParameter { get; set; }
        string SecondaryButtonText { get; set; }

        ICommand CloseButtonCommand { get; set; }
        object CloseButtonCommandParameter { get; set; }
        string CloseButtonText { get; set; }

        /// <summary>
        /// Title of the <see cref="ContentDialog"/>
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// Content of the <see cref="ContentDialog"/>
        /// </summary>
        object Content { get; set; }
    }
}