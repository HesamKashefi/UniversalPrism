using System;
using System.Collections.Specialized;
using UniversalPrism.Core;
using UniversalPrism.View.Common;

namespace UniversalPrism.View.Regions.Behaviors
{
    /// <summary>
    /// Executes the <see cref="IDestructible.Destroy"/> method on the removed items from the <see cref="IRegion"/>
    /// </summary>
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
