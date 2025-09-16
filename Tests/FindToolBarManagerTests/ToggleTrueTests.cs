using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EnhancedFlowDocumentControls.Management;
using Moq;
using NUnit.Framework;

namespace Tests.FindToolBarManagerTests
{
    internal sealed class ToggleTrueTests : FindToolBarManagerTestsBase
    {
        [Test]
        public void Should_Set_The_ViewModel_On_IFindToolBarViewModelAware()
        {
            _ = MockFindToolBarViewModelFactory.Setup(findToolBarViewModelFactory
                => findToolBarViewModelFactory.Create(OriginalFindToolbar, null)).Returns(FindableToolBarViewModel);

            ShowToolBar(FindToolBarViewModelAware);

            Assert.That(FindToolBarViewModelAware.FindToolBarViewModel, Is.SameAs(FindableToolBarViewModel));
        }

        [Test]
        public void Should_Set_The_DataContext_When_FindToolBar_Is_Not_IFindToolBarViewModelAware()
        {
            _ = MockFindToolBarViewModelFactory.Setup(findToolBarViewModelFactory
                => findToolBarViewModelFactory.Create(OriginalFindToolbar, EnhancedFlowDocumentControl)).Returns(FindableToolBarViewModel);

            var findToolBarNotAware = new ToolBar();
            ShowToolBar(findToolBarNotAware);

            Assert.That(findToolBarNotAware.DataContext, Is.SameAs(FindableToolBarViewModel));
        }

        [Test]
        public void Should_Add_FindToolbar_As_Child_Of_Original_Host()
        {
            _ = MockFindToolBarViewModelFactory.Setup(findToolBarViewModelFactory => findToolBarViewModelFactory.Create(OriginalFindToolbar, null)).Returns(FindableToolBarViewModel);

            ShowToolBar();

            Assert.That(OriginalHost.Child, Is.SameAs(FindToolBarViewModelAware));
        }

        [Test]
        public void Should_ToggleFindToolBarHost_True()
        {
            _ = MockFindToolBarViewModelFactory.Setup(findToolBarViewModelFactory => findToolBarViewModelFactory.Create(OriginalFindToolbar, null)).Returns(FindableToolBarViewModel);

            ShowToolBar();

            MockDocumentViewHelper.Verify(documentViewHelper => documentViewHelper.ToggleFindToolBarHost(OriginalHost, true));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Should_Focus_Find_TextBox_When_FindToolBar_Loaded(bool providesDispatcher)
        {
            Action<Action> dispatcher = providesDispatcher ? (Action<Action>)((_) => { }) : null;
            _ = MockFindToolBarViewModelFactory.Setup(findToolBarViewModelFactory => findToolBarViewModelFactory.Create(OriginalFindToolbar, null)).Returns(FindableToolBarViewModel);
            var alertingFindToolBarHost = new AlertingFindToolBarHost();
            var findToolbarManager = new FindToolBarManager(
                alertingFindToolBarHost,
                FlowControlReflectorFactory,
                new Mock<IDocumentViewerHelper>().Object,
                MockFindToolBarViewModelFactory.Object,
                DummyWpfUtilities,
                dispatcher);

            ShowToolBar(null, new FindToolBarManagerAndAlertingHost(findToolbarManager, alertingFindToolBarHost));

            DummyWpfUtilities.InvokeLoaded();

            DummyWpfUtilities.Mock.Verify(wpfUtilities => wpfUtilities.FocusTextBox(dispatcher));
        }

        [Test]
        public void Should_Find_When_Find_TextBox_Return_KeyDown()
        {
            _ = MockFindToolBarViewModelFactory.Setup(findToolBarViewModelFactory
                => findToolBarViewModelFactory.Create(OriginalFindToolbar, null))
                .Returns(MockFindableToolBarViewModel.Object);

            ShowToolBar();

            DummyWpfUtilities.InvokeLoaded();
            DummyWpfUtilities.InvokeEnterOrExecute();

            MockFindableToolBarViewModel.Verify(findableToolBarViewModel => findableToolBarViewModel.Find());
        }

        internal sealed class DummyWPfUtilities : IWpfUtilities
        {
            private readonly IWpfUtilities _wpfUtilities;
            private RoutedEventHandler _loadedHandler;
            private Action _previewKeyDownEnterOrExecuteHandler;

            public Mock<IWpfUtilities> Mock { get; }

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

            public void InvokeLoaded() => _loadedHandler(null, null);

            public void InvokeEnterOrExecute() => _previewKeyDownEnterOrExecuteHandler();
        }
    }
}
