using FlaUI.Core.AutomationElements;
using FlaUI.Core.Tools;
using UIAutomationHelpers;
using UITests.NUnit;
using UITests.TestHelpers;

namespace UITests.Tests.Comparisons
{
    [CommonComparisonTest]
    internal sealed class NoStateTest(string windowTypeName, FrameworkVersion frameworkVersion)
        : FindToolBarTestsBase(windowTypeName, frameworkVersion)
    {
        [Test]
        public void Closing_Removes_Find_Toolbar_State()
        {
            CommonHelpers.ShowFindToolbarFirstTime(Window);

            TextBox? findTextBox = ControlFinder.FindFindTextBox(Window);
            findTextBox!.Text = "abc";
            MenuItem matchCaseMenuItem = MenuHelper.SelectMatchCase(Window);
            Assert.That(matchCaseMenuItem.IsChecked, Is.True);

            CloseFindToolbar();
            AssertClosed();

            Thread.Sleep(100);
            ShowFindToolbar();

            findTextBox = Retry.WhileNull(() => ControlFinder.FindFindTextBox(Window)).Result;
            Assert.That(findTextBox!.Text, Is.Empty);
            matchCaseMenuItem = MenuHelper.GetMatchCaseMenuItem(Window);
            Assert.That(matchCaseMenuItem.IsChecked, Is.False);
        }

        private static void CloseFindToolbar() => Typer.TypeEsc();

        private static void ShowFindToolbar() => Typer.TypeF3();
    }
}
