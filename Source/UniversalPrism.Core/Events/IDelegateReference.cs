


using System;

namespace UniversalPrism.Core.Events
{
    /// <summary>
    /// Represents a reference to a <see cref="Delegate"/>.
    /// </summary>
    public interface IDelegateReference
    {
        /// <summary>
        /// Gets the referenced <see cref="Delegate" /> object.
        /// </summary>
        /// <value>A <see cref="Delegate"/> instance if the target is valid; otherwise <see langword="null"/>.</value>
        System.Delegate Target { get; }
    }
}