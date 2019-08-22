using System;

namespace SimpleMvvm.Core.Container
{
    public interface IContainerRegistry
    {
        void RegisterInstance(Type type, object instance);

        void RegisterSingleton(Type from, Type to);

        void Register(Type from, Type to);

        void Register(Type from, Type to, string name);
    }
}