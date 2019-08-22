using System;
using System.Reflection;

namespace SimpleMvvm.View.Delegate
{
    /// <summary>
    /// Represents a reference to a <see cref="SimpleMvvm.View.Delegate"/> that may contain a
    /// <see cref="WeakReference"/> to the target. This class is used
    /// internally by the Prism Library.
    /// </summary>
    public class DelegateReference : IDelegateReference
    {
        private readonly System.Delegate _delegate;
        private readonly WeakReference weakReference;
        private readonly MethodInfo method;
        private readonly Type delegateType;

        /// <summary>
        /// Initializes a new instance of <see cref="DelegateReference"/>.
        /// </summary>
        /// <param name="delegate">The original <see cref="SimpleMvvm.View.Delegate"/> to create a reference for.</param>
        /// <param name="keepReferenceAlive">If <see langword="false" /> the class will create a weak reference to the delegate, allowing it to be garbage collected. Otherwise it will keep a strong reference to the target.</param>
        /// <exception cref="ArgumentNullException">If the passed <paramref name="delegate"/> is not assignable to <see cref="SimpleMvvm.View.Delegate"/>.</exception>
        public DelegateReference(System.Delegate @delegate, bool keepReferenceAlive)
        {
            if (@delegate == null)
                throw new ArgumentNullException("delegate");

            if (keepReferenceAlive)
            {
                this._delegate = @delegate;
            }
            else
            {
                weakReference = new WeakReference(@delegate.Target);
                method = @delegate.GetMethodInfo();
                delegateType = @delegate.GetType();
            }
        }

        /// <summary>
        /// Gets the <see cref="SimpleMvvm.View.Delegate" /> (the target) referenced by the current <see cref="DelegateReference"/> object.
        /// </summary>
        /// <value><see langword="null"/> if the object referenced by the current <see cref="DelegateReference"/> object has been garbage collected; otherwise, a reference to the <see cref="SimpleMvvm.View.Delegate"/> referenced by the current <see cref="DelegateReference"/> object.</value>
        public System.Delegate Target
        {
            get
            {
                if (_delegate != null)
                {
                    return _delegate;
                }
                else
                {
                    return TryGetDelegate();
                }
            }
        }

        /// <summary>
        /// Checks if the <see cref="SimpleMvvm.View.Delegate" /> (the target) referenced by the current <see cref="DelegateReference"/> object are equal to another <see cref="SimpleMvvm.View.Delegate" />.
        /// This is equivalent with comparing <see cref="Target"/> with <paramref name="delegate"/>, only more efficient.
        /// </summary>
        /// <param name="delegate">The other delegate to compare with.</param>
        /// <returns>True if the target referenced by the current object are equal to <paramref name="delegate"/>.</returns>
        public bool TargetEquals(System.Delegate @delegate)
        {
            if (_delegate != null)
            {
                return _delegate == @delegate;
            }
            if (@delegate == null)
            {
                return !method.IsStatic && !weakReference.IsAlive;
            }
            return weakReference.Target == @delegate.Target && Equals(method, @delegate.GetMethodInfo());
        }

        private System.Delegate TryGetDelegate()
        {
            if (method.IsStatic)
            {
                return method.CreateDelegate(delegateType, null);
            }
            object target = weakReference.Target;
            if (target != null)
            {
                return method.CreateDelegate(delegateType, target);
            }
            return null;
        }
    }
}