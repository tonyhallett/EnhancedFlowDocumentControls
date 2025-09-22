using System.Collections;
using UIAutomationHelpers;

namespace UITests.NUnit
{
    internal static class TestFixtureSourceHelper
    {
        public static IEnumerator GetAllCtorPermutations(IEnumerable<string> windowTypeNames)
        {
            IEnumerable<FrameworkVersion> frameworkVersions = FrameworkVersionValues.Get();
            foreach (string windowTypeName in windowTypeNames)
            {
                foreach (FrameworkVersion frameworkVersion in frameworkVersions)
                {
                    yield return new object[] { windowTypeName, frameworkVersion };
                }
            }
        }
    }
}
