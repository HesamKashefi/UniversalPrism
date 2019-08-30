using CommonServiceLocator;
using Prism.Events;
using SimpleMvvm.Core.Container;
using SimpleMvvm.Core.Mvvm;
using SimpleMvvm.View.Common;
using SimpleMvvm.View.Regions;
using SimpleMvvm.View.Regions.Adapters;
using SimpleMvvm.View.Regions.Behaviors;
using SimpleMvvm.View.Regions.Navigation;
using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace SimpleMvvm.View
{
    /// <summary>
    /// Base application class that provides a basic initialization sequence
    /// </summary>
    /// <remarks>
    /// This class must be overridden to provide application specific configuration.
    /// </remarks>
    public abstract partial class ApplicationBase : Application
    {
        private static readonly SemaphoreSlim StartSemaphore = new SemaphoreSlim(1, 1);
        IContainerExtension containerExtension;

        public ApplicationBase()
        {
            InitializeInternal();
        }

        /// <summary>
        /// The dependency injection container used to resolve objects
        /// </summary>
        public IContainerProvider Container => containerExtension;

        private async Task InternalStartAsync(StartArgs startArgs)
        {
            await StartSemaphore.WaitAsync();

            try
            {
                await OnStartAsync(startArgs);
            }
            finally
            {
                StartSemaphore.Release();
            }
        }

        /// <summary>
        /// Run the initialization process.
        /// </summary>
        void InitializeInternal()
        {
            ConfigureViewModelLocator();
            Initialize();
            OnInitialized();
        }

        /// <summary>
        /// Configures the <see cref="SimpleMvvm.View.Mvvm.ViewModelLocator"/> used by Prism.
        /// </summary>
        protected virtual void ConfigureViewModelLocator()
        {
            ViewModelLocationProvider.SetDefaultViewModelFactory((view, type) => Container.Resolve(type));
        }

        /// <summary>
        /// Runs the initialization sequence to configure the Prism application.
        /// </summary>
        public virtual void Initialize()
        {
            #region IOC Container
            containerExtension = CreateContainerExtension();
            RegisterRequiredTypes(containerExtension);
            RegisterTypes(containerExtension);
            containerExtension.FinalizeExtension();
            #endregion

            ConfigureServiceLocator();

            #region Region Adapters
            var regionAdapterMappings = containerExtension.Resolve<RegionAdapterMappings>();
            ConfigureRegionAdapterMappings(regionAdapterMappings);
            #endregion

            #region Region Behaviors
            var defaultRegionBehaviors = containerExtension.Resolve<IRegionBehaviorFactory>();
            ConfigureDefaultRegionBehaviors(defaultRegionBehaviors);
            #endregion

            RegisterFrameworkExceptionTypes();

            var appShell = CreateShell();
            if (appShell != null)
            {
                RegionManager.SetRegionManager(appShell, containerExtension.Resolve<IRegionManager>());
                RegionManager.UpdateRegions();
                InitializeShell(appShell);
            }
        }

        /// <summary>
        /// Creates the container used by Prism.
        /// </summary>
        /// <returns>The container</returns>
        protected abstract IContainerExtension CreateContainerExtension();

        /// <summary>
        /// Registers all types that are required by Prism to function with the container.
        /// </summary>
        /// <param name="containerRegistry"></param>
        protected virtual void RegisterRequiredTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance(containerExtension);
            //containerRegistry.RegisterSingleton<ILoggerFacade, TextLogger>();
            containerRegistry.RegisterSingleton<RegionAdapterMappings>();
            containerRegistry.RegisterSingleton<IRegionManager, RegionManager>();
            containerRegistry.RegisterSingleton<IEventAggregator, EventAggregator>();
            containerRegistry.RegisterSingleton<IRegionViewRegistry, RegionViewRegistry>();
            containerRegistry.RegisterSingleton<IRegionBehaviorFactory, RegionBehaviorFactory>();
            containerRegistry.Register<IRegionNavigationJournalEntry, RegionNavigationJournalEntry>();
            containerRegistry.Register<IRegionNavigationJournal, RegionNavigationJournal>();
            containerRegistry.Register<IRegionNavigationService, RegionNavigationService>();
        }

        /// <summary>
        /// Used to register types with the container that will be used by your application.
        /// </summary>
        protected abstract void RegisterTypes(IContainerRegistry containerRegistry);

        /// <summary>
        /// Configures the <see cref="IRegionBehaviorFactory"/>. 
        /// This will be the list of default behaviors that will be added to a region. 
        /// </summary>
        protected virtual void ConfigureDefaultRegionBehaviors(IRegionBehaviorFactory regionBehaviors)
        {
            if (regionBehaviors == null) return;

            regionBehaviors.AddIfMissing(BindRegionContextToDependencyObjectBehavior.BehaviorKey, typeof(BindRegionContextToDependencyObjectBehavior));
            regionBehaviors.AddIfMissing(RegionActiveAwareBehavior.BehaviorKey, typeof(RegionActiveAwareBehavior));
            regionBehaviors.AddIfMissing(SyncRegionContextWithHostBehavior.BehaviorKey, typeof(SyncRegionContextWithHostBehavior));
            regionBehaviors.AddIfMissing(RegionManagerRegistrationBehavior.BehaviorKey, typeof(RegionManagerRegistrationBehavior));
            regionBehaviors.AddIfMissing(RegionMemberLifetimeBehavior.BehaviorKey, typeof(RegionMemberLifetimeBehavior));
            regionBehaviors.AddIfMissing(ClearChildViewsRegionBehavior.BehaviorKey, typeof(ClearChildViewsRegionBehavior));
            regionBehaviors.AddIfMissing(AutoPopulateRegionBehavior.BehaviorKey, typeof(AutoPopulateRegionBehavior));
            regionBehaviors.AddIfMissing(IDestructibleRegionBehavior.BehaviorKey, typeof(IDestructibleRegionBehavior));
        }

        /// <summary>
        /// Configures the default region adapter mappings to use in the application, in order
        /// to adapt UI controls defined in XAML to use a region and register it automatically.
        /// May be overwritten in a derived class to add specific mappings required by the application.
        /// </summary>
        /// <returns>The <see cref="RegionAdapterMappings"/> instance containing all the mappings.</returns>
        protected virtual void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
            if (regionAdapterMappings != null)
            {
                regionAdapterMappings.RegisterMapping(typeof(Selector), containerExtension.Resolve<SelectorRegionAdapter>());
                regionAdapterMappings.RegisterMapping(typeof(ItemsControl), containerExtension.Resolve<ItemsControlRegionAdapter>());
                regionAdapterMappings.RegisterMapping(typeof(ContentControl), containerExtension.Resolve<ContentControlRegionAdapter>());
            }
        }

        /// <summary>
        /// Registers the <see cref="Type"/>s of the Exceptions that are not considered 
        /// root exceptions by the <see cref="ExceptionExtensions"/>.
        /// </summary>
        protected virtual void RegisterFrameworkExceptionTypes()
        {
            ExceptionExtensions.RegisterFrameworkExceptionType(typeof(ActivationException));
        }

        /// <summary>
        /// Creates the shell or main window of the application.
        /// </summary>
        /// <returns>The shell of the application.</returns>
        protected abstract DependencyObject CreateShell();

        /// <summary>
        /// Initializes the shell.
        /// </summary>
        protected virtual void InitializeShell(DependencyObject appShell)
        {
        }

        /// <summary>
        /// This will be called on app startup
        /// </summary>
        /// <param name="startArgs"></param>
        protected virtual Task OnStartAsync(StartArgs startArgs)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Contains actions that should occur last.
        /// </summary>
        protected virtual void OnInitialized()
        {
        }

        /// <summary>
        /// Configures the LocatorProvider for the <see cref="ServiceLocator" />.
        /// </summary>
        protected virtual void ConfigureServiceLocator()
        {
            ServiceLocator.SetLocatorProvider(() => containerExtension.Resolve<IServiceLocator>());
        }
    }
}
