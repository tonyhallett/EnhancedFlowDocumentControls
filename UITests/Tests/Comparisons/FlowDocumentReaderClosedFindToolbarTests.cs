using UIAutomationHelpers;
using UITests.NUnit;

namespace UITests.Tests.Comparisons
{
    [FlowDocumentReaderComparisonTest]
    internal sealed class FlowDocumentReaderClosedFindToolbarTests(string windowTypeName, FrameworkVersion frameworkVersion)
        : FindToolBarTestsBase(windowTypeName, frameworkVersion)
    {
        [Test]
        public void Should_Show_Find_Toolbar_When_Click_Find_Button()
        {
            FlaUI.Core.AutomationElements.Button findButton = ControlFinder.FindFindButton(Window)!;
            findButton.Click();
            AssertShowsFindToolbar();
        }
    }
}
