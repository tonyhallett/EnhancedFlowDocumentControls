using FlaUI.Core.AutomationElements;
using UIAutomationHelpers;

namespace UITests.TestHelpers
{
    internal static class CommonHelpers
    {
        public static void ShowFindToolbarFirstTime(Window window)
        {
            AutomationElement? flowDocument = ControlFinder.FindFlowDocument(window);
            flowDocument!.Focus();

            Typer.TypeF3();
        }
    }
}
