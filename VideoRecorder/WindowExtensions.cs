using System.Drawing;
using FlaUI.Core.AutomationElements;

namespace VideoRecorder
{
    internal static class WindowExtensions
    {
        public static Rectangle InflatedBounds(this Window window, int amount)
        {
            Rectangle windowBounds = window.BoundingRectangle;
            var bounds = new Rectangle(windowBounds.Location, windowBounds.Size);
            bounds.Inflate(amount, amount);
            return bounds;
        }
    }
}
