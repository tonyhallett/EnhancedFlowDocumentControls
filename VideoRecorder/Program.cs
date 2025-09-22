using FlaUI.Core.Capturing;
using FlaUI.UIA3;
using UIAutomationHelpers;
using VideoRecorder;

// https://github.com/FlaUI/FlaUI/issues/359
// https://github.com/FlaUI/FlaUI/issues/306
NativeMethods.SetProcessDPIAware();

try
{
    (string DemoVideoPath, string NormalVideoPath) = await Record();
    VideoDeleter.Delete(DemoVideoPath, NormalVideoPath);
}
catch (Exception ex)
{
    File.WriteAllText(args[0], ex.ToString());
}

static async Task<(string DemoVideoPath, string NormalVideoPath)> Record()
{
    CaptureSettings? captureSettings = null;
    List<IStep> steps = RecordSteps.GetSteps();
    using var automation = new UIA3Automation();
    string? enhancedVideoPath = await Recorder.RecordAvi(nameof(Demo.EnhancedFlowDocumentReaderVideoWindow), steps, automation, captureSettings);
    string? normalVideoPath = await Recorder.RecordAvi(nameof(Demo.NormalFlowDocumentReaderVideoWindow), steps, automation, captureSettings);
    await AviToGifConverter.ConvertAsync(enhancedVideoPath!, normalVideoPath!);
    return (enhancedVideoPath!, normalVideoPath!);
}
