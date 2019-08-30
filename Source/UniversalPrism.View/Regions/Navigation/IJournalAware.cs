namespace UniversalPrism.View.Regions.Navigation
{
    /// <summary>
    /// Provides a way for objects involved in navigation to opt-out of being added to the IRegionNavigationJournal BackStack.
    /// </summary>
    public interface IJournalAware
    {
        /// <summary>
        /// Determines if the current object is going to be added to the navigation journal's BackStack.
        /// </summary>
        /// <returns>True, add to BackStack. False, remove from BackStack.</returns>
        bool PersistInHistory();
    }
}
