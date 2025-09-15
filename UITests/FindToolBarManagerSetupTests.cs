using System.Windows.Controls;
using EnhancedFlowDocumentControls.FindToolBarControls;
using EnhancedFlowDocumentControls.Management;
using EnhancedFlowDocumentControls.ViewModel;
using Moq;

namespace UITests
{
    internal class FindToolBarManagerSetupTests
    {
        [Test]
        public void Should_Replace_EnhancedFlowDocumentControl_ToolBarHost_Field_With_Alerting_When_Setup()
        {
            var alertingFindToolBarHost = new AlertingFindToolBarHost();
            IEnhancedFlowDocumentControl enhancedFlowDocumentControl = new Mock<IEnhancedFlowDocumentControl>().Object;
            var mockFlowControlReflectorFactory = new Mock<IFlowControlReflectorFactory>();
            var mockFlowControlReflector = new Mock<IFlowControlReflector>();
            _ = mockFlowControlReflectorFactory
                .Setup(factory => factory.GetReflector(enhancedFlowDocumentControl))
                .Returns(mockFlowControlReflector.Object);

            var findToolbarManager = new FindToolBarManager(
                alertingFindToolBarHost,
                mockFlowControlReflectorFactory.Object,
                null,
                null);

            var customFindToolBar = new Button();
            findToolbarManager.Setup(enhancedFlowDocumentControl, customFindToolBar);

            mockFlowControlReflector.Verify(flowControlReflector => flowControlReflector.SetFindToolBarHost(enhancedFlowDocumentControl, alertingFindToolBarHost));
        }
    }

    internal class FindToolBarShowToolbarTests
    {
        [Test]
        public void Should_Set_The_ViewModel_On_IFindToolBarViewModelAware()
        {
            var alertingFindToolBarHost = new AlertingFindToolBarHost();
            IEnhancedFlowDocumentControl enhancedFlowDocumentControl = new Mock<IEnhancedFlowDocumentControl>().Object;
            var mockFlowControlReflectorFactory = new Mock<IFlowControlReflectorFactory>();
            var mockFlowControlReflector = new Mock<IFlowControlReflector>();
            _ = mockFlowControlReflectorFactory
                .Setup(factory => factory.GetReflector(enhancedFlowDocumentControl))
                .Returns(mockFlowControlReflector.Object);
            _ = mockFlowControlReflector.Setup(flowControlReflector => flowControlReflector.GetFindToolBarHost(enhancedFlowDocumentControl)).Returns(new Decorator());

            var originalFindToolbar = new ToolBar();
            var mockFindToolBarViewModelFactory = new Mock<IFindToolBarViewModelFactory>();
            IFindableToolBarViewModel findableToolBarViewModel = new Mock<IFindableToolBarViewModel>().Object;
            _ = mockFindToolBarViewModelFactory.Setup(findToolBarViewModelFactory => findToolBarViewModelFactory.Create(originalFindToolbar, null)).Returns(findableToolBarViewModel);
            var findToolbarManager = new FindToolBarManager(
                alertingFindToolBarHost,
                mockFlowControlReflectorFactory.Object,
                new Mock<IDocumentViewHelper>().Object,
                mockFindToolBarViewModelFactory.Object);

            var findToolBarViewModelAware = new FindToolBar();
            findToolbarManager.Setup(enhancedFlowDocumentControl, findToolBarViewModelAware);

            alertingFindToolBarHost.Child = originalFindToolbar;

            Assert.That(findToolBarViewModelAware.FindToolBarViewModel, Is.SameAs(findableToolBarViewModel));
        }

        [Test]
        public void Should_Set_The_DataContext_When_FindToolBar_Is_Not_IFindToolBarViewModelAware()
        {
            throw new NotImplementedException();
            // demo that it is live too
        }
    }
}
