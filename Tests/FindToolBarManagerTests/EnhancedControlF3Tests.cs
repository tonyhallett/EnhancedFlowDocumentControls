using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EnhancedFlowDocumentControls.Management;
using Moq;
using NUnit.Framework;
using Tests.Helpers;

namespace Tests.FindToolBarManagerTests
{
    internal class EnhancedControlF3Tests : TestsBase
    {
        [SetUp]
        public void SetUp() => MockFindToolBarViewModelFactory.Setup(
                findToolBarViewModelFactory => findToolBarViewModelFactory.Create(
                    It.IsAny<ToolBar>(),
                    It.IsAny<FrameworkElement>())).Returns(FindableToolBarViewModel);

        [Test]
        public void Should_Not_Find_When_Not_Showing_And_F3()
        {
            FindToolBarManager.Setup(EnhancedFlowDocumentControl, FindToolBarViewModelAware);
            DoesNotFindTest(Key.F3);
        }

        [Test]
        public void Should_Not_Find_When_Showing_But_Not_F3()
        {
            SetupAndShowToolBar();
            DoesNotFindTest(Key.N);
        }

        private void DoesNotFindTest(Key key)
        {
            KeyEventArgs keyEventArgs = KeyEventArgsCreator.Create(key);
            KeyEventArgs baseKeyEventArgs = null;
            void BaseKeyDown(KeyEventArgs e) => baseKeyEventArgs = e;
            FindToolBarManager.KeyDown(keyEventArgs, BaseKeyDown);

            Assert.That(baseKeyEventArgs, Is.SameAs(keyEventArgs));
            Assert.That(keyEventArgs.Handled, Is.False);
            MockFindableToolBarViewModel.Verify(findableToolBarViewModel => findableToolBarViewModel.Find(It.IsAny<bool>()), Times.Never);
        }

        [Test]
        public void Should_Search_Down_When_Showing_And_F3() => DoesFindTest(false);

        [Test]
        public void Should_Search_Up_When_Showing_And_ShiftF3() => DoesFindTest(true);

        private void DoesFindTest(bool isShift)
        {
            SetupAndShowToolBar();

            KeyEventArgs keyEventArgs = KeyEventArgsCreator.Create(Key.F3, isShift ? ModifierKeys.Shift : ModifierKeys.None);
            FindToolBarManager.KeyDown(keyEventArgs, _ => Assert.Fail());

            Assert.That(keyEventArgs.Handled, Is.True);
            MockFindableToolBarViewModel.Verify(findableToolBarViewModel => findableToolBarViewModel.Find(isShift));
        }
    }
}
