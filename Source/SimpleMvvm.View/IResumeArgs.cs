using System;
using Windows.ApplicationModel.Activation;

namespace SimpleMvvm.View
{
    public interface IResumeArgs
    {
        ApplicationExecutionState PreviousExecutionState { get; set; }
        ActivationKind Kind { get; set; }
        DateTime SuspensionDate { get; set; }
    }
}