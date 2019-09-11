using System;
using System.Collections.Generic;
using UniversalPrism.Core.Container;
using Windows.UI.Xaml.Controls;

namespace UniversalPrism.Interactivity
{
    public static class DialogContainerExtensions
    {
        /// <summary>
        /// Dictionary of the registered dialogs to avoid duplication
        /// </summary>
        private static readonly IDictionary<string, Type> RegisteredDialogs = new Dictionary<string, Type>();

        /// <summary>
        /// Get Dialog type for the specified dialog name
        /// </summary>
        /// <param name="name">Name of the dialog</param>
        /// <returns>Type of the dialog</returns>
        public static Type GetType(string name)
        {
            return RegisteredDialogs[name];
        }

        /// <summary>
        /// Registers <see cref="IDialogService"/>
        /// </summary>
        /// <param name="containerRegistry">Container Registry</param>
        public static void AddDialogServices(this IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IDialogService, DialogService>();
        }

        /// <summary>
        /// Registers the <typeparamref name="TContentDialog"/> with <paramref name="name"/> in the <see cref="IDialogService"/>
        /// </summary>
        /// <typeparam name="TContentDialog">Type of the content dialog</typeparam>
        /// <param name="containerRegistry">Container Registry</param>
        /// <param name="name">Name of the dialog</param>
        public static void RegisterContentDialog<TContentDialog>(this IContainerRegistry containerRegistry, string name) where TContentDialog : ContentDialog
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name), "Dialog name can't be empty or null");
            }

            if (RegisteredDialogs.ContainsKey(name))
            {
                if (RegisteredDialogs[name] != typeof(TContentDialog))
                    throw new InvalidOperationException($"Dialog with name:{name} is already registered with type:{RegisteredDialogs[name].Name}");
                return;
            }

            RegisteredDialogs.Add(name, typeof(TContentDialog));
            containerRegistry.Register<TContentDialog>(name);
        }
    }
}
