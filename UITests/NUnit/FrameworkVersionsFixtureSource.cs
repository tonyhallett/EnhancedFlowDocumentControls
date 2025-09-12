using System.Collections;

namespace UITests.NUnit
{
    internal sealed class FrameworkVersionsFixtureSource
    {
        public static IEnumerable FixtureArgs { get; } = FrameworkVersionValues.Get();
    }
}
