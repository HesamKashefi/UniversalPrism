using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using CommonServiceLocator;
using UniversalPrism.Core.Container;
using UniversalPrism.Core.Mvvm;
using UniversalPrism.View.Common;
using UniversalPrism.View.Logging;
using UniversalPrism.View.Mvvm;
using UniversalPrism.View.Regions;
using UniversalPrism.View.Regions.Adapters;
using UniversalPrism.View.Regions.Behaviors;
using UniversalPrism.View.Regions.Navigation;

namespace UniversalPrism.View
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
        private IContainerExtension _containerExtension;

        public DependencyObject Shell { get; set; }

        public ApplicationBase()
        {
            InitializeInternal();
        }

        /// <summary>
        /// The dependency injection container used to resolve objects
        /// </summary>
        public IContainerProvider Container => _containerExtension;

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
        /// Configures the <see cref="ViewModelLocator"/> used by UniversalPrism.
        /// </summary>
        protected virtual void ConfigureViewModelLocator()
        {
            ViewModelLocationProvider.SetDefaultViewModelFactory((view, type) => Container.Resolve(type));
        }

        /// <summary>
        /// Runs the initialization sequence to configure the UniversalPrism application.
        /// </summary>
        public virtual void Initialize()
        {
            #region IOC Container
            _containerExtension = CreateContainerExtension();
            RegisterRequiredTypes(_containerExtension);
            RegisterTypes(_containerExtension);
            _containerExtension.FinalizeExtension();
            #endregion

            ConfigureServiceLocator();

            #region Region Adapters
            var regionAdapterMappings = _containerExtension.Resolve<RegionAdapterMappings>();
            ConfigureRegionAdapterMappings(regionAdapterMappings);
            #endregion

            #region Region Behaviors
            var defaultRegionBehaviors = _containerExtension.Resolve<IRegionBehaviorFactory>();
            ConfigureDefaultRegionBehaviors(defaultRegionBehaviors);
            #endregion

            RegisterFrameworkExceptionTypes();

            var appShell = CreateShell();
            if (appShell != null)
            {
                RegionManager.SetRegionManager(appShell, _containerExtension.Resolve<IRegionManager>());
                RegionManager.UpdateRegions();
                InitializeShell(appShell);
            }
        }

        /// <summary>
        /// Creates the container used by UniversalPrism.
        /// </summary>
        /// <returns>The container</returns>
        protected abstract IContainerExtension CreateContainerExtension();

        /// <summary>
        /// Registers all types that are required by UniversalPrism to function with the container.
        /// </summary>
        /// <param name="containerRegistry"></param>
        protected virtual void RegisterRequiredTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance(_containerExtension);
            containerRegistry.RegisterSingleton<ILoggerFacade, TextLogger>();
            containerRegistry.RegisterSingleton<RegionAdapterMappings>();
            containerRegistry.RegisterSingleton<IRegionManager, RegionManager>();
            //containerRegistry.RegisterSingleton<IEventAggregator, EventAggregator>();
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
                regionAdapterMappings.RegisterMapping(typeof(Selector), _containerExtension.Resolve<SelectorRegionAdapter>());
                regionAdapterMappings.RegisterMapping(typeof(ItemsControl), _containerExtension.Resolve<ItemsControlRegionAdapter>());
                regionAdapterMappings.RegisterMapping(typeof(ContentControl), _containerExtension.Resolve<ContentControlRegionAdapter>());
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
        /// Creates the shell or main page of the application.
        /// DO NOT SET ANYTHING ON THE CURRENT APP UI HERE JUST CREATE YOUR SHELL
        /// </summary>
        /// <returns>The shell of the application.</returns>
        protected abstract DependencyObject CreateShell();

        /// <summary>
        /// Initializes Shell.
        /// </summary>
        protected virtual void InitializeShell(DependencyObject appShell)
        {
            Shell = appShell;
        }

        /// <summary>
        /// This will be called on app startup
        /// Here you can set shell in the current application window as the app is ready to start
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
            ServiceLocator.SetLocatorProvider(() => _containerExtension.Resolve<IServiceLocator>());
        }
    }
}
