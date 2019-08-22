using System;
using System.Collections.Specialized;
using Prism.Navigation;
using SimpleMvvm.View.Common;

namespace SimpleMvvm.View.Regions.Behaviors
{
    public class IDestructibleRegionBehavior : RegionBehavior
    {
        public const string BehaviorKey = "IDestructibleRegionBehavior";

        protected override void OnAttach()
        {
            Region.Views.CollectionChanged += Views_CollectionChanged;
        }

        private void Views_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    Action<IDestructible> invocation = destructible => destructible.Destroy();
                    MvvmHelpers.ViewAndViewModelAction(item, invocation);
                }
            }
        }
    }
}
