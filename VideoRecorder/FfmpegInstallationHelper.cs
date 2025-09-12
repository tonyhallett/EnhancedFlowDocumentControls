using FlauVideoRecorder = FlaUI.Core.Capturing.VideoRecorder;

namespace VideoRecorder
{
    internal static class FfmpegInstallationHelper
    {
        public static async Task<string> GetFfmpegPathAsync()
        {
            string? ffmpegPath = GetFfmpegPathTBD();

            // it won't download again if ffmpeg.exe exists in the target folder
            // todo create directory in the solution
            ffmpegPath ??= await FlauVideoRecorder.DownloadFFMpeg("C:\\temp");
            return ffmpegPath;
        }

        // todo - allow specifying the path
        private static string? GetFfmpegPathTBD() => null;
    }
}
