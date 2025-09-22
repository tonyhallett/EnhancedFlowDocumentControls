using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Capturing;
using FlaUI.Core.Tools;
using UIAutomationHelpers;
using FlauVideoRecorder = FlaUI.Core.Capturing.VideoRecorder;

namespace VideoRecorder
{
    internal sealed class Recorder
    {
        private static Application? s_application;
        private static Window? s_window;
        private static FlauVideoRecorder? s_videoRecorder;

        public static async Task<string?> RecordAvi(
            string windowTypeName,
            List<IStep> steps,
            AutomationBase automation,
            CaptureSettings? captureSettings = null)
        {
            s_application = DemoApplicationLauncher.Launch(FrameworkVersion.Net472, windowTypeName);
            try
            {
                Window? mainWindow = s_application.GetMainWindow(automation);
                s_window = mainWindow!;
                Thread.Sleep(1000);
                string aviFileName = windowTypeName.Replace("Window", string.Empty);
                string videoPath = await StartRecording(aviFileName, captureSettings);
                ExecuteSteps(steps);
                StopRecording();
                return videoPath;
            }
            finally
            {
                CloseApp();
            }
        }

        private static void ExecuteSteps(List<IStep> steps)
        {
            foreach (IStep step in steps)
            {
                int wait = step.Execute(s_window!);
                Thread.Sleep(wait);
            }
        }

        private static void CloseApp()
        {
            _ = s_application!.Close();
            _ = Retry.WhileFalse(() => s_application.HasExited, TimeSpan.FromSeconds(2.0), null, throwOnTimeout: false, ignoreException: true);
            s_application.Dispose();
            s_application = null;
        }

        private static void StopRecording() => s_videoRecorder!.Stop();

        private static async Task<string> StartRecording(string aviFileNameWithoutExtension, CaptureSettings? captureSettings)
        {
            string videosDirectory = Path.Combine(DemoApplicationLauncher.GetSolutionPath(), "videos");
            if (!Directory.Exists(videosDirectory))
            {
                _ = Directory.CreateDirectory(videosDirectory);
            }

            var videoRecorderSettings = new VideoRecorderSettings
            {
                VideoFormat = VideoFormat.xvid,
                VideoQuality = 6,
                TargetVideoPath = Path.Combine(videosDirectory, $"{aviFileNameWithoutExtension}.avi"),
                LogMissingFrames = false,
                ffmpegPath = await FfmpegInstallationHelper.GetFfmpegPathAsync(),
            };

            s_videoRecorder = new FlauVideoRecorder(
                videoRecorderSettings,
                (_) => Capture.Rectangle(s_window!.InflatedBounds(200), captureSettings).WithMouseOverlay());
            return videoRecorderSettings.TargetVideoPath;
        }
    }
}
