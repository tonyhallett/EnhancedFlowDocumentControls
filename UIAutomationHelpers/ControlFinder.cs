using FlaUI.Core.AutomationElements;

namespace UIAutomationHelpers
{
    public static class ControlFinder
    {
        public static Button? FindFindButton(Window window)
            => window.FindFirstDescendant(cf => cf.ByAutomationId("FindButton")).AsButton();

        public static AutomationElement? FindFindToolbar(Window window, string automationId)
            => window.FindFirstDescendant(cf => cf.ByAutomationId(automationId));

        public static TextBox? FindFindTextBox(Window window)
           => window.FindFirstDescendant(cf => cf.ByClassName("TextBox")).AsTextBox();

        public static AutomationElement? FindFlowDocument(Window window)
            => window.FindFirstDescendant(cf => cf.ByClassName("Document"));

        public static Menu? FindFindMenu(Window window)
            => window.FindFirstDescendant(cf => cf.ByClassName("Menu")).AsMenu();

        public static AutomationElement? FindFlowDocumentReader(Window window)
            => window!.FindFirstDescendant(cf => cf.ByClassName("FlowDocumentReader"));

        public static Window? FindCannotFindWindow(Window window)
            => window.FindFirstDescendant(cf => cf.ByName("Find")).AsWindow();

        public static RadioButton? FindFindEnabledRadioButton(Window window)
            => window.FindFirstDescendant(cf => cf.ByName("Find Enabled")).AsRadioButton();

        public static RadioButton? FindFindDisabledRadioButton(Window window)
            => window.FindFirstDescendant(cf => cf.ByName("Find Disabled")).AsRadioButton();

        public static RadioButton? FindDocumentNotNullRadioButton(Window window)
            => window.FindFirstDescendant(cf => cf.ByName("Document Not Null")).AsRadioButton();

        public static RadioButton? FindDocumentNullRadioButton(Window window)
            => window.FindFirstDescendant(cf => cf.ByName("Document Null")).AsRadioButton();

        public static RadioButton? FindPinkPaletteRadioButton(Window window)
            => window.FindFirstDescendant(cf => cf.ByName("Pink")).AsRadioButton();

        public static Button? FindFindPreviousButton(Window window)
            => window.FindFirstDescendant(cf => cf.ByAutomationId(AutomationIds.FindPreviousButton)).AsButton();

        public static Button? FindFindNextButton(Window window)
            => window.FindFirstDescendant(cf => cf.ByAutomationId(AutomationIds.FindNextButton)).AsButton();
    }
}
