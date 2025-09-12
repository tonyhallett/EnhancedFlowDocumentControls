using System.Drawing;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using UIAutomationHelpers;
using UITests.NUnit;

namespace UITests.Tests.Enhanced
{
    internal static class TextBoxForegroundTester
    {
        public static void AssertExpected(Window window)
        {
            ControlFinder.FindFindButton(window)!.Click();

            TextBox? findTextBox = ControlFinder.FindFindTextBox(window);

            AssertForegroundColorsEqual(findTextBox!, Color.FromArgb(0, Color.DarkBlue), window.Automation);

            RadioButton? pinkRadioButton = ControlFinder.FindPinkPaletteRadioButton(window);
            pinkRadioButton!.IsChecked = true;

            AssertForegroundColorsEqual(findTextBox!, Color.FromArgb(0, Color.DeepPink), window.Automation);

        }

        private static void AssertForegroundColorsEqual(TextBox textBox, Color expectedColor, AutomationBase automation)
        {
            ITextRange textRangePattern = textBox.Patterns.Text.Pattern.DocumentRange;
            int foregroundInt = (int)textRangePattern.GetAttributeValue(automation.TextAttributeLibrary.ForegroundColor);
            Color foregroundColor = Color.FromArgb(foregroundInt);
            Assert.That(foregroundColor.ToArgb(), Is.EqualTo(expectedColor.ToArgb()));
        }
    }
}
