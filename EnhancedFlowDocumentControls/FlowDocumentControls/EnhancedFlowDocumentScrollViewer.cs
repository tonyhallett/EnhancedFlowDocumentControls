using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EnhancedFlowDocumentControls.Management;

namespace EnhancedFlowDocumentControls.FlowDocumentControls
{
    public class EnhancedFlowDocumentScrollViewer : FlowDocumentScrollViewer, IEnhancedFlowDocumentControl
    {
        private readonly FindToolBarManager _findToolBarManager = new FindToolBarManager();

        FindToolBarManager IEnhancedFlowDocumentControl.FindToolBarManager => _findToolBarManager;

        public static readonly DependencyProperty FindToolbarContentProperty =
            DependencyProperty.Register(
                nameof(FindToolbarContent),
                typeof(FrameworkElement),
                typeof(EnhancedFlowDocumentScrollViewer),
                new PropertyMetadata(null));

        public FrameworkElement FindToolbarContent
        {
            get => (FrameworkElement)GetValue(FindToolbarContentProperty);
            set => SetValue(FindToolbarContentProperty, value);
        }

        static EnhancedFlowDocumentScrollViewer() => EventManager.RegisterClassHandler(
                typeof(EnhancedFlowDocumentScrollViewer),
                Keyboard.KeyDownEvent,
                new KeyEventHandler(FindToolBarManager.KeyDownHandler),
                true);

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _findToolBarManager.Setup(this, FindToolbarContent);
        }

        protected override void OnKeyDown(KeyEventArgs e) => _findToolBarManager.KeyDown(e, base.OnKeyDown);
    }
}
