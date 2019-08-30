using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Windows.UI.Xaml;
using CommonServiceLocator;
using UniversalPrism.View.Common;

namespace UniversalPrism.View.Regions.Navigation
{
    /// <summary>
    /// Provides navigation for regions.
    /// </summary>
    public class RegionNavigationService : IRegionNavigationService
    {
        #region Dependencies
        private readonly IServiceLocator serviceLocator;
        private readonly IRegionNavigationContentLoader regionNavigationContentLoader;
        private readonly IRegionNavigationJournal journal;
        private NavigationContext currentNavigationContext; 
        #endregion

        #region Ctor
        /// <summary>
        /// Initializes a new instance of the <see cref="RegionNavigationService"/> class.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="regionNavigationContentLoader">The navigation target handler.</param>
        /// <param name="journal">The journal.</param>
        public RegionNavigationService(IServiceLocator serviceLocator, IRegionNavigationContentLoader regionNavigationContentLoader, IRegionNavigationJournal journal)
        {
            this.serviceLocator = serviceLocator ?? throw new ArgumentNullException(nameof(serviceLocator));
            this.regionNavigationContentLoader = regionNavigationContentLoader ?? throw new ArgumentNullException(nameof(regionNavigationContentLoader));
            this.journal = journal ?? throw new ArgumentNullException(nameof(journal));
            this.journal.NavigationTarget = this;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the region.
        /// </summary>
        /// <value>The region.</value>
        public IRegion Region { get; set; }

        /// <summary>
        /// Gets the journal.
        /// </summary>
        /// <value>The journal.</value>
        public IRegionNavigationJournal Journal => this.journal; 
        #endregion

        #region Events
        /// <summary>
        /// Raised when the region is about to be navigated to content.
        /// </summary>
        public event EventHandler<RegionNavigationEventArgs> Navigating;

        private void RaiseNavigating(NavigationContext navigationContext)
        {
            Navigating?.Invoke(this, new RegionNavigationEventArgs(navigationContext));
        }

        /// <summary>
        /// Raised when the region is navigated to content.
        /// </summary>
        public event EventHandler<RegionNavigationEventArgs> Navigated;

        private void RaiseNavigated(NavigationContext navigationContext)
        {
            Navigated?.Invoke(this, new RegionNavigationEventArgs(navigationContext));
        }

        /// <summary>
        /// Raised when a navigation request fails.
        /// </summary>
        public event EventHandler<RegionNavigationFailedEventArgs> NavigationFailed;

        private void RaiseNavigationFailed(NavigationContext navigationContext, Exception error)
        {
            NavigationFailed?.Invoke(this, new RegionNavigationFailedEventArgs(navigationContext, error));
        } 
        #endregion

        /// <summary>
        /// Initiates navigation to the specified target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="navigationCallback">A callback to execute when the navigation request is completed.</param>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Exception is marshaled to callback")]
        public void RequestNavigate(Uri target, Action<NavigationResult> navigationCallback)
        {
            this.RequestNavigate(target, navigationCallback, null);
        }

        /// <summary>
        /// Initiates navigation to the specified target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="navigationCallback">A callback to execute when the navigation request is completed.</param>
        /// <param name="navigationParameters">The navigation parameters specific to the navigation request.</param>
        public void RequestNavigate(Uri target, Action<NavigationResult> navigationCallback, NavigationParameters navigationParameters)
        {
            if (navigationCallback == null)
                throw new ArgumentNullException(nameof(navigationCallback));

            try
            {
                this.DoNavigate(target, navigationCallback, navigationParameters);
            }
            catch (Exception e)
            {
                this.NotifyNavigationFailed(new NavigationContext(this, target), navigationCallback, e);
            }
        }

        private void DoNavigate(Uri source, Action<NavigationResult> navigationCallback, NavigationParameters navigationParameters)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (this.Region == null)
                throw new InvalidOperationException("NavigationServiceHasNoRegion");

            this.currentNavigationContext = new NavigationContext(this, source, navigationParameters);

            // starts querying the active views
            RequestCanNavigateFromOnCurrentlyActiveView(
                this.currentNavigationContext,
                navigationCallback,
                this.Region.ActiveViews.ToArray(),
                0);
        }

        private void RequestCanNavigateFromOnCurrentlyActiveView(
            NavigationContext navigationContext,
            Action<NavigationResult> navigationCallback,
            object[] activeViews,
            int currentViewIndex)
        {
            if (currentViewIndex < activeViews.Length)
            {
                if (activeViews[currentViewIndex] is IConfirmNavigationRequest vetoingView)
                {
                    // the current active view implements IConfirmNavigationRequest, request confirmation
                    // providing a callback to resume the navigation request
                    vetoingView.ConfirmNavigationRequest(
                        navigationContext,
                        canNavigate =>
                        {
                            if (this.currentNavigationContext == navigationContext && canNavigate)
                            {
                                RequestCanNavigateFromOnCurrentlyActiveViewModel(
                                    navigationContext,
                                    navigationCallback,
                                    activeViews,
                                    currentViewIndex);
                            }
                            else
                            {
                                this.NotifyNavigationFailed(navigationContext, navigationCallback, null);
                            }
                        });
                }
                else
                {
                    RequestCanNavigateFromOnCurrentlyActiveViewModel(
                        navigationContext,
                        navigationCallback,
                        activeViews,
                        currentViewIndex);
                }
            }
            else
            {
                ExecuteNavigation(navigationContext, activeViews, navigationCallback);
            }
        }

        private void RequestCanNavigateFromOnCurrentlyActiveViewModel(
            NavigationContext navigationContext,
            Action<NavigationResult> navigationCallback,
            object[] activeViews,
            int currentViewIndex)
        {
            if (activeViews[currentViewIndex] is FrameworkElement frameworkElement)
            {
                if (frameworkElement.DataContext is IConfirmNavigationRequest vetoingViewModel)
                {
                    // the data model for the current active view implements IConfirmNavigationRequest, request confirmation
                    // providing a callback to resume the navigation request
                    vetoingViewModel.ConfirmNavigationRequest(
                        navigationContext,
                        canNavigate =>
                        {
                            if (this.currentNavigationContext == navigationContext && canNavigate)
                            {
                                RequestCanNavigateFromOnCurrentlyActiveView(
                                    navigationContext,
                                    navigationCallback,
                                    activeViews,
                                    currentViewIndex + 1);
                            }
                            else
                            {
                                this.NotifyNavigationFailed(navigationContext, navigationCallback, null);
                            }
                        });

                    return;
                }
            }

            RequestCanNavigateFromOnCurrentlyActiveView(
                navigationContext,
                navigationCallback,
                activeViews,
                currentViewIndex + 1);
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Exception is marshaled to callback")]
        private void ExecuteNavigation(NavigationContext navigationContext, object[] activeViews, Action<NavigationResult> navigationCallback)
        {
            try
            {
                NotifyActiveViewsNavigatingFrom(navigationContext, activeViews);

                object view = this.regionNavigationContentLoader.LoadContent(this.Region, navigationContext);

                // Raise the navigating event just before activating the view.
                this.RaiseNavigating(navigationContext);

                this.Region.Activate(view);

                // Update the navigation journal before notifying others of navigation
                IRegionNavigationJournalEntry journalEntry = this.serviceLocator.GetInstance<IRegionNavigationJournalEntry>();
                journalEntry.Uri = navigationContext.Uri;
                journalEntry.Parameters = navigationContext.Parameters;

                bool persistInHistory = PersistInHistory(activeViews);

                this.journal.RecordNavigation(journalEntry, persistInHistory);

                // The view can be informed of navigation
                Action<INavigationAware> action = (n) => n.OnNavigatedTo(navigationContext);
                MvvmHelpers.ViewAndViewModelAction(view, action);

                navigationCallback(new NavigationResult(navigationContext, true));

                // Raise the navigated event when navigation is completed.
                this.RaiseNavigated(navigationContext);
            }
            catch (Exception e)
            {
                this.NotifyNavigationFailed(navigationContext, navigationCallback, e);
            }
        }

        private static bool PersistInHistory(object[] activeViews)
        {
            bool persist = true;
            if (activeViews.Length > 0)
            {
                MvvmHelpers.ViewAndViewModelAction<IJournalAware>(activeViews[0], ija => { persist &= ija.PersistInHistory(); });
            }
            return persist;
        }

        private void NotifyNavigationFailed(NavigationContext navigationContext, Action<NavigationResult> navigationCallback, Exception e)
        {
            var navigationResult =
                e != null ? new NavigationResult(navigationContext, e) : new NavigationResult(navigationContext, false);

            navigationCallback(navigationResult);
            this.RaiseNavigationFailed(navigationContext, e);
        }

        private static void NotifyActiveViewsNavigatingFrom(NavigationContext navigationContext, object[] activeViews)
        {
            InvokeOnNavigationAwareElements(activeViews, (n) => n.OnNavigatedFrom(navigationContext));
        }

        private static void InvokeOnNavigationAwareElements(IEnumerable<object> items, Action<INavigationAware> invocation)
        {
            foreach (var item in items)
            {
                MvvmHelpers.ViewAndViewModelAction(item, invocation);
            }
        }
    }
}
