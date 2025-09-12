using FlaUI.Core.AutomationElements;
using UIAutomationHelpers;

namespace UITests.TestHelpers
{
    internal static class MenuHelper
    {
        public static MenuItem SelectMatchCase(Window window)
        {
            MenuItem matchCaseMenuItem = GetMatchCaseMenuItem(window);
            _ = matchCaseMenuItem.Invoke();
            return matchCaseMenuItem;
        }

        public static MenuItem GetMatchCaseMenuItem(Window window)
        {
            Menu? findMenu = ControlFinder.FindFindMenu(window);
            MenuItem rootItem = findMenu!.Items[0];
            _ = rootItem.Expand();
            return rootItem.Items[1];
        }
    }
}
