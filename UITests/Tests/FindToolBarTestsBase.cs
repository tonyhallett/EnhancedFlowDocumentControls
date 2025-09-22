using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Tools;
using FlaUI.TestUtilities;
using FlaUI.UIA3;
using UIAutomationHelpers;
using UITests.TestHelpers;

[assembly: RequiresThread(ApartmentState.STA)]

namespace UITests.Tests
{
    internal abstract class FindToolBarTestsBase(string windowTypeName, FrameworkVersion frameworkVersion = FrameworkVersion.Net472)
        : LongPathFailingTestScreenShotFixBase
    {
        private Window? _window;

        protected Window Window => _window!;

        protected bool IsNormal { get; private set; }

        protected override VideoRecordingMode VideoRecordingMode => VideoRecordingMode.NoVideo;

        protected override AutomationBase GetAutomation() => new UIA3Automation();

        protected override Application StartApplication()
        {
            if (!SolutionConfiguration.IsUITests)
            {
                throw new Exception("UITests must be run with the UITests solution configuration.");
            }

            _ = NativeMethods.SetProcessDPIAware();
            IsNormal = windowTypeName.StartsWith("Normal");
            Application application = DemoApplicationLauncher.Launch(frameworkVersion, windowTypeName);
            _window = application.GetMainWindow(Automation);
            return application;
        }

        protected AutomationElement? FindFindToolbar()
        {
            string automationId = IsNormal ? "FindToolbar" : "replacedFindToolBar";
            return ControlFinder.FindFindToolbar(Window, automationId);
        }

        protected void AssertShowsFindToolbar()
        {
            RetryResult<AutomationElement?> findToolBarResult = Retry.WhileNull(FindFindToolbar);
            AutomationElement? findToolbar = findToolBarResult.Result;
            Assert.That(findToolbar, Is.Not.Null, "Find toolbar should not be null");
        }

        protected void AssertClosed() => Assert.That(FindFindToolbar(), Is.Null, "Find toolbar should be null");

        protected void FindsTest(string findText, string expectedEnclosingWord, Action findAction)
        {
            FocusFindTextAndSetText(findText);

            findAction();

            AssertSelected(expectedEnclosingWord, findText);
        }

        protected TextBox FocusFindTextBox()
        {
            TextBox? findTextBox = Retry.WhileNull(() => ControlFinder.FindFindTextBox(Window)).Result;
            findTextBox!.Focus();
            return findTextBox;
        }

        protected void FocusFindTextAndSetText(string text)
        {
            TextBox findTextBox = FocusFindTextBox();
            findTextBox.Text = text;
        }

        protected void AssertSelected(string expectedEnclosingWord, string expectedSelectedText)
        {
            AutomationElement? document = ControlFinder.FindFlowDocument(Window);
            ITextRange[] selectionTextRanges = document!.Patterns.Text.Pattern.GetSelection();
            Assert.That(selectionTextRanges.Count, Is.EqualTo(1));
            ITextRange selectedTextRange = selectionTextRanges[0];
            Assert.That(selectedTextRange.GetText(-1), Is.EqualTo(expectedSelectedText));
            selectedTextRange.ExpandToEnclosingUnit(FlaUI.Core.Definitions.TextUnit.Word);
            Assert.That(selectedTextRange.GetText(-1), Is.EqualTo(expectedEnclosingWord));
        }
    }
}
