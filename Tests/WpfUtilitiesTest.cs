using System;
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
        private TextBox _findTextBox;

        [SetUp]
        public void SetUp() => SetUp("findTextBox");

        private void SetUp(string findTextBoxName)
        {
            var findToolBar = new FindToolBar();
            _findTextBox = new TextBox() { Name = findTextBoxName };
            _ = findToolBar.Items.Add(_findTextBox);
            _wpfUtilities = new WpfUtilities();
            _wpfUtilities.AddLoadedEventHandler(findToolBar, (s, e) => { });
        }

        [Test]
        public void Should_Throw_If_No_FindTextBox_Found()
        {
            SetUp("mismatchName");
            _ = Assert.Throws<NullReferenceException>(() => _wpfUtilities.AddPreviewKeyDownEnterOrExecuteHandler(null));
        }

        [Test]
        public void Should_Focus_The_FindTextBox_If_It_Exists()
        {
            _wpfUtilities.AddPreviewKeyDownEnterOrExecuteHandler(null);
            bool dispatcherInvoked = false;
            _wpfUtilities.FocusTextBox((action) =>
            {
                dispatcherInvoked = true;
                action();
            });

            Assert.IsTrue(dispatcherInvoked);
            Assert.IsTrue(_findTextBox.IsFocused);
        }

        [Test]
        public void Should_Not_Throw_If_The_FindTextBox_Has_Been_Removed()
        {
            _wpfUtilities.AddPreviewKeyDownEnterOrExecuteHandler(null);
            _wpfUtilities.Clear();
            _wpfUtilities.FocusTextBox((action) => action());
        }

        [TestCase(Key.Enter, true)]
        [TestCase(Key.Execute, true)]
        [TestCase(Key.N, false)]
        public void Should_Callback_Wnen_FindTextBox_PreviewKeyDown_Enter(Key key, bool expectedCalledBack)
        {
            bool calledBack = false;
            _wpfUtilities.AddPreviewKeyDownEnterOrExecuteHandler(() => calledBack = true);
            _findTextBox.RaiseEvent(
                KeyEventArgsCreator.Create(key, ModifierKeys.None, UIElement.PreviewKeyDownEvent));

            Assert.That(calledBack, Is.EqualTo(expectedCalledBack));
        }
    }
}
