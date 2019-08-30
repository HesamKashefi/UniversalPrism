using CommonServiceLocator;
using SimpleMvvm.Core.Container;
using SimpleMvvm.View;
using SimpleMvvm.View.Regions.Navigation;

namespace SimpleMvvm.Unity
{
    /// <summary>
    /// Application base class with Unity Container Registered
    /// </summary>
    public abstract class UnityApplicationBase : ApplicationBase
    {
        /// <inheritdoc />
        protected override IContainerExtension CreateContainerExtension()
        {
            return new UnityContainerExtension();
        }

        /// <inheritdoc />
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterRequiredTypes(containerRegistry);
            containerRegistry.RegisterSingleton<IRegionNavigationContentLoader, UnityRegionNavigationContentLoader>();
            containerRegistry.RegisterSingleton<IServiceLocator, UnityServiceLocatorAdapter>();
        }
    }
}
