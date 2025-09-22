using FlaUI.Core.AutomationElements;
using FlaUI.Core.Tools;
using UIAutomationHelpers;
using UITests.NUnit;
using UITests.TestHelpers;

namespace UITests.Tests.Comparisons
{
    [CommonComparisonTest]
    internal sealed class CommonOpenedFindToolbarTests(string windowTypeName, FrameworkVersion frameworkVersion)
        : OpenedFindToolbarTestsBase(windowTypeName, frameworkVersion)
    {
        [Test]
        public void Should_Close_Find_Toolbar_When_Press_Escape()
        {
            Typer.TypeEsc();

            AssertClosed();
        }

        [Test]
        public void Should_Find_Up_When_ShiftF3()
        {
            SelectMiddle();
            FindsTest("s", "simple ", Typer.TypeShiftF3);
        }

        private void SelectMiddle()
        {
            FocusFindTextAndSetText("FlowDocument");
            Typer.TypeEnter();

            AssertSelected("FlowDocument ", "FlowDocument");
        }

        [Test]
        public void Should_Find_Down_When_F3()
        {
            SelectMiddle();

            FindsTest("s", "stylable ", Typer.TypeF3);
        }

        [Test]
        public void Should_Find_Down_When_FindDown_Button_Is_Keyboard_Enter()
            => FindsTest("s", "This ", () =>
            {
                Typer.TypeTab(2);
                Typer.TypeEnter();
            });

        [Test]
        public void Should_Find_Up_When_FindUp_Button_Is_Keyboard_Enter()
        {
            SelectMiddle();

            FindsTest("s", "simple ", () =>
            {
                Typer.TypeTab();
                Typer.TypeEnter();
            });
        }

        [Test]
        public void Should_Find_Down_When_Enter_First_Pressed() // search down is the default
            => FindsTest("s", "This ", Typer.TypeEnter);

        [Test]
        public void Should_Find_Last_Searched_Direction_When_Press_Enter()
        {
            // start at the far right
            FocusFindTextAndSetText("toolbar");
            Typer.TypeEnter();

            // switch from default
            FindsTest("s", "stylable ", Typer.TypeShiftF3);

            FindsTest("s", "simple ", Typer.TypeEnter);

            FindsTest("s", "is ", Typer.TypeEnter);

            // switch
            FindsTest("s", "simple ", Typer.TypeF3);

            FindsTest("s", "stylable ", Typer.TypeEnter);
        }

        [Test]
        public void Menu_Test()
        {
            const string cannotFindCaseSensitive = "Toolbar";
            FocusFindTextAndSetText(cannotFindCaseSensitive);
            _ = MenuHelper.SelectMatchCase(Window);
            Typer.TypeF3();

            AssertCannotFind(cannotFindCaseSensitive);
        }

        [Test]
        public void Should_Hide_ToolBar_When_Document_Is_Set_To_Null()
        {
            RadioButton? documentNullRadioButton = ControlFinder.FindDocumentNullRadioButton(Window);
            documentNullRadioButton!.IsChecked = true;
            AssertClosed();
        }

        private void AssertCannotFind(string findText)
        {
            RetryResult<Window?> findWindowResult = Retry.WhileNull(() => ControlFinder.FindCannotFindWindow(Window));
            Window? findWindow = findWindowResult.Result;
            TextBox? findWindowTextBox = findWindow!.FindFirstChild(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Text)).AsTextBox();
            Assert.That(findWindowTextBox, Is.Not.Null, "Find window text box should not be null");
            Assert.That(findWindowTextBox!.Text, Is.EqualTo($"Searched to the end of this document. Cannot find '{findText}'."));
            findWindow.Close();
        }
    }
}
