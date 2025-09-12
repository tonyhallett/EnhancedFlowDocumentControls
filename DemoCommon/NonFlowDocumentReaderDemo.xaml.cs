using System.Windows;
using System.Windows.Controls;

namespace DemoCommon
{
    /// <summary>
    /// Interaction logic for NonFlowDocumentReaderDemo.xaml.
    /// </summary>
    public partial class NonFlowDocumentReaderDemo : UserControl
    {
        public NonFlowDocumentReaderDemo() => InitializeComponent();

        public Control FlowControl
        {
            get => (Control)GetValue(FlowControlProperty);
            set => SetValue(FlowControlProperty, value);
        }

        // Using a DependencyProperty as the backing store for FlowControl.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FlowControlProperty =
            DependencyProperty.Register(nameof(FlowControl), typeof(Control), typeof(NonFlowDocumentReaderDemo), new PropertyMetadata(null));
    }
}
