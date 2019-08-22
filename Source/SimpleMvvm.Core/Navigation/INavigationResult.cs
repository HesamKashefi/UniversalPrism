using System;

namespace SimpleMvvm.Core.Navigation
{
    public interface INavigationResult
    {
        bool Success { get; }

        Exception Exception { get; }
    }
}