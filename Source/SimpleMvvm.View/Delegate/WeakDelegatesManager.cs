using Prism.Events;
using System.Collections.Generic;
using System.Linq;

namespace SimpleMvvm.View.Delegate
{
    internal class WeakDelegatesManager
    {
        private readonly List<DelegateReference> listeners = new List<DelegateReference>();

        public void AddListener(System.Delegate listener)
        {
            this.listeners.Add(new DelegateReference(listener, false));
        }

        public void RemoveListener(System.Delegate listener)
        {
            //Remove the listener, and prune collected listeners
            this.listeners.RemoveAll(reference => reference.TargetEquals(null) || reference.TargetEquals(listener));
        }

        public void Raise(params object[] args)
        {
            this.listeners.RemoveAll(listener => listener.TargetEquals(null));

            foreach (System.Delegate handler in this.listeners.Select(listener => listener.Target).Where(listener => listener != null).ToList())
            {
                handler.DynamicInvoke(args);
            }
        }
    }
}
