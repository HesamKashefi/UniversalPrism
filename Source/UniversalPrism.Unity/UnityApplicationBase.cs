using CommonServiceLocator;
using UniversalPrism.Core.Container;
using UniversalPrism.View;
using UniversalPrism.View.Regions.Navigation;

namespace UniversalPrism.Unity
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
