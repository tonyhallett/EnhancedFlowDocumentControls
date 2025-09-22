using FlaUI.Core.AutomationElements;

namespace VideoRecorder
{
    internal interface IStep
    {
        int Execute(Window window);
    }
}
