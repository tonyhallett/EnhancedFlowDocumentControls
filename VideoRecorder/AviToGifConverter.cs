using System.Diagnostics;

namespace VideoRecorder
{
    internal static class AviToGifConverter
    {
        public static async Task ConvertAsync(string enhancedVideoPath, string normalVideoPath)
        {
            string ffmpegPath = await FfmpegInstallationHelper.GetFfmpegPathAsync();
            ConvertAviToGif(enhancedVideoPath, ffmpegPath);
            ConvertAviToGif(normalVideoPath, ffmpegPath);
        }

        private static string GetGifPath(string aviPath)
        {
            string? videoDirectory = Path.GetDirectoryName(aviPath);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(aviPath);
            return Path.Combine(videoDirectory!, $"{fileNameWithoutExtension}.gif");
        }

        private static void ConvertAviToGif(string inputAvi, string ffmpegPath)
            => ConvertAviToGif(ffmpegPath, inputAvi, GetGifPath(inputAvi));

        public static void ConvertAviToGif(string ffmpegPath, string inputAvi, string outputGif)
        {
            string arguments = $"-y -i \"{inputAvi}\" \"{outputGif}\"";

            var processStartInfo = new ProcessStartInfo
            {
                FileName = ffmpegPath,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            using var process = new Process { StartInfo = processStartInfo };
            _ = process.Start();
            string stderr = process.StandardError.ReadToEnd(); // FFmpeg logs to stderr
            process.WaitForExit();

            if (process.ExitCode == 0)
            {
                return;
            }

            throw new Exception($"FFmpeg failed with exit code {process.ExitCode}: {stderr}");
        }
    }
}
