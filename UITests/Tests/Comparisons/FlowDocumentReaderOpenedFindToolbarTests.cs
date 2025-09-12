using FlaUI.Core.AutomationElements;
using FlaUI.Core.Tools;
using UIAutomationHelpers;
using UITests.NUnit;

namespace UITests.Tests.Comparisons
{
    [FlowDocumentReaderComparisonTest]
    internal sealed class FlowDocumentReaderOpenedFindToolbarTests(string windowTypeName, FrameworkVersion frameworkVersion)
        : OpenedFindToolbarTestsBase(windowTypeName, frameworkVersion)
    {
        [Test]
        public void Should_Close_Find_Toolbar_When_Click_Find_Button()
        {
            ControlFinder.FindFindButton(Window)!.Click();

            AssertClosed();
        }

        [Test]
        public void Should_Respect_IsFindEnabled()
        {
            FocusFindTextAndSetText("abc");

            RadioButton? findDisabledRadioButton = ControlFinder.FindFindDisabledRadioButton(Window);
            findDisabledRadioButton!.IsChecked = true;

            _ = Retry.WhileNotNull(FindFindToolbar, timeout: TimeSpan.FromSeconds(2), throwOnTimeout: true);

            RadioButton? findEnabledRadioButton = ControlFinder.FindFindEnabledRadioButton(Window);
            findEnabledRadioButton!.IsChecked = true;

            RetryResult<Button?> findButtonResult = Retry.WhileNull(() => ControlFinder.FindFindButton(Window));
            findButtonResult.Result!.Click();

            AssertShowsFindToolbar();

            TextBox? findTextBox = ControlFinder.FindFindTextBox(Window);
            Assert.That(findTextBox!.Text, Is.Empty);
        }
    }
}
