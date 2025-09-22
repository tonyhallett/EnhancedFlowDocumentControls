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
            string releasePath = GetExePath(binDirectory, frameworkVersion, projectName, false);

            return !File.Exists(releasePath) ? throw new Exception($"{releasePath} does not exist.") : releasePath;
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
