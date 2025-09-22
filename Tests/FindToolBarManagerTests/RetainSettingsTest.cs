using System.Windows;
using System.Windows.Controls;
using EnhancedFlowDocumentControls.ViewModel;
using Moq;
using NUnit.Framework;

namespace Tests.FindToolBarManagerTests
{
    internal class RetainSettingsTest : TestsBase
    {
        private Mock<IFindableToolBarViewModel> _mockFindableToolBarViewModel;
        private IFindableToolBarViewModel _stubbedFindableToolBarViewModel;

        [SetUp]
        public void SetUp()
        {
            _mockFindableToolBarViewModel = new Mock<IFindableToolBarViewModel>();
            _stubbedFindableToolBarViewModel = _mockFindableToolBarViewModel.Object;
            _ = _mockFindableToolBarViewModel.SetupAllProperties();
            _ = MockFindToolBarViewModelFactory.Setup(findToolBarViewModelFactory
                => findToolBarViewModelFactory.Create(It.IsAny<ToolBar>(), It.IsAny<FrameworkElement>()))
                .Returns(_stubbedFindableToolBarViewModel);
        }

        [Test]
        public void Should_Not_By_Default()
        {
            SetupAndShowToolBar();
            CloseAndShow();

            AssertApplySettingsNotCalled();
        }

        private void AssertApplySettingsNotCalled()
            => _mockFindableToolBarViewModel.Verify(findableToolBarViewModel => findableToolBarViewModel.ApplySettings(It.IsAny<IFindToolBarSettings>()), Times.Never);

        [Test]
        public void Should_ApplySettings_With_Settings_From_Before_When_True()
        {
            FindToolBarManager.RetainSettings = true;
            SetupAndShowToolBar();

            // _stubbedFindableToolBarViewModel.IsSearchUp = true;
            _stubbedFindableToolBarViewModel.MatchKashida = true;
            _stubbedFindableToolBarViewModel.MatchWholeWord = true;
            _stubbedFindableToolBarViewModel.MatchCase = true;
            _stubbedFindableToolBarViewModel.MatchDiacritic = true;
            _stubbedFindableToolBarViewModel.MatchAlefHamza = true;
            _stubbedFindableToolBarViewModel.FindText = "abc";

            CloseAndShow();
            _mockFindableToolBarViewModel.Verify(
                findableToolBarViewModel => findableToolBarViewModel.ApplySettings(It.Is<IFindToolBarSettings>(
                    settings =>

                        // settings.IsSearchUp == true &&
                        settings.MatchKashida == true &&
                        settings.MatchWholeWord == true &&
                        settings.MatchCase == true &&
                        settings.MatchDiacritic == true &&
                        settings.MatchAlefHamza == true &&
                        settings.FindText == "abc")),
                Times.Once);
        }

        [Test]
        public void Should_Not_ApplySettings_From_Before_When_Set_From_True_To_False()
        {
            FindToolBarManager.RetainSettings = true;
            SetupAndShowToolBar();
            AlertingFindToolBarHost.Child = null;
            FindToolBarManager.RetainSettings = false;
            AlertingFindToolBarHost.Child = OriginalFindToolbar;

            AssertApplySettingsNotCalled();
        }

        private void CloseAndShow()
        {
            AlertingFindToolBarHost.Child = null;
            AlertingFindToolBarHost.Child = OriginalFindToolbar;
        }
    }
}
