namespace UniversalPrism.Core
{
    public interface IStartArgs
    {
        object Arguments { get; }
        int StartCause { get; }
        StartKinds StartKind { get; }
    }
    public enum StartKinds
    {
        Prelaunch,
        Launch,
        Activate,
        Background,
        Resume
    }
}