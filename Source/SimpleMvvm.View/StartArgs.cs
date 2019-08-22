using SimpleMvvm.Core;

namespace SimpleMvvm.View
{
    public class StartArgs : IStartArgs
    {
        public StartArgs(object arguments, StartKinds startKind)
        {
            Arguments = arguments;
            StartKind = startKind;
        }

        public StartArgs(object arguments, StartKinds startKind, int startCause)
        {
            Arguments = arguments;
            StartKind = startKind;
            StartCause = startCause;
        }

        #region Implementation of IStartArgs

        public object Arguments { get; set; }
        public int StartCause { get; }
        public StartKinds StartKind { get; set; }

        #endregion
    }
}
