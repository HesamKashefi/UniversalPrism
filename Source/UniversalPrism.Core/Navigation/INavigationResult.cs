using System;

namespace UniversalPrism.Core.Navigation
{
    public interface INavigationResult
    {
        bool Success { get; }

        Exception Exception { get; }
    }
}