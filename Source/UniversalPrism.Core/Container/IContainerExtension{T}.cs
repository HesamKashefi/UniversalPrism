namespace UniversalPrism.Core.Container
{
    public interface IContainerExtension<out TContainer> : IContainerExtension
    {
        /// <summary>
        /// The instance of the wrapped container
        /// </summary>
        TContainer Instance { get; }
    }
}
