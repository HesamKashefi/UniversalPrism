using System;
using System.Collections.Generic;
using UniversalPrism.Core.Container;
using Windows.UI.Xaml.Controls;

namespace UniversalPrism.Interactivity
{
    public static class DialogContainerExtensions
    {
        private static readonly IDictionary<string, Type> RegisteredDialogs = new Dictionary<string, Type>();

        public static Type GetType(string name)
        {
            return RegisteredDialogs[name];
        }

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
