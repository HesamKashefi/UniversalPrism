using Prism.Events;
using Prism.Logging;
using SimpleMvvm.Core;
using SimpleMvvm.Core.Container;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;

namespace SimpleMvvm.View
{
    public abstract partial class ApplicationTemplate : IApplicationBase
    {
        public new static ApplicationTemplate Current => (ApplicationTemplate)Application.Current;
        private static readonly SemaphoreSlim StartSemaphore = new SemaphoreSlim(1, 1);
        private readonly bool _logStartingEvents = false;

        public ApplicationTemplate()
        {
            InternalInitialize();
            Resuming += async (s, e) =>
            {
                await InternalStartAsync(new StartArgs(ResumeArgs.Create(ApplicationExecutionState.Suspended), StartKinds.Resume));
            };
        }

        private IContainerExtension _containerExtension;
        public IContainerProvider Container => _containerExtension;

        private void InternalInitialize()
        {
            if (_logStartingEvents)
            {
                _logger.Log($"{nameof(ApplicationTemplate)}.{nameof(InternalInitialize)}", Category.Info, Priority.None);
            }

            _containerExtension = CreateContainerExtension();
            if (_containerExtension is IContainerRegistry registry)
            {
                registry.RegisterSingleton<ILoggerFacade, DebugLogger>();
                registry.RegisterSingleton<IEventAggregator, EventAggregator>();
                RegisterInternalTypes(registry);
            }

            RegisterTypes(_containerExtension as IContainerRegistry);

            _containerExtension.FinalizeExtension();

            _logger = Container.Resolve<ILoggerFacade>();

            ConfigureViewModelLocator();
        }

        private static int _initialized = 0;
        private ILoggerFacade _logger;

        private void CallOnInitializedOnce()
        {
            if (_logStartingEvents)
            {
                _logger.Log($"{nameof(ApplicationTemplate)}.{nameof(CallOnInitializedOnce)}", Category.Info, Priority.None);
            }

            if (Interlocked.Increment(ref _initialized) == 1)
            {
                _logger.Log("[App.OnInitialize()]", Category.Info, Priority.None);
                OnInitialized();
            }
        }

        private async Task InternalStartAsync(StartArgs startArgs)
        {
            await StartSemaphore.WaitAsync();
            if (_logStartingEvents)
            {
                _logger.Log($"{nameof(ApplicationTemplate)}.{nameof(InternalStartAsync)}({startArgs})", Category.Info, Priority.None);
            }

            try
            {
                CallOnInitializedOnce();
                OnStart(startArgs);
                await OnStartAsync(startArgs);
                Window.Current.Activate();
            }
            finally
            {
                StartSemaphore.Release();
            }
        }


        #region overrides

        public abstract void RegisterTypes(IContainerRegistry container);

        public virtual void OnInitialized() { /* empty */ }

        public virtual void OnStart(IStartArgs args) {  /* empty */ }

        public virtual Task OnStartAsync(IStartArgs args)
        {
            return Task.CompletedTask;
        }

        public virtual void ConfigureViewModelLocator()
        {
            ViewModelLocationProvider.SetDefaultViewModelFactory((view, type) => _containerExtension.ResolveViewModelForView(view, type));
        }

        public abstract IContainerExtension CreateContainerExtension();

        protected virtual void RegisterInternalTypes(IContainerRegistry containerRegistry)
        {
            // don't forget there is no logger yet
            Debug.WriteLine($"{nameof(ApplicationTemplate)}.{nameof(RegisterInternalTypes)}()");
        }

        #endregion
    }
}
