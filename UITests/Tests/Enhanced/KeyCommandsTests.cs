using UIAutomationHelpers;
using UITests.NUnit;
using UITests.TestHelpers;

namespace UITests.Tests.Enhanced
{
    [FrameworkVersionsTest]
    internal sealed class KeyCommandsTests(FrameworkVersion frameworkVersion)
        : FindToolBarTestsBase(DemoWindowTypeNames.InputBindings, frameworkVersion)
    {
        [Test]
        public void Should_Execute_The_KeyBindings_In_InputBindings_When_Pressed()
        {
            FlaUI.Core.AutomationElements.AutomationElement? flowDocument = ControlFinder.FindFlowDocument(Window);
            flowDocument!.Focus();

            Typer.TypeCtrlR();

            AssertShowsFindToolbar();
        }
    }
}
