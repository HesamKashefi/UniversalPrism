using System.Threading.Tasks;
using UniversalPrism.Core.Navigation;

namespace UniversalPrism.Core.Mvvm
{
    public abstract class ViewModelBase : BindableBase,
        IConfirmNavigation,
        IConfirmNavigationAsync,
        IDestructible,
        INavigatedAware,
        INavigatedAwareAsync,
        INavigatingAware,
        INavigatingAwareAsync
    {
        public virtual bool CanNavigate(INavigationParameters parameters) => true;

        public Task<bool> CanNavigateAsync(INavigationParameters parameters) =>  Task.FromResult(true);

        public virtual void Destroy() { /* empty */ }

        public virtual void OnNavigatedFrom(INavigationParameters parameters) { /* empty */ }

        public virtual void OnNavigatedTo(INavigationParameters parameters) { /* empty */ }

        public virtual Task OnNavigatedToAsync(INavigationParameters parameters) => Task.CompletedTask;

        public virtual void OnNavigatingTo(INavigationParameters parameters) { /* empty */ }

        public virtual Task OnNavigatingToAsync(INavigationParameters parameters) =>  Task.CompletedTask;
    }
}
