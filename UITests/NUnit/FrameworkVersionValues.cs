using System.Collections.ObjectModel;
using UIAutomationHelpers;

namespace UITests.NUnit
{
    internal static class FrameworkVersionValues
    {
        private static ReadOnlyCollection<FrameworkVersion> Values { get; } = Enum.GetValues<FrameworkVersion>().AsReadOnly();

        public static IEnumerable<FrameworkVersion> Get() => [FrameworkVersion.Net472];
    }
}
