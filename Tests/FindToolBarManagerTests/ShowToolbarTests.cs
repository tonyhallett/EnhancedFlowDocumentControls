using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EnhancedFlowDocumentControls.FindToolBarControls;
using EnhancedFlowDocumentControls.FlowDocumentControls;
using EnhancedFlowDocumentControls.Management;
using EnhancedFlowDocumentControls.ViewModel;
using Moq;
using NUnit.Framework;

namespace Tests.FindToolBarManagerTests
{
    [RequiresThread(ApartmentState.STA)]
    internal sealed class ShowToolbarTests
    {
        private readonly ToolBar _originalFindToolbar = new ToolBar();
        private DummyWPfUtilities _dummyWpfUtilities;
        private FindToolBarManager _findToolBarManager;
        private AlertingFindToolBarHost _alertingFindToolBarHost;
        private EnhancedFlowDocumentReader _enhancedFlowDocumentControl;
        private IFlowControlReflectorFactory _flowControlReflectorFactory;
        private Mock<IFindToolBarViewModelFactory> _mockFindToolBarViewModelFactory;
        private Mock<IFindableToolBarViewModel> _mockFindableToolBarViewModel;
        private Mock<IFlowControlReflector> _mockFlowControlReflector;
        private IFindableToolBarViewModel _findableToolBarViewModel;
        private Mock<IDocumentViewHelper> _mockDocumentViewHelper;
        private FindToolBar _findToolBarViewModelAware;
        private Decorator _originalHost;

        [SetUp]
        public void SetUp()
        {
            _alertingFindToolBarHost = new AlertingFindToolBarHost();
            _enhancedFlowDocumentControl = new EnhancedFlowDocumentReader();

            var mockFlowControlReflectorFactory = new Mock<IFlowControlReflectorFactory>();
            _mockFlowControlReflector = new Mock<IFlowControlReflector>();
            _originalHost = new Decorator();
            _ = _mockFlowControlReflector.Setup(flowControlReflector => flowControlReflector.GetFindToolBarHost(_enhancedFlowDocumentControl)).Returns(_originalHost);
            _ = mockFlowControlReflectorFactory
                .Setup(factory => factory.GetReflector(_enhancedFlowDocumentControl))
                .Returns(_mockFlowControlReflector.Object);
            _flowControlReflectorFactory = mockFlowControlReflectorFactory.Object;

            _mockFindToolBarViewModelFactory = new Mock<IFindToolBarViewModelFactory>();
            _mockFindableToolBarViewModel = new Mock<IFindableToolBarViewModel>();
            _findableToolBarViewModel = _mockFindableToolBarViewModel.Object;
            _mockDocumentViewHelper = new Mock<IDocumentViewHelper>();
            _findToolBarViewModelAware = new FindToolBar();
            _dummyWpfUtilities = new DummyWPfUtilities();
            _findToolBarManager = new FindToolBarManager(
                _alertingFindToolBarHost,
                _flowControlReflectorFactory,
                _mockDocumentViewHelper.Object,
                _mockFindToolBarViewModelFactory.Object,
                _dummyWpfUtilities,
                null);
        }

        private class FindToolBarManagerAlertingHost
        {
            public FindToolBarManagerAlertingHost(FindToolBarManager findToolBarManager, AlertingFindToolBarHost alertingFindToolBarHost)
            {
                FindToolBarManager = findToolBarManager;
                AlertingFindToolBarHost = alertingFindToolBarHost;
            }

            public FindToolBarManager FindToolBarManager { get; }

            public AlertingFindToolBarHost AlertingFindToolBarHost { get; }
        }

        private void ShowToolBar(FrameworkElement customFindToolBar = null, FindToolBarManagerAlertingHost showToolbarParameters = null)
        {
            customFindToolBar = customFindToolBar ?? _findToolBarViewModelAware;
            _dummyWpfUtilities.ExpectedCustomFindToolBar = customFindToolBar;

            FindToolBarManager findToolBarManager = showToolbarParameters?.FindToolBarManager ?? _findToolBarManager;
            findToolBarManager.Setup(_enhancedFlowDocumentControl, customFindToolBar);

            AlertingFindToolBarHost alertingFindToolBarHost = showToolbarParameters?.AlertingFindToolBarHost ?? _alertingFindToolBarHost;
            alertingFindToolBarHost.Child = _originalFindToolbar;
        }

        [Test]
        public void Should_Set_The_ViewModel_On_IFindToolBarViewModelAware()
        {
            _ = _mockFindToolBarViewModelFactory.Setup(findToolBarViewModelFactory
                => findToolBarViewModelFactory.Create(_originalFindToolbar, null)).Returns(_findableToolBarViewModel);

            ShowToolBar(_findToolBarViewModelAware);

            Assert.That(_findToolBarViewModelAware.FindToolBarViewModel, Is.SameAs(_findableToolBarViewModel));
        }

        [Test]
        public void Should_Set_The_DataContext_When_FindToolBar_Is_Not_IFindToolBarViewModelAware()
        {
            _ = _mockFindToolBarViewModelFactory.Setup(findToolBarViewModelFactory
                => findToolBarViewModelFactory.Create(_originalFindToolbar, _enhancedFlowDocumentControl)).Returns(_findableToolBarViewModel);

            var findToolBarNotAware = new ToolBar();
            ShowToolBar(findToolBarNotAware);

            Assert.That(findToolBarNotAware.DataContext, Is.SameAs(_findableToolBarViewModel));
        }

        [Test]
        public void Should_Add_FindToolbar_As_Child_Of_Original_Host()
        {
            _ = _mockFindToolBarViewModelFactory.Setup(findToolBarViewModelFactory => findToolBarViewModelFactory.Create(_originalFindToolbar, null)).Returns(_findableToolBarViewModel);

            ShowToolBar();

            Assert.That(_originalHost.Child, Is.SameAs(_findToolBarViewModelAware));
        }

        [Test]
        public void Should_Setup_The_Host()
        {
            _ = _mockFindToolBarViewModelFactory.Setup(findToolBarViewModelFactory => findToolBarViewModelFactory.Create(_originalFindToolbar, null)).Returns(_findableToolBarViewModel);

            ShowToolBar();

            _mockDocumentViewHelper.Verify(documentViewHelper => documentViewHelper.ToggleFindToolBarHost(_originalHost, true));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Should_Focus_Find_TextBox_When_FindToolBar_Loaded(bool providesDispatcher)
        {
            Action<Action> dispatcher = providesDispatcher ? (Action<Action>)((_) => { }) : null;
            _ = _mockFindToolBarViewModelFactory.Setup(findToolBarViewModelFactory => findToolBarViewModelFactory.Create(_originalFindToolbar, null)).Returns(_findableToolBarViewModel);
            var alertingFindToolBarHost = new AlertingFindToolBarHost();
            var findToolbarManager = new FindToolBarManager(
                alertingFindToolBarHost,
                _flowControlReflectorFactory,
                new Mock<IDocumentViewHelper>().Object,
                _mockFindToolBarViewModelFactory.Object,
                _dummyWpfUtilities,
                dispatcher);

            ShowToolBar(null, new FindToolBarManagerAlertingHost(findToolbarManager, alertingFindToolBarHost));

            _dummyWpfUtilities.InvokeLoaded();

            _dummyWpfUtilities.Mock.Verify(wpfUtilities => wpfUtilities.FocusTextBox(dispatcher));
        }

        [Test]
        public void Should_Find_When_Find_TextBox_Return_KeyDown()
        {
            _ = _mockFindToolBarViewModelFactory.Setup(findToolBarViewModelFactory
                => findToolBarViewModelFactory.Create(_originalFindToolbar, null))
                .Returns(_mockFindableToolBarViewModel.Object);

            ShowToolBar();

            _dummyWpfUtilities.InvokeLoaded();
            _dummyWpfUtilities.InvokeEnterOrExecute();

            _mockFindableToolBarViewModel.Verify(findableToolBarViewModel => findableToolBarViewModel.Find());
        }

        internal sealed class DummyWPfUtilities : IWpfUtilities
        {
            private readonly IWpfUtilities _wpfUtilities;
            private RoutedEventHandler _loadedHandler;
            private Action _previewKeyDownEnterOrExecuteHandler;

            public Mock<IWpfUtilities> Mock { get; }

            public F3KeyType F3KeyType { get; set; }

            public FrameworkElement ExpectedCustomFindToolBar { get; set; }

            public DummyWPfUtilities()
            {
                Mock = new Mock<IWpfUtilities>();
                _wpfUtilities = Mock.Object;
            }

            public void AddLoadedEventHandler(FrameworkElement customFindToolBar, RoutedEventHandler loadedHandler)
            {
                Assert.That(customFindToolBar, Is.SameAs(ExpectedCustomFindToolBar));
                _loadedHandler = loadedHandler;
                _wpfUtilities.AddLoadedEventHandler(customFindToolBar, loadedHandler);
            }

            public void AddPreviewKeyDownEnterOrExecuteHandler(Action handler) => _previewKeyDownEnterOrExecuteHandler = handler;

            public void Clear() => _wpfUtilities.Clear();

            public void FocusTextBox(Action<Action> dispatcher) => _wpfUtilities.FocusTextBox(dispatcher);

            public F3KeyType GetF3KeyType(KeyEventArgs e) => F3KeyType;

            public void InvokeLoaded() => _loadedHandler(null, null);

            public void InvokeEnterOrExecute() => _previewKeyDownEnterOrExecuteHandler();
        }
    }
}
