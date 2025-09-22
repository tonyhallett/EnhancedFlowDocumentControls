using UIAutomationHelpers;
using UITests.NUnit;
using UITests.TestHelpers;

namespace UITests.Tests.Enhanced
{
    [FrameworkVersionsTest]
    internal sealed class OriginalDataContextTest(FrameworkVersion frameworkVersion)
        : FindToolBarTestsBase(DemoWindowTypeNames.OriginalDataContext, frameworkVersion)
    {
        [Test]
        public void Should_Be_What_Would_Have_Inherited() => TextBoxForegroundTester.AssertExpected(Window);

        [Test]
        public void Should_Require_Binding_Custom_Find_Control_Properties_To_FindToolbarViewModel_DataContext()
        {
            ControlFinder.FindFindButton(Window)!.Click();

            FindsTest("OriginalDataContext", "OriginalDataContext", Typer.TypeF3);
        }
    }
}
