using FlaUI.Core.Capturing;

namespace VideoRecorder
{
    internal static class CaptureImageExtensions
    {
        public static CaptureImage WithMouseOverlay(this CaptureImage captureImage)
        {
            var mouseOverlay = new MouseOverlay(captureImage);
            return captureImage.ApplyOverlays(mouseOverlay);
        }
    }
}
