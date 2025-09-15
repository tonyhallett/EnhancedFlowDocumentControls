using System.Windows.Controls;
using EnhancedFlowDocumentControls.FindToolBarControls;
using EnhancedFlowDocumentControls.FlowDocumentControls;
using EnhancedFlowDocumentControls.Management;
using EnhancedFlowDocumentControls.ViewModel;
using Moq;

namespace UITests
{

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
            var alertingFindToolBarHost = new AlertingFindToolBarHost();

            object originalDataContext = new();
            EnhancedFlowDocumentReader enhancedFlowDocumentControl = new()
            {
                DataContext = originalDataContext,
            };

            var mockFlowControlReflectorFactory = new Mock<IFlowControlReflectorFactory>();
            var mockFlowControlReflector = new Mock<IFlowControlReflector>();
            _ = mockFlowControlReflectorFactory
                .Setup(factory => factory.GetReflector(enhancedFlowDocumentControl))
                .Returns(mockFlowControlReflector.Object);
            _ = mockFlowControlReflector.Setup(flowControlReflector => flowControlReflector.GetFindToolBarHost(enhancedFlowDocumentControl)).Returns(new Decorator());

            var originalFindToolbar = new ToolBar();
            var mockFindToolBarViewModelFactory = new Mock<IFindToolBarViewModelFactory>();
            IFindableToolBarViewModel findableToolBarViewModel = new Mock<IFindableToolBarViewModel>().Object;
            _ = mockFindToolBarViewModelFactory.Setup(findToolBarViewModelFactory => findToolBarViewModelFactory.Create(originalFindToolbar, enhancedFlowDocumentControl)).Returns(findableToolBarViewModel);
            var findToolbarManager = new FindToolBarManager(
                alertingFindToolBarHost,
                mockFlowControlReflectorFactory.Object,
                new Mock<IDocumentViewHelper>().Object,
                mockFindToolBarViewModelFactory.Object);

            var findToolBar = new ToolBar();
            findToolbarManager.Setup(enhancedFlowDocumentControl, findToolBar);

            alertingFindToolBarHost.Child = originalFindToolbar;

            Assert.That(findToolBar.DataContext, Is.SameAs(findableToolBarViewModel));
        }

        [Test]
        public void Should_Add_FindToolbar_As_Child_Of_Original_Host()
        {
            var alertingFindToolBarHost = new AlertingFindToolBarHost();
            IEnhancedFlowDocumentControl enhancedFlowDocumentControl = new Mock<IEnhancedFlowDocumentControl>().Object;
            var mockFlowControlReflectorFactory = new Mock<IFlowControlReflectorFactory>();
            var mockFlowControlReflector = new Mock<IFlowControlReflector>();
            _ = mockFlowControlReflectorFactory
                .Setup(factory => factory.GetReflector(enhancedFlowDocumentControl))
                .Returns(mockFlowControlReflector.Object);
            var originalHost = new Decorator();
            _ = mockFlowControlReflector.Setup(flowControlReflector => flowControlReflector.GetFindToolBarHost(enhancedFlowDocumentControl)).Returns(originalHost);

            var originalFindToolbar = new ToolBar();
            var mockFindToolBarViewModelFactory = new Mock<IFindToolBarViewModelFactory>();
            IFindableToolBarViewModel findableToolBarViewModel = new Mock<IFindableToolBarViewModel>().Object;
            _ = mockFindToolBarViewModelFactory.Setup(findToolBarViewModelFactory => findToolBarViewModelFactory.Create(originalFindToolbar, null)).Returns(findableToolBarViewModel);
            var findToolbarManager = new FindToolBarManager(
                alertingFindToolBarHost,
                mockFlowControlReflectorFactory.Object,
                new Mock<IDocumentViewHelper>().Object,
                mockFindToolBarViewModelFactory.Object);

            var findToolBar = new FindToolBar();
            findToolbarManager.Setup(enhancedFlowDocumentControl, findToolBar);

            alertingFindToolBarHost.Child = originalFindToolbar;

            Assert.That(originalHost.Child, Is.SameAs(findToolBar));
        }
    }
}
