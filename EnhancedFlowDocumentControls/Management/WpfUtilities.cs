using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using EnhancedFlowDocumentControls.Utils;

namespace EnhancedFlowDocumentControls.Management
{
    internal sealed class WpfUtilities : IWpfUtilities
    {
        private FrameworkElement _customFindToolBar;
        private RoutedEventHandler _loadedHandler;
        private TextBox _findTextBox;
        private KeyEventHandler _previewKeyDownHandler;

        public void AddPreviewKeyDownEnterOrExecuteHandler(Action handler)
        {
            // todo - use a timer to find the textbox if not found, change Clear method
            _findTextBox = VisualTreeUtilities.FindByName<TextBox>(_customFindToolBar, "findTextBox");
            _previewKeyDownHandler = (_, e) =>
            {
                if (e == null || (e.Key != Key.Return && e.Key != Key.Execute))
                {
                    return;
                }

                e.Handled = true;
                handler();
            };

            if (_findTextBox == null)
            {
                return;
            }

            _findTextBox.PreviewKeyDown += _previewKeyDownHandler;
        }

        public void FocusTextBox(Action<Action> dispatcher)
            => DoDispatch(
                () =>
                {
                    // due to fast toggling of the find toolbar, the textbox may not be there
                    if (_findTextBox == null)
                    {
                        return;
                    }

                    _ = _findTextBox.Focus();
                    _ = Keyboard.Focus(_findTextBox);
                },
                dispatcher);

        private void DoDispatch(Action action, Action<Action> dispatcher)
        {
            if (dispatcher != null)
            {
                dispatcher(action);
            }
            else
            {
                _ = _customFindToolBar.Dispatcher.BeginInvoke(action, DispatcherPriority.Background);
            }
        }

        public void AddLoadedEventHandler(FrameworkElement customFindToolBar, RoutedEventHandler loadedHandler)
        {
            _customFindToolBar = customFindToolBar;
            _loadedHandler = loadedHandler;
            customFindToolBar.Loaded += loadedHandler;
        }

        public void Clear()
        {
            // todo remove once have a timer for finding the find textbox
            if (_findTextBox != null)
            {
                _findTextBox.PreviewKeyDown -= _previewKeyDownHandler;
                _previewKeyDownHandler = null;
                _findTextBox = null;
            }

            _customFindToolBar.Loaded -= _loadedHandler;
            _loadedHandler = null;
            _customFindToolBar = null;
        }
    }
}
