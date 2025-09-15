using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using EnhancedFlowDocumentControls.Utils;
using EnhancedFlowDocumentControls.ViewModel;

namespace EnhancedFlowDocumentControls.Management
{
    internal class FindToolBarManager
    {
        private readonly Action<Action> _dispatcher;
        private readonly AlertingFindToolBarHost _alertingFindToolBarHost;
        private readonly IFlowControlReflectorFactory _flowControlReflectorFactory;
        private readonly IDocumentViewHelper _documentViewerHelper;
        private readonly IFindToolBarViewModelFactory _findToolBarViewModelFactory;
        private IFlowControlReflector _flowControlReflector;
        private FrameworkElement _customFindToolBar;
        private Decorator _originalFindToolBarHost;
        private IEnhancedFlowDocumentControl _flowControl;
        private IFindableToolBarViewModel _findToolBarViewModel;

        internal FindToolBarManager(Action<Action> dispatcher = null)
            : this(
                  new AlertingFindToolBarHost(),
                  new FlowControlReflectorFactory(),
                  new DocumentViewHelper(),
                  new FindToolBarViewModelFactory(),
                  dispatcher)
        {
        }

        // ctor for tests
        internal FindToolBarManager(
            AlertingFindToolBarHost alertingFindToolBarHost,
            IFlowControlReflectorFactory flowControlReflectorFactory,
            IDocumentViewHelper documentViewerHelper,
            IFindToolBarViewModelFactory findToolBarViewModelFactory,
            Action<Action> dispatcher = null)
        {
            _dispatcher = dispatcher;
            _alertingFindToolBarHost = alertingFindToolBarHost;
            _flowControlReflectorFactory = flowControlReflectorFactory;
            _documentViewerHelper = documentViewerHelper;
            _findToolBarViewModelFactory = findToolBarViewModelFactory;
            alertingFindToolBarHost.ShowToolBarEvent += AlertingFindToolBarHost_ShowToolBarEvent;
            alertingFindToolBarHost.CloseToolBarEvent += AlertingFindToolBarHost_CloseToolBarEvent;
        }

        private bool IsShowingFindToolbar => _originalFindToolBarHost.Child != null;

        internal void Setup(IEnhancedFlowDocumentControl flowControl, FrameworkElement customFindToolBar)
        {
            _customFindToolBar = customFindToolBar;
            _flowControl = flowControl;
            ReplaceToolBarHostFieldWithAlertingNotInTree();
        }

        private void ReplaceToolBarHostFieldWithAlertingNotInTree()
        {
            _flowControlReflector = _flowControlReflectorFactory.GetReflector(_flowControl);
            _originalFindToolBarHost = _flowControlReflector.GetFindToolBarHost(_flowControl);
            _flowControlReflector.SetFindToolBarHost(_flowControl, _alertingFindToolBarHost);
        }

        private void AlertingFindToolBarHost_CloseToolBarEvent(object sender, EventArgs e)
        {
            _findToolBarViewModel = null;
            _originalFindToolBarHost.Child = null;
            _documentViewerHelper.ToggleFindToolBarHost(_originalFindToolBarHost, false);
        }

        private void AlertingFindToolBarHost_ShowToolBarEvent(object sender, ToolBar findToolBar)
        {
            FrameworkElement originalDataContextElement = _customFindToolBar is IFindToolBarViewModelAware ? null : _flowControl as FrameworkElement;
            _findToolBarViewModel = _findToolBarViewModelFactory.Create(findToolBar, originalDataContextElement);
            AddCustomFindToolBarToHost();
            _documentViewerHelper.ToggleFindToolBarHost(_originalFindToolBarHost, true);
            _customFindToolBar.Loaded += (_, __) => ReadyTextBox();
        }

        private void AddCustomFindToolBarToHost()
        {
            SetCustomFindToolBarViewModel();
            _originalFindToolBarHost.Child = _customFindToolBar;
        }

        private void SetCustomFindToolBarViewModel()
        {
            if (_customFindToolBar is IFindToolBarViewModelAware findToolBarViewModelAware)
            {
                findToolBarViewModelAware.FindToolBarViewModel = _findToolBarViewModel;
            }
            else
            {
                _customFindToolBar.DataContext = _findToolBarViewModel;
            }
        }

        private void ReadyTextBox(bool goToTextBox = true)
        {
            TextBox findTextBox = VisualTreeUtilities.FindByName<TextBox>(_customFindToolBar, "findTextBox");
            if (findTextBox == null)
            {
                return;
            }

            findTextBox.PreviewKeyDown += (sender, e) =>
            {
                if (e == null || (e.Key != Key.Return && e.Key != Key.Execute))
                {
                    return;
                }

                e.Handled = true;
                _findToolBarViewModel.Find();
            };

            if (!goToTextBox)
            {
                return;
            }

            GoToTextBox(findTextBox);
        }

        private void GoToTextBox(TextBox findTextBox)
            => DoDispatch(() =>
            {
                _ = findTextBox.Focus();
                _ = Keyboard.Focus(findTextBox);
            });

        private void DoDispatch(Action action)
        {
            if (_dispatcher != null)
            {
                _dispatcher(action);
            }
            else
            {
                _ = _originalFindToolBarHost.Dispatcher.BeginInvoke(action, DispatcherPriority.Background);
            }
        }

        // the requirement to not call base is due to FlowDocumentPageViewer not immediately exiting when e.Handled is true - it still processes F3 itself
        internal void KeyDown(KeyEventArgs e, Action<KeyEventArgs> baseKeyDown)
        {
            if (ShouldF3Search(e))
            {
                bool searchUp = (e.KeyboardDevice.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;
                _findToolBarViewModel.Find(searchUp);
                e.Handled = true;
            }
            else
            {
                baseKeyDown(e);
            }
        }

        private bool ShouldF3Search(KeyEventArgs e)
            => e.Key == Key.F3 && _flowControlReflector.CanShowFindToolBar(_flowControl) && IsShowingFindToolbar;

        private void KeyDownHandler(KeyEventArgs e) => _documentViewerHelper.KeyDownHelper(e, _originalFindToolBarHost);

        internal static void KeyDownHandler(object sender, KeyEventArgs e)
        {
            FindToolBarManager findToolBarManager = (sender as IEnhancedFlowDocumentControl).FindToolBarManager;
            findToolBarManager.KeyDownHandler(e);
        }

        // unlikely and not documented.
        internal void FindToolBarChanged(FrameworkElement customFindToolBar)
        {
            _customFindToolBar = customFindToolBar;
            if (_findToolBarViewModel == null)
            {
                return;
            }

            string focusedElementName = null;
            if (FocusManager.GetFocusedElement(_originalFindToolBarHost) is FrameworkElement focusedElement)
            {
                focusedElementName = focusedElement.Name;
            }

            AddCustomFindToolBarToHost();

            _customFindToolBar.Loaded += (_, __) =>
            {
                ReadyTextBox(false);
                if (focusedElementName == null)
                {
                    return;
                }

                FrameworkElement newFocusedElement = VisualTreeUtilities.FindByName<FrameworkElement>(_customFindToolBar, focusedElementName);
                if (newFocusedElement == null)
                {
                    return;
                }

                DoDispatch(() => FocusManager.SetFocusedElement(_originalFindToolBarHost, newFocusedElement));
            };
        }
    }
}
