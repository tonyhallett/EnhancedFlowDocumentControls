using FlaUI.Core.AutomationElements;
using FlaUI.Core.Tools;
using UIAutomationHelpers;
using UITests.TestHelpers;

namespace UITests.Tests.Enhanced
{
    internal sealed class FindToolBarChangedTests()
        : FindToolBarTestsBase(DemoWindowTypeNames.FindToolBarChanged, FrameworkVersion.Net472)
    {
        [Test]
        public void Should_Work_If_Changed_Before_Shown()
        {
            ClickChangeImmediate();
            CommonHelpers.ShowFindToolbarFirstTime(Window);
            AssertChangedFindToolBar();

            FindsTest("Changed", "Changed", Typer.TypeF3);
        }

        private void ClickChangeImmediate()
            => Window.FindFirstDescendant(cf => cf.ByAutomationId("ChangeFindToolBarImmediateButton")).AsButton()!.Click();

        private AutomationElement? GetChangedFindToolBar() => Window.FindFirstDescendant(cf => cf.ByAutomationId("changedFindToolBar"));

        private void AssertChangedFindToolBar() => Assert.That(GetChangedFindToolBar(), Is.Not.Null);

        [Test]
        public void Should_Change_FindToolBar_With_Existing_ViewModel()
        {
            CommonHelpers.ShowFindToolbarFirstTime(Window);
            FocusFindTextAndSetText("Changed");

            ClickChangeImmediate();
            _ = FocusFindTextBox();
            Typer.TypeF3();
            AssertSelected("Changed", "Changed");
        }

        [Test]
        public void Should_Focus_Previous_Focused_With_Matching_XName_In_Host_Scope_Host_Focused()
        {
            CommonHelpers.ShowFindToolbarFirstTime(Window);
            Window.FindFirstDescendant(cf => cf.ByAutomationId("ChangeFindToolBarDelayedButton")).AsButton()!.Click();
            TextBox? findTextBox = ControlFinder.FindFindTextBox(Window);
            _ = Retry.WhileException(() => FocusFindTextAndSetText("abc"));
            Typer.TypeTab(2);

            RetryResult<AutomationElement?> retryResult = Retry.WhileNull(GetChangedFindToolBar, timeout: TimeSpan.FromSeconds(2));

            Assert.That(retryResult.Result, Is.Not.Null);

            Assert.That(Window.Automation.FocusedElement().AutomationId, Is.EqualTo(AutomationIds.FindNextButton));
        }
    }
}
