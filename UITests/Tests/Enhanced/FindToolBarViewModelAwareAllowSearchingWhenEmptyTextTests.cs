using UIAutomationHelpers;
using UITests.TestHelpers;

namespace UITests.Tests.Enhanced
{
    [TestFixture(DemoWindowTypeNames.FindToolBarViewModelAwareAllowSearchingWhenEmptyText, true)]
    [TestFixture(DemoWindowTypeNames.EnhancedFlowDocumentReader, false)]
    internal sealed class FindToolBarViewModelAwareAllowSearchingWhenEmptyTextTests(string windowTypeName, bool allowSearchingWhenEmptyText)
        : OpenedFindToolbarTestsBase(windowTypeName, FrameworkVersion.Net472)
    {
        [Test]
        public void Should_Enable_Buttons_If_AllowSearchingWhenEmptyText_Is_True()
            => AssertButtonsIsEnabled(allowSearchingWhenEmptyText);

        [Test]
        public void Should_Be_Enabled_When_Non_Empty_Text()
        {
            FocusFindTextAndSetText("text");
            AssertButtonsIsEnabled(true);
        }

        private void AssertButtonsIsEnabled(bool expectedIsEnabled)
            => Assert.Multiple(() =>
            {
                Assert.That(ControlFinder.FindFindPreviousButton(Window)!.IsEnabled, Is.EqualTo(expectedIsEnabled), "Find previous button enabled state is incorrect");
                Assert.That(ControlFinder.FindFindNextButton(Window)!.IsEnabled, Is.EqualTo(expectedIsEnabled), "Find next button enabled state is incorrect");
            });

    }
}
