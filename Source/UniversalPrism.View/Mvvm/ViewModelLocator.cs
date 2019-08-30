using Windows.UI.Xaml;
using UniversalPrism.Core.Mvvm;

namespace UniversalPrism.View.Mvvm
{
    /// <summary>
    /// This class defines the attached property and related change handler that calls the ViewModelLocator in UniversalPrism.Mvvm.
    /// </summary>
    public static class ViewModelLocator
    {
        #region Dependency Properties
        /// <summary>
        /// The AutoWireViewModel attached property.
        /// </summary>
        public static DependencyProperty AutoWireViewModelProperty = 
            DependencyProperty.RegisterAttached("AutoWireViewModel", typeof(bool), typeof(ViewModelLocator),
                new PropertyMetadata(false, AutoWireViewModelChanged));

        public static bool GetAutoWireViewModel(DependencyObject obj)
        {
            return (bool)obj.GetValue(AutoWireViewModelProperty);
        }

        public static void SetAutoWireViewModel(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoWireViewModelProperty, value);
        }

        private static void AutoWireViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                if ((bool)e.NewValue)
                {
                    ViewModelLocationProvider.AutoWireViewModelChanged(d, Bind);
                }
            }
        }
        #endregion

        /// <summary>
        /// Sets the DataContext of a View
        /// </summary>
        /// <param name="view">The View to set the DataContext on</param>
        /// <param name="viewModel">The object to use as the DataContext for the View</param>
        static void Bind(object view, object viewModel)
        {
            if (view is FrameworkElement element)
                element.DataContext = viewModel;
        }
    }
}