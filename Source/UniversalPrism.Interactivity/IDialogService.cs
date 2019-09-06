using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace UniversalPrism.Interactivity
{
    public interface IDialogService
    {
        Task<ContentDialogResult> ShowDialogAsync(string name);
        Task<ContentDialogResult> ShowDialogAsync(DialogArgs dialogArgs);
    }
}
