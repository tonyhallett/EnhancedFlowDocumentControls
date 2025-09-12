using System.Runtime.InteropServices;

namespace UIAutomationHelpers
{
    public static class NativeMethods
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetProcessDPIAware();
    }
}
