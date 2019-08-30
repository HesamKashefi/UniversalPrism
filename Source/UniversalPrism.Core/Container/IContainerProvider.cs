using System;

namespace UniversalPrism.Core.Container
{
    public interface IContainerProvider
    {
        T Resolve<T>();
        object Resolve(Type type);

        T Resolve<T>(string name);
        object Resolve(Type type, string name);
    }
}