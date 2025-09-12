using FlaUI.Core.Capturing;
using FlaUI.UIA3;
using UIAutomationHelpers;
using VideoRecorder;

// https://github.com/FlaUI/FlaUI/issues/359
// https://github.com/FlaUI/FlaUI/issues/306
NativeMethods.SetProcessDPIAware();

(string DemoVideoPath, string NormalVideoPath) = await Record();

VideoDeleter.Delete(DemoVideoPath, NormalVideoPath);

static async Task<(string DemoVideoPath, string NormalVideoPath)> Record()
{
    CaptureSettings? captureSettings = null;
    List<IStep> steps = RecordSteps.GetSteps();
    using var automation = new UIA3Automation();

    string? normalVideoPath = null;
    string? enhancedVideoPath = await Recorder.RecordAvi(nameof(Demo.EnhancedFlowDocumentReaderVideoWindow), steps, automation, captureSettings);
    if (enhancedVideoPath != null)
    {
        normalVideoPath = await Recorder.RecordAvi(nameof(Demo.NormalFlowDocumentReaderVideoWindow), steps, automation, captureSettings);
    }

    if (enhancedVideoPath == null || normalVideoPath == null)
    {
        throw new Exception("Recording failed. One of the video paths is null.");
    }

    await AviToGifConverter.ConvertAsync(enhancedVideoPath, normalVideoPath);
    return (enhancedVideoPath, normalVideoPath);
}
