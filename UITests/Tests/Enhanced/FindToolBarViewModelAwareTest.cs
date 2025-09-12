using UIAutomationHelpers;
using UITests.NUnit;

namespace UITests.Tests.Enhanced
{
    [FrameworkVersionsTest]
    internal sealed class FindToolBarViewModelAwareTest(FrameworkVersion frameworkVersion)
        : FindToolBarTestsBase(DemoWindowTypeNames.FindToolbarViewModelAware, frameworkVersion)
    {
        [Test]
        public void Should_Preserve_DataContext() => TextBoxForegroundTester.AssertExpected(Window);
    }
}
