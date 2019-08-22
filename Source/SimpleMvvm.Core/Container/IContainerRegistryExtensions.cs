using System;

namespace SimpleMvvm.Core.Container
{
    public static partial class IContainerRegistryExtensions
    {
        public static void RegisterInstance<TInterface>(this IContainerRegistry containerRegistry, TInterface instance)
        {
            containerRegistry.RegisterInstance(typeof(TInterface), instance);
        }

        public static void RegisterSingleton(this IContainerRegistry containerRegistry, Type type)
        {
            containerRegistry.RegisterSingleton(type, type);
        }

        public static void RegisterSingleton<TFrom, TTo>(this IContainerRegistry containerRegistry) where TTo : TFrom
        {
            containerRegistry.RegisterSingleton(typeof(TFrom), typeof(TTo));
        }

        public static void RegisterSingleton<T>(this IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton(typeof(T));
        }

        public static void Register(this IContainerRegistry containerRegistry, Type type)
        {
            containerRegistry.Register(type, type);
        }

        public static void Register<T>(this IContainerRegistry containerRegistry)
        {
            containerRegistry.Register(typeof(T));
        }

        public static void Register(this IContainerRegistry containerRegistry, Type type, string name)
        {
            containerRegistry.Register(type, type, name);
        }

        public static void Register<T>(this IContainerRegistry containerRegistry, string name)
        {
            containerRegistry.Register(typeof(T), name);
        }

        public static void Register<TFrom, TTo>(this IContainerRegistry containerRegistry) where TTo : TFrom
        {
            containerRegistry.Register(typeof(TFrom), typeof(TTo));
        }

        public static void Register<TFrom, TTo>(this IContainerRegistry containerRegistry, string name) where TTo : TFrom
        {
            containerRegistry.Register(typeof(TFrom), typeof(TTo), name);
        }
        public static void Register(IContainerRegistry container, string key, Type view, Type viewModel)
        {
            if (viewModel != null)
            {
                container.Register(viewModel);
                ViewModelLocationProvider.Register(view.ToString(), viewModel);
            }
            PageRegistry.Register(key, (view, viewModel));
        }

        public static void RegisterView<TView, TViewModel>(this IContainerRegistry registry)
            => Register(registry, typeof(TView).Name, typeof(TView), typeof(TViewModel));
        public static void RegisterView<TView, TViewModel>(this IContainerRegistry registry, string key)
            => Register(registry, key, typeof(TView), typeof(TViewModel));
        public static void RegisterView<TView>(this IContainerRegistry registry)
            => Register(registry, typeof(TView).Name, typeof(TView), null);
        public static void RegisterView<TView>(this IContainerRegistry registry, string key)
            => Register(registry, key, typeof(TView), null);

        public static void RegisterForNavigation<TView, TViewModel>(this IContainerRegistry registry)
            => Register(registry, typeof(TView).Name, typeof(TView), typeof(TViewModel));
        public static void RegisterForNavigation<TView, TViewModel>(this IContainerRegistry registry, string key)
            => Register(registry, key, typeof(TView), typeof(TViewModel));
        public static void RegisterForNavigation<TView>(this IContainerRegistry registry)
            => Register(registry, typeof(TView).Name, typeof(TView), null);
        public static void RegisterForNavigation<TView>(this IContainerRegistry registry, string key)
            => Register(registry, key, typeof(TView), null);

        public static void RegisterViewModel() { }

    }
}