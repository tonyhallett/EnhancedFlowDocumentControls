using System.Collections.ObjectModel;
using UIAutomationHelpers;

namespace UITests.NUnit
{
    internal static class FrameworkVersionValues
    {
        private static ReadOnlyCollection<FrameworkVersion> Values { get; } = Enum.GetValues<FrameworkVersion>().AsReadOnly();

        // change to only test on specific version
        public static IEnumerable<FrameworkVersion> Get() => Values;
    }
}
