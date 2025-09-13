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
            FocusFindTextAndSetText("a");

            Typer.TypeTab();
            AssertPreviousButton();

            Typer.TypeTab();
            AssertNextButton();

            Typer.TypeLeft();
            AssertPreviousButton();

            Typer.TypeRight();
            AssertNextButton();

            Typer.TypeTab();
            AssertDromDownMenuItem();

            Typer.TypeDown(); // open the drop down, focuses first
            AssertTopMenuItem();

            Typer.TypeTab();
            AssertSecondMenuItem();

            Typer.TypeTab();
            AssertThirdMenuItem();

            Typer.TypeTab();
            AssertFourthMenuItem();

            Typer.TypeTab();
            AssertBottomMenuItem();

            // tab cyles
            Typer.TypeTab();
            AssertTopMenuItem();

            Typer.TypeShiftTab();
            AssertBottomMenuItem();

            Typer.TypeShiftTab();
            AssertFourthMenuItem();

            Typer.TypeUp();
            AssertThirdMenuItem();

            Typer.TypeUp();
            AssertSecondMenuItem();

            Typer.TypeUp();
            AssertTopMenuItem();

            // up cycles
            Typer.TypeUp();
            AssertBottomMenuItem();

            Typer.TypeDown();
            AssertTopMenuItem();

            // todo navigation away
        }

        private void AssertDromDownMenuItem()
            => Assert.That(Window.Automation.FocusedElement().ControlType, Is.EqualTo(ControlType.MenuItem));

        private void AssertPreviousButton() => AssertButton("FindPreviousButton");

        private void AssertNextButton() => AssertButton("FindNextButton");

        private void AssertButton(string automationId) => AssertFocusedElement("Button", automationId);

        private void AssertTopMenuItem() => AssertMenuItem("OptionsWholeWordMenuItem");

        private void AssertSecondMenuItem() => AssertMenuItem("OptionsCaseMenuItem");

        private void AssertThirdMenuItem() => AssertMenuItem("OptionsDiacriticMenuItem");

        private void AssertFourthMenuItem() => AssertMenuItem("OptionsKashidaMenuItem");

        private void AssertBottomMenuItem() => AssertMenuItem("OptionsAlefHamzaMenuItem");

        private void AssertMenuItem(string automationId) => AssertFocusedElement("MenuItem", automationId);

        private void AssertFocusedElement(string className, string automationId)
        {
            AutomationElement element = Window.Automation.FocusedElement();
            Assert.That(element.ClassName, Is.EqualTo(className));
            Assert.That(element.AutomationId, Is.EqualTo(automationId));
        }
    }
}
