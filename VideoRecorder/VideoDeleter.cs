namespace VideoRecorder
{
    internal static class VideoDeleter
    {
        public static void Delete(params string?[] videoPaths)
        {
            foreach (string? videoPath in videoPaths)
            {
                if (string.IsNullOrEmpty(videoPath) || !File.Exists(videoPath))
                {
                    continue;
                }

                try
                {
                    File.Delete(videoPath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to delete video {videoPath}: {ex.Message}");
                }
            }
        }
    }
}
