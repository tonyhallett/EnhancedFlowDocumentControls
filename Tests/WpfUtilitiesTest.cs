using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EnhancedFlowDocumentControls.FindToolBarControls;
using EnhancedFlowDocumentControls.Management;
using NUnit.Framework;
using Tests.Helpers;

namespace Tests
{
    [RequiresThread(System.Threading.ApartmentState.STA)]
    internal class WpfUtilitiesTest
    {
        private WpfUtilities _wpfUtilities;
        private FindToolBar _findToolBar;
        private TextBox _findTextBox;
        private bool _dispatcherInvoked;

        [SetUp]
        public void SetUp() => SetUp(WpfUtilities.FindTextBoxName);

        private void SetUp(string findTextBoxName)
        {
            _findToolBar = new FindToolBar();
            _findTextBox = new TextBox() { Name = findTextBoxName };
            _ = _findToolBar.Items.Add(_findTextBox);
            _wpfUtilities = new WpfUtilities
            {
                Dispatcher = (action) =>
                {
                    _dispatcherInvoked = true;
                    action();
                },
            };
        }

        private void FindFindTextBox()
        {
            _wpfUtilities.FindTextBox(_findToolBar, _wpfUtilities.FocusTextBox);
            _findToolBar.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent, _findToolBar));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Should_Callback_If_FindTextBox_Found(bool finds)
        {
            SetUp(finds ? WpfUtilities.FindTextBoxName : "mismatchName");
            bool calledBack = false;
            _wpfUtilities.FindTextBox(_findToolBar, () => calledBack = true);
            _findToolBar.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent, _findToolBar));

            Assert.That(calledBack, Is.EqualTo(finds));
            Assert.That(_dispatcherInvoked, Is.True);
        }

        [Test]
        public void Should_Focus_The_FindTextBox()
        {
            FindFindTextBox();

            Assert.IsTrue(_dispatcherInvoked);
            Assert.IsTrue(_findTextBox.IsFocused);
        }

        [Test]
        public void Should_Not_Throw_If_The_FindTextBox_Has_Been_Removed() => _wpfUtilities.FocusTextBox();

        [TestCase(Key.Enter, true)]
        [TestCase(Key.Execute, true)]
        [TestCase(Key.N, false)]
        public void Should_Callback_Wnen_FindTextBox_PreviewKeyDown_Enter(Key key, bool expectedCalledBack)
        {
            FindFindTextBox();

            bool calledBack = false;
            _wpfUtilities.AddPreviewKeyDownEnterOrExecuteHandler(() => calledBack = true);
            _findTextBox.RaiseEvent(
                KeyEventArgsCreator.Create(key, ModifierKeys.None, UIElement.PreviewKeyDownEvent));

            Assert.That(calledBack, Is.EqualTo(expectedCalledBack));
        }
    }
}
