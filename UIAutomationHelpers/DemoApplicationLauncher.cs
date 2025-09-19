using System.Reflection;
using FlaUI.Core;

namespace UIAutomationHelpers
{
    public static class DemoApplicationLauncher
    {
        public static Application Launch(FrameworkVersion frameworkVersion, string windowTypeName)
            => Application.Launch(GetAppPath("Demo", frameworkVersion), windowTypeName);

        public static string GetSolutionPath()
        {
            var directory = new DirectoryInfo(Assembly.GetExecutingAssembly().Location);
            while (directory!.Name != "EnhancedFlowDocumentControls")
            {
                directory = directory.Parent;
            }

            return directory.FullName;
        }

        private static string GetAppPath(string projectName, FrameworkVersion frameworkVersion)
        {
            string root = GetSolutionPath();
            string bin = Path.Combine(root, projectName, "bin");
            return GetLatest(bin, frameworkVersion, projectName);
        }

        private static string GetLatest(string binDirectory, FrameworkVersion frameworkVersion, string projectName)
        {
            string debugPath = GetExePath(binDirectory, frameworkVersion, projectName, true);
            string releasePath = GetExePath(binDirectory, frameworkVersion, projectName, false);
            bool debugPathExists = File.Exists(debugPath);
            bool releasePathExists = File.Exists(releasePath);
            if (debugPathExists && !releasePathExists)
            {
                return debugPath;
            }

            if (!debugPathExists && releasePathExists)
            {
                return releasePath;
            }

            if (debugPathExists && releasePathExists)
            {
                DateTime debugTime = File.GetLastWriteTime(debugPath);
                DateTime releaseTime = File.GetLastWriteTime(releasePath);
                return debugTime > releaseTime ? debugPath : releasePath;
            }

            throw new Exception($"Neither {debugPath} nor {releasePath} exists.");
        }

        private static string GetExePath(string binDirectory, FrameworkVersion frameworkVersion, string projectName, bool isDebug)
            => Path.Combine(binDirectory, isDebug ? "Debug" : "Release", GetFrameworkVersionDirectoryName(frameworkVersion), $"{projectName}.exe");

        private static string GetFrameworkVersionDirectoryName(FrameworkVersion frameworkVersion)
        {
            string directoryName = frameworkVersion.ToString().ToLower();
            int windowsIndex = directoryName.IndexOf("windows");
            if (windowsIndex > -1)
            {
                directoryName = directoryName[..windowsIndex] + ".0-windows";
            }

            return directoryName;
        }
    }
}
