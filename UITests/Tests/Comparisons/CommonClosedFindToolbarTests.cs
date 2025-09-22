using UIAutomationHelpers;
using UITests.NUnit;
using UITests.TestHelpers;

namespace UITests.Tests.Comparisons
{
    [CommonComparisonTest]
    internal sealed class CommonClosedFindToolbarTests(string windowTypeName, FrameworkVersion frameworkVersion)
        : FindToolBarTestsBase(windowTypeName, frameworkVersion)
    {
        [Test]
        public void Should_Show_Find_Toolbar_When_Press_F3()
        {
            CommonHelpers.ShowFindToolbarFirstTime(Window);

            AssertShowsFindToolbar();
        }
    }
}
