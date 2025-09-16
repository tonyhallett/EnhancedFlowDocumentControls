using System;
using System.Windows;
using System.Windows.Controls;
using EnhancedFlowDocumentControls.FindToolBarControls;
using EnhancedFlowDocumentControls.FlowDocumentControls;
using EnhancedFlowDocumentControls.Management;
using EnhancedFlowDocumentControls.ViewModel;
using Moq;
using NUnit.Framework;

namespace Tests
{
    internal sealed class FindToolBarShowToolbarTests
    {
        [Test]
        [UiTest]
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
                mockFindToolBarViewModelFactory.Object,
                new Mock<IWpfUtilities>().Object);

            var findToolBarViewModelAware = new FindToolBar();
            findToolbarManager.Setup(enhancedFlowDocumentControl, findToolBarViewModelAware);

            alertingFindToolBarHost.Child = originalFindToolbar;

            Assert.That(findToolBarViewModelAware.FindToolBarViewModel, Is.SameAs(findableToolBarViewModel));
        }

        [Test]
        [UiTest]
        public void Should_Set_The_DataContext_When_FindToolBar_Is_Not_IFindToolBarViewModelAware()
        {
            var alertingFindToolBarHost = new AlertingFindToolBarHost();

            object originalDataContext = new object();
            EnhancedFlowDocumentReader enhancedFlowDocumentControl = new EnhancedFlowDocumentReader()
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
                mockFindToolBarViewModelFactory.Object,
                new Mock<IWpfUtilities>().Object);

            var findToolBar = new ToolBar();
            findToolbarManager.Setup(enhancedFlowDocumentControl, findToolBar);

            alertingFindToolBarHost.Child = originalFindToolbar;

            Assert.That(findToolBar.DataContext, Is.SameAs(findableToolBarViewModel));
        }

        [Test]
        [UiTest]
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
                mockFindToolBarViewModelFactory.Object,
                new Mock<IWpfUtilities>().Object);

            var findToolBar = new FindToolBar();
            findToolbarManager.Setup(enhancedFlowDocumentControl, findToolBar);

            alertingFindToolBarHost.Child = originalFindToolbar;

            Assert.That(originalHost.Child, Is.SameAs(findToolBar));
        }

        [Test]
        [UiTest]
        public void Should_Setup_The_Host()
        {
            var mockDocumentViewHelper = new Mock<IDocumentViewHelper>();
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
                mockDocumentViewHelper.Object,
                mockFindToolBarViewModelFactory.Object,
                new Mock<IWpfUtilities>().Object);

            var findToolBar = new FindToolBar();
            findToolbarManager.Setup(enhancedFlowDocumentControl, findToolBar);

            alertingFindToolBarHost.Child = originalFindToolbar;

            mockDocumentViewHelper.Verify(documentViewHelper => documentViewHelper.ToggleFindToolBarHost(originalHost, true));
        }

        [Test]
        [UiTest]
        public void Should_Focus_Find_TextBox_When_FindToolBar_Loaded()
        {
            Action<Action> dispatcher = (_) => { };
            var findToolBar = new FindToolBar();
            RoutedEventHandler loadedEventHandler = null;
            var mockWpfUtilities = new Mock<IWpfUtilities>();
            _ = mockWpfUtilities.Setup(wpfUtilities => wpfUtilities.AddLoadedEventHandler(findToolBar, It.IsAny<RoutedEventHandler>()))
                .Callback<FrameworkElement, RoutedEventHandler>((_, handler) => loadedEventHandler = handler);
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
                mockFindToolBarViewModelFactory.Object,
                mockWpfUtilities.Object,
                dispatcher);

            findToolbarManager.Setup(enhancedFlowDocumentControl, findToolBar);

            alertingFindToolBarHost.Child = originalFindToolbar;

            loadedEventHandler(null, null);
            mockWpfUtilities.Verify(wpfUtilities => wpfUtilities.FocusTextBox(dispatcher));
        }

        [Test]
        [UiTest]
        public void Should_Find_When_Find_TextBox_Return_KeyDown()
        {
            var findToolBar = new FindToolBar();
            RoutedEventHandler findToolBarLoadedEventHandler = null;
            Action findTextBoxKeyDownEnterOrExecuteHandler = null;
            var mockWpfUtilities = new Mock<IWpfUtilities>();
            _ = mockWpfUtilities.Setup(wpfUtilities => wpfUtilities.AddLoadedEventHandler(findToolBar, It.IsAny<RoutedEventHandler>()))
                .Callback<FrameworkElement, RoutedEventHandler>((_, handler) => findToolBarLoadedEventHandler = handler);
            _ = mockWpfUtilities.Setup(wpfUtilities => wpfUtilities.AddPreviewKeyDownEnterOrExecuteHandler(It.IsAny<Action>()))
               .Callback<Action>((handler) => findTextBoxKeyDownEnterOrExecuteHandler = handler);

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
            var mockFindableToolBarViewModel = new Mock<IFindableToolBarViewModel>();
            _ = mockFindToolBarViewModelFactory.Setup(findToolBarViewModelFactory => findToolBarViewModelFactory.Create(originalFindToolbar, null)).Returns(mockFindableToolBarViewModel.Object);
            var findToolbarManager = new FindToolBarManager(
                alertingFindToolBarHost,
                mockFlowControlReflectorFactory.Object,
                new Mock<IDocumentViewHelper>().Object,
                mockFindToolBarViewModelFactory.Object,
                mockWpfUtilities.Object,
                null);

            findToolbarManager.Setup(enhancedFlowDocumentControl, findToolBar);

            alertingFindToolBarHost.Child = originalFindToolbar;

            findToolBarLoadedEventHandler(null, null);
            findTextBoxKeyDownEnterOrExecuteHandler();

            mockFindableToolBarViewModel.Verify(findableToolBarViewModel => findableToolBarViewModel.Find());
        }
    }
}
