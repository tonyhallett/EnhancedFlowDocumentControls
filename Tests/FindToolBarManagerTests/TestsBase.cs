using System.Threading;
using System.Windows;
using System.Windows.Controls;
using EnhancedFlowDocumentControls.FindToolBarControls;
using EnhancedFlowDocumentControls.FlowDocumentControls;
using EnhancedFlowDocumentControls.Management;
using EnhancedFlowDocumentControls.ViewModel;
using Moq;
using NUnit.Framework;
using static Tests.FindToolBarManagerTests.ToggleTrueTests;

namespace Tests.FindToolBarManagerTests
{
    [RequiresThread(ApartmentState.STA)]
    internal abstract class TestsBase
    {
        protected ToolBar OriginalFindToolbar { get; } = new ToolBar();

        protected DummyWPfUtilities DummyWpfUtilities { get; private set; }

        protected FindToolBarManager FindToolBarManager { get; private set; }

        protected AlertingFindToolBarHost AlertingFindToolBarHost { get; private set; }

        protected EnhancedFlowDocumentReader EnhancedFlowDocumentControl { get; private set; }

        protected IFlowControlReflectorFactory FlowControlReflectorFactory { get; private set; }

        protected Mock<IFindToolBarViewModelFactory> MockFindToolBarViewModelFactory { get; private set; }

        protected Mock<IFindableToolBarViewModel> MockFindableToolBarViewModel { get; private set; }

        protected Mock<IFlowControlReflector> MockFlowControlReflector { get; private set; }

        protected IFindableToolBarViewModel FindableToolBarViewModel { get; private set; }

        protected Mock<IDocumentViewerHelper> MockDocumentViewHelper { get; private set; }

        protected FindToolBar FindToolBarViewModelAware { get; private set; }

        protected Decorator OriginalHost { get; private set; }

        [SetUp]
        public void BaseSetUp()
        {
            AlertingFindToolBarHost = new AlertingFindToolBarHost();
            EnhancedFlowDocumentControl = new EnhancedFlowDocumentReader();

            var mockFlowControlReflectorFactory = new Mock<IFlowControlReflectorFactory>();
            MockFlowControlReflector = new Mock<IFlowControlReflector>();
            OriginalHost = new Decorator();
            _ = MockFlowControlReflector.Setup(flowControlReflector => flowControlReflector.GetFindToolBarHost(EnhancedFlowDocumentControl)).Returns(OriginalHost);
            _ = mockFlowControlReflectorFactory
                .Setup(factory => factory.GetReflector(EnhancedFlowDocumentControl))
                .Returns(MockFlowControlReflector.Object);
            FlowControlReflectorFactory = mockFlowControlReflectorFactory.Object;

            MockFindToolBarViewModelFactory = new Mock<IFindToolBarViewModelFactory>();
            MockFindableToolBarViewModel = new Mock<IFindableToolBarViewModel>();
            FindableToolBarViewModel = MockFindableToolBarViewModel.Object;
            MockDocumentViewHelper = new Mock<IDocumentViewerHelper>();
            FindToolBarViewModelAware = new FindToolBar();
            DummyWpfUtilities = new DummyWPfUtilities();
            FindToolBarManager = new FindToolBarManager(
                AlertingFindToolBarHost,
                FlowControlReflectorFactory,
                MockDocumentViewHelper.Object,
                MockFindToolBarViewModelFactory.Object,
                DummyWpfUtilities,
                null);
        }

        protected void SetupAndShowToolBar(FrameworkElement customFindToolBar = null, FindToolBarManagerAndAlertingHost showToolbarParameters = null)
        {
            customFindToolBar = customFindToolBar ?? FindToolBarViewModelAware;
            DummyWpfUtilities.ExpectedCustomFindToolBar = customFindToolBar;

            FindToolBarManager findToolBarManager = showToolbarParameters?.FindToolBarManager ?? FindToolBarManager;
            findToolBarManager.Setup(EnhancedFlowDocumentControl, customFindToolBar);

            AlertingFindToolBarHost alertingFindToolBarHost = showToolbarParameters?.AlertingFindToolBarHost ?? AlertingFindToolBarHost;
            alertingFindToolBarHost.Child = OriginalFindToolbar;
        }
    }
}
