using System;
using System.Windows;
using System.Windows.Controls;
using EnhancedFlowDocumentControls.Management;
using Moq;
using NUnit.Framework;

namespace Tests.FindToolBarManagerTests
{
    internal sealed class ToggleTrueTests : TestsBase
    {
        [SetUp]
        public void SetUp() => DummyWpfUtilities.FoundFindTextBox = true;

        [Test]
        public void Should_Set_The_ViewModel_On_IFindToolBarViewModelAware()
        {
            _ = MockFindToolBarViewModelFactory.Setup(findToolBarViewModelFactory
                => findToolBarViewModelFactory.Create(OriginalFindToolbar, null)).Returns(FindableToolBarViewModel);

            SetupAndShowToolBar(FindToolBarViewModelAware);

            Assert.That(FindToolBarViewModelAware.FindToolBarViewModel, Is.SameAs(FindableToolBarViewModel));
        }

        [Test]
        public void Should_Set_The_DataContext_When_FindToolBar_Is_Not_IFindToolBarViewModelAware()
        {
            _ = MockFindToolBarViewModelFactory.Setup(findToolBarViewModelFactory
                => findToolBarViewModelFactory.Create(OriginalFindToolbar, EnhancedFlowDocumentControl)).Returns(FindableToolBarViewModel);

            var findToolBarNotAware = new ToolBar();
            SetupAndShowToolBar(findToolBarNotAware);

            Assert.That(findToolBarNotAware.DataContext, Is.SameAs(FindableToolBarViewModel));
        }

        [Test]
        public void Should_Add_FindToolbar_As_Child_Of_Original_Host()
        {
            _ = MockFindToolBarViewModelFactory.Setup(findToolBarViewModelFactory => findToolBarViewModelFactory.Create(OriginalFindToolbar, null)).Returns(FindableToolBarViewModel);

            SetupAndShowToolBar();

            Assert.That(OriginalHost.Child, Is.SameAs(FindToolBarViewModelAware));
        }

        [Test]
        public void Should_ToggleFindToolBarHost_True()
        {
            _ = MockFindToolBarViewModelFactory.Setup(findToolBarViewModelFactory => findToolBarViewModelFactory.Create(OriginalFindToolbar, null)).Returns(FindableToolBarViewModel);

            SetupAndShowToolBar();

            MockDocumentViewHelper.Verify(documentViewHelper => documentViewHelper.ToggleFindToolBarHost(OriginalHost, true));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Should_Set_The_Dispatcher(bool providesDispatcher)
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

            Assert.That(DummyWpfUtilities.Dispatcher, Is.SameAs(dispatcher));
        }

        [Test]
        public void Should_Focus_Find_TextBox_When_Found()
        {
            _ = MockFindToolBarViewModelFactory.Setup(findToolBarViewModelFactory => findToolBarViewModelFactory.Create(OriginalFindToolbar, null)).Returns(FindableToolBarViewModel);
            var alertingFindToolBarHost = new AlertingFindToolBarHost();
            var findToolbarManager = new FindToolBarManager(
                alertingFindToolBarHost,
                FlowControlReflectorFactory,
                new Mock<IDocumentViewerHelper>().Object,
                MockFindToolBarViewModelFactory.Object,
                DummyWpfUtilities,
                null);

            SetupAndShowToolBar(null, new FindToolBarManagerAndAlertingHost(findToolbarManager, alertingFindToolBarHost));

            DummyWpfUtilities.Mock.Verify(wpfUtilities => wpfUtilities.FindTextBox(FindToolBarViewModelAware, It.IsAny<Action>()));
            DummyWpfUtilities.Mock.Verify(wpfUtilities => wpfUtilities.FocusTextBox());
        }

        [Test]
        public void Should_Find_When_Find_TextBox_Return_KeyDown()
        {
            _ = MockFindToolBarViewModelFactory.Setup(findToolBarViewModelFactory
                => findToolBarViewModelFactory.Create(OriginalFindToolbar, null))
                .Returns(MockFindableToolBarViewModel.Object);

            SetupAndShowToolBar();

            DummyWpfUtilities.InvokeEnterOrExecute();

            MockFindableToolBarViewModel.Verify(findableToolBarViewModel => findableToolBarViewModel.Find());
        }

        internal sealed class DummyWPfUtilities : IWpfUtilities
        {
            private readonly IWpfUtilities _wpfUtilities;
            private Action _previewKeyDownEnterOrExecuteHandler;

            public Mock<IWpfUtilities> Mock { get; }

            public FrameworkElement ExpectedCustomFindToolBar { get; set; }

            public Action<Action> Dispatcher { get; set; }

            public bool FoundFindTextBox { get; set; }

            public DummyWPfUtilities()
            {
                Mock = new Mock<IWpfUtilities>();
                _wpfUtilities = Mock.Object;
            }

            public void AddPreviewKeyDownEnterOrExecuteHandler(Action handler) => _previewKeyDownEnterOrExecuteHandler = handler;

            public void Clear() => _wpfUtilities.Clear();

            public void FocusTextBox() => _wpfUtilities.FocusTextBox();

            public void InvokeEnterOrExecute() => _previewKeyDownEnterOrExecuteHandler();

            public void FindTextBox(FrameworkElement customFindToolBar, Action callback)
            {
                _wpfUtilities.FindTextBox(customFindToolBar, callback);
                if (!FoundFindTextBox)
                {
                    return;
                }

                callback();
            }

            public string GetFocusedElementName(DependencyObject element) => throw new NotImplementedException();

            public void TryDispatchSetFocusedCustomFindToolBarDescendentByName(DependencyObject element, string focusedElementName) => throw new NotImplementedException();
        }
    }
}
