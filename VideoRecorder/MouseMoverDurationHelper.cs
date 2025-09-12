using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;
using FlaUI.Core.Tools;

namespace VideoRecorder
{
    internal static class MouseMoverDurationHelper
    {
        public static TimeSpan Get(AutomationElement toElement)
        {
            System.Drawing.Point newPosition = toElement.GetClickablePoint();
            double num = Mouse.Position.Distance(newPosition.X, newPosition.Y);
            return TimeSpan.FromMilliseconds((double)Convert.ToInt32(num / Mouse.MovePixelsPerMillisecond));
        }
    }
}
