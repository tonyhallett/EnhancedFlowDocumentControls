using UIAutomationHelpers;
using UITests.NUnit;
using UITests.TestHelpers;

namespace UITests.Tests.Enhanced
{
    [FrameworkVersionsTest]
    internal sealed class FindToolBarViewModelAwareDataContextTest(FrameworkVersion frameworkVersion)
        : FindToolBarTestsBase(DemoWindowTypeNames.FindToolbarViewModelAwareDataContext, frameworkVersion)
    {
        [Test]
        public void Should_Preserve_DataContext() => TextBoxForegroundTester.AssertExpected(Window);
    }
}
