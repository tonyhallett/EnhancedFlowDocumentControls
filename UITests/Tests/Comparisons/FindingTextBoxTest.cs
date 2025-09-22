using FlaUI.Core.AutomationElements;
using FlaUI.Core.Tools;
using UIAutomationHelpers;
using UITests.NUnit;

namespace UITests.Tests.Comparisons
{
    // <findControls:FindTextBox - defined in Control Template
    [FindingTextBoxTest]
    internal sealed class FindingTextBoxTest(string windowTypeName, FrameworkVersion frameworkVersion)
        : OpenedFindToolbarTestsBase(windowTypeName, frameworkVersion)
    {
        [Test]
        public void Should_Focus_The_TextBox_With_XName_findTextBox()
        {
            RetryResult<TextBox?> findTextBoxResult = Retry.WhileNull(() =>
            {
                TextBox? findTextBox = ControlFinder.FindFindTextBox(Window);
                return findTextBox?.Properties.HasKeyboardFocus ? findTextBox : null;
            });
            Assert.That(findTextBoxResult.Result, Is.Not.Null, "Find text box should not be null");
        }
    }
}
