using System.Drawing;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Tools;
using UIAutomationHelpers;

namespace UITests.Tests.Enhanced
{
    internal static class TextBoxForegroundTester
    {
        public static void AssertExpected(Window window)
        {
            ControlFinder.FindFindButton(window)!.Click();

            TextBox? findTextBox = Retry.WhileNull(() => ControlFinder.FindFindTextBox(window)).Result;

            AssertForegroundColorsEqual(findTextBox!, Color.FromArgb(0, Color.DarkBlue), window.Automation);

            RadioButton? pinkRadioButton = Retry.WhileNull(() => ControlFinder.FindPinkPaletteRadioButton(window)).Result;
            pinkRadioButton!.IsChecked = true;

            _ = Retry.WhileException(() => AssertForegroundColorsEqual(findTextBox!, Color.FromArgb(0, Color.DeepPink), window.Automation), throwOnTimeout: true);

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
