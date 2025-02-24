using System;
using UniversalPrism.View.Properties;

namespace UniversalPrism.View.Regions
{
    /// <summary>
    /// Provides a base class for region's behaviors.
    /// </summary>
    public abstract class RegionBehavior : IRegionBehavior
    {
        private IRegion region;

        /// <summary>
        /// Behavior's attached region.
        /// </summary>
        public IRegion Region
        {
            get => region;
            set
            {
                if (this.IsAttached)
                {
                    throw new InvalidOperationException(Resources.RegionBehaviorRegionCannotBeSetAfterAttach);
                }

                this.region = value;
            }
        }

        /// <summary>
        /// Returns <see langword="true"/> if the behavior is attached to a region, <see langword="false"/> otherwise.
        /// </summary>
        public bool IsAttached { get; private set; }

        /// <summary>
        /// Attaches the behavior to the region.
        /// </summary>
        public void Attach()
        {
            if (this.region == null)
            {
                throw new InvalidOperationException(Resources.RegionBehaviorAttachCannotBeCallWithNullRegion);
            }

            IsAttached = true;
            OnAttach();
        }

        /// <summary>
        /// Override this method to perform the logic after the behavior has been attached.
        /// </summary>
        protected abstract void OnAttach();
    }
}
