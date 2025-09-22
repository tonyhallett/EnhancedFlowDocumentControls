using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using UIAutomationHelpers;
using UITests.NUnit;

namespace UITests.Tests.Comparisons
{
    [CommonComparisonTest]
    internal sealed class CommonNavigationTests(string windowTypeName, FrameworkVersion frameworkVersion)
      : OpenedFindToolbarTestsBase(windowTypeName, frameworkVersion)
    {
        [Test]
        public void Should_Navigate_As_Expected()
        {
            // due to Enhanced commands disabled until text.
            FocusFindTextAndSetText("a");

            List<(Action Navigation, Action FocusedElementAssertion)> navigationFocusAsserts = [
                (TabOnce, AssertIsPreviousButton),
                (TabOnce, AssertIsNextButton),
                (Typer.TypeLeft,  AssertIsPreviousButton),
                (Typer.TypeRight, AssertIsNextButton),
                (TabOnce, AssertIsDropDownMenuItem),

                // open the drop down, focuses first
                (DownOnce, AssertIsTopMenuItem),
                (TabOnce, AssertIsSecondMenuItem),
                (TabOnce, AssertIsThirdMenuItem),
                (TabOnce, AssertIsFourthMenuItem),
                (TabOnce, AssertIsBottomMenuItem),

                // tab cyles
                (TabOnce, AssertIsTopMenuItem),
                (Typer.TypeShiftTab, AssertIsBottomMenuItem),
                (Typer.TypeShiftTab, AssertIsFourthMenuItem),
                (UpOnce, AssertIsThirdMenuItem),
                (UpOnce, AssertIsSecondMenuItem),
                (UpOnce, AssertIsTopMenuItem),

                // up cycles
                (UpOnce, AssertIsBottomMenuItem),

                // down cycles
                (DownOnce, AssertIsTopMenuItem)
            ];

            navigationFocusAsserts.ForEach(navigationFocusAssert =>
            {
                navigationFocusAssert.Navigation();
                Thread.Sleep(100);
                navigationFocusAssert.FocusedElementAssertion();
            });

            static void TabOnce() => Typer.TypeTab();
            static void DownOnce() => Typer.TypeDown();
            static void UpOnce() => Typer.TypeUp();

            // todo navigation away
        }

        private void AssertIsDropDownMenuItem()
            => Assert.That(Window.Automation.FocusedElement().ControlType, Is.EqualTo(ControlType.MenuItem));

        private void AssertIsPreviousButton() => AssertButton(AutomationIds.FindPreviousButton);

        private void AssertIsNextButton() => AssertButton(AutomationIds.FindNextButton);

        private void AssertButton(string automationId) => AssertFocusedElement("Button", automationId);

        private void AssertIsTopMenuItem() => AssertMenuItem(AutomationIds.WholeWordMenuItem);

        private void AssertIsSecondMenuItem() => AssertMenuItem(AutomationIds.CaseMenuItem);

        private void AssertIsThirdMenuItem() => AssertMenuItem(AutomationIds.DiacriticMenuItem);

        private void AssertIsFourthMenuItem() => AssertMenuItem(AutomationIds.KashidaMenuItem);

        private void AssertIsBottomMenuItem() => AssertMenuItem(AutomationIds.AlefHamzaMenuItem);

        private void AssertMenuItem(string automationId) => AssertFocusedElement("MenuItem", automationId);

        private void AssertFocusedElement(string className, string automationId)
        {
            AutomationElement element = Window.Automation.FocusedElement();
            Assert.That(element.ClassName, Is.EqualTo(className));
            Assert.That(element.AutomationId, Is.EqualTo(automationId));
        }
    }
}
