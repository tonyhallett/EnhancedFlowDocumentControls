using FlaUI.Core.AutomationElements;

namespace VideoRecorder
{
    internal sealed class SimpleStep(Action<Window> action, int wait) : IStep
    {
        public int Execute(Window window)
        {
            action(window);
            return wait;
        }
    }
}
