using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace UniversalPrism.Interactivity
{
    /// <summary>
    /// A service to show dialogs
    /// </summary>
    public interface IDialogService
    {
        /// <summary>
        /// Shows a registered dialog with the specified name
        /// </summary>
        /// <param name="name">Name of the registered dialog</param>
        /// <returns>Result of the dialog</returns>
        Task<ContentDialogResult> ShowDialogAsync(string name);

        /// <summary>
        /// Shows a registered dialog with the specified name
        /// The parameters will be sent to dialog
        /// </summary>
        /// <param name="name">Name of the registered dialog</param>
        /// <param name="parameters">Extra parameters to send to a dialog that implements <see cref="INavigationAwareDialog"/></param>
        /// <returns>Result of the dialog</returns>
        Task<ContentDialogResult> ShowDialogAsync(string name, Dictionary<string, object> parameters);

        /// <summary>
        /// Shows a preconfigured dialog
        /// </summary>
        /// <param name="dialogArgs">Configuration of the dialog</param>
        /// <returns>Result of the dialog</returns>
        Task<ContentDialogResult> ShowDialogAsync(IDialogArgs dialogArgs);
    }
}
