using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Prism.Events;
using Prism.Logging;
using SimpleMvvm.Core;
using SimpleMvvm.Core.Container;
using SimpleMvvm.Core.Mvvm;

namespace SimpleMvvm.View
{
    public abstract partial class ApplicationTemplate : IApplicationBase
    {
        private static readonly SemaphoreSlim StartSemaphore = new SemaphoreSlim(1, 1);

        private static int _initialized;
        private readonly bool _logStartingEvents = false;

        private IContainerExtension containerExtension;
        private ILoggerFacade logger;

        public ApplicationTemplate()
        {
            InternalInitialize();
            Resuming += async (s, e) =>
            {
                await InternalStartAsync(new StartArgs(ResumeArgs.Create(ApplicationExecutionState.Suspended),
                    StartKinds.Resume));
            };
        }

        public new static ApplicationTemplate Current => (ApplicationTemplate) Application.Current;
        public IContainerProvider Container => containerExtension;

        private void InternalInitialize()
        {
            if (_logStartingEvents)
                logger.Log($"{nameof(ApplicationTemplate)}.{nameof(InternalInitialize)}", Category.Info, Priority.None);

            containerExtension = CreateContainerExtension();
            if (containerExtension is IContainerRegistry registry)
            {
                registry.RegisterSingleton<ILoggerFacade, DebugLogger>();
                registry.RegisterSingleton<IEventAggregator, EventAggregator>();
                RegisterInternalTypes(registry);
            }

            RegisterTypes(containerExtension);

            containerExtension.FinalizeExtension();

            logger = Container.Resolve<ILoggerFacade>();

            ConfigureViewModelLocator();
        }

        private void CallOnInitializedOnce()
        {
            if (_logStartingEvents)
                logger.Log($"{nameof(ApplicationTemplate)}.{nameof(CallOnInitializedOnce)}", Category.Info,
                    Priority.None);

            if (Interlocked.Increment(ref _initialized) == 1)
            {
                logger.Log("[App.OnInitialize()]", Category.Info, Priority.None);
                OnInitialized();
            }
        }

        private async Task InternalStartAsync(StartArgs startArgs)
        {
            await StartSemaphore.WaitAsync();
            if (_logStartingEvents)
                logger.Log($"{nameof(ApplicationTemplate)}.{nameof(InternalStartAsync)}({startArgs})", Category.Info,
                    Priority.None);

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

        public virtual void OnInitialized()
        {
            /* empty */
        }

        public virtual void OnStart(IStartArgs args)
        {
            /* empty */
        }

        public virtual Task OnStartAsync(IStartArgs args)
        {
            return Task.CompletedTask;
        }

        public virtual void ConfigureViewModelLocator()
        {
            ViewModelLocationProvider.SetDefaultViewModelFactory((view, type) =>
                containerExtension.ResolveViewModelForView(view, type));
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