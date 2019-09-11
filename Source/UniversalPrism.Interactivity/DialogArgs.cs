using System.Windows.Input;

namespace UniversalPrism.Interactivity
{
    public class DialogArgs : IDialogArgs
    {
        public ICommand PrimaryButtonCommand { get; set; }
        public object PrimaryButtonCommandParameter { get; set; }
        public string PrimaryButtonText { get; set; }

        public ICommand SecondaryButtonCommand { get; set; }
        public object SecondaryButtonCommandParameter { get; set; }
        public string SecondaryButtonText { get; set; }

        public ICommand CloseButtonCommand { get; set; }
        public object CloseButtonCommandParameter { get; set; }
        public string CloseButtonText { get; set; }

        public string Title { get; set; }
        public object Content { get; set; }
    }
}
