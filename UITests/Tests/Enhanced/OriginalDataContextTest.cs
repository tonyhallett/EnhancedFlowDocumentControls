using UIAutomationHelpers;
using UITests.NUnit;
using UITests.TestHelpers;

namespace UITests.Tests.Enhanced
{
    [FrameworkVersionsTest]
    internal sealed class OriginalDataContextTest(FrameworkVersion frameworkVersion)
        : FindToolBarTestsBase(DemoWindowTypeNames.OriginalDataContext, frameworkVersion)
    {
        private bool _takeScreenshots = true;

        protected override bool TakeScreenshots => _takeScreenshots;

        [Test]
        public void Should_Be_What_Would_Have_Inherited() => TextBoxForegroundTester.AssertExpected(Window);

        [Test]
        public void Should_Require_Binding_Custom_Find_Control_Properties_To_FindToolbarViewModel_DataContext()
        {
            /*
                path too long for this test
                https://github.com/nunit/nunit/issues/4353
                FlaUITestBase.UITestBaseTearDown => TakeScreenShot => TestContext.AddTestAttachment
                if there is an exception CloseApplication in teardown is not run.
            */
            _takeScreenshots = false;

            ControlFinder.FindFindButton(Window)!.Click();

            FindsTest("OriginalDataContext", "OriginalDataContext", Typer.TypeF3);
        }
    }
}
