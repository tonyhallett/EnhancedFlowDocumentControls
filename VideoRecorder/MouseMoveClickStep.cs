using FlaUI.Core.AutomationElements;

namespace VideoRecorder
{
    internal sealed class MouseMoveClickStep(Func<Window, AutomationElement> getElement, int wait) : IStep
    {
        public int Execute(Window window)
        {
            AutomationElement element = getElement(window);
            TimeSpan timeSpan = MouseMoverDurationHelper.Get(element);
            element.Click(true);
            return timeSpan.Milliseconds + wait;
        }
    }
}
