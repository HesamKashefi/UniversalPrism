using System;
using Windows.UI.Xaml;

namespace UniversalPrism.View.Common
{
    /// <summary>
    /// Provides a set of Helper methods for interacting with View Models
    /// </summary>
    public static class MvvmHelpers
    {
        /// <summary>
        /// Executes <paramref name="action"/> on the <paramref name="view"/> and it's ViewModel (object in the DataContext of type T)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="view"></param>
        /// <param name="action"></param>
        public static void ViewAndViewModelAction<T>(object view, Action<T> action) where T : class
        {
            if (view is T viewAsT)
                action(viewAsT);
            if (view is FrameworkElement element)
            {
                if (element.DataContext is T viewModelAsT)
                {
                    action(viewModelAsT);
                }
            }
        }

        /// <summary>
        /// Returns the View or it's ViewModel if they are of type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Type of the Implementer</typeparam>
        /// <param name="view"></param>
        /// <returns></returns>
        public static T GetImplementerFromViewOrViewModel<T>(object view) where T : class
        {
            if (view is T viewAsT)
            {
                return viewAsT;
            }

            if (view is FrameworkElement element)
            {
                var vmAsT = element.DataContext as T;
                return vmAsT;
            }

            return null;
        }
    }
}
