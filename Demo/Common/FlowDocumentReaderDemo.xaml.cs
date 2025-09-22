using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Demo.Common
{
    /// <summary>
    /// Interaction logic for FlowDocumentReaderDemo.xaml.
    /// </summary>
    internal sealed partial class FlowDocumentReaderDemo : UserControl
    {
        public FlowDocumentReaderDemo() => InitializeComponent();

        public FlowDocumentReader FlowDocumentReader
        {
            get => (FlowDocumentReader)GetValue(FlowDocumentReaderProperty);
            set => SetValue(FlowDocumentReaderProperty, value);
        }

        public static readonly DependencyProperty FlowDocumentReaderProperty =
            DependencyProperty.Register(nameof(FlowDocumentReader), typeof(FlowDocumentReader), typeof(FlowDocumentReaderDemo), new PropertyMetadata(null, FlowDocumentReaderChanged));

        private static void FlowDocumentReaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fdr = e.NewValue as FlowDocumentReader;
            _ = fdr.SetBinding(FlowDocumentReader.IsFindEnabledProperty, new Binding
            {
                ElementName = nameof(IsFindEnabledRadioButton),
                Path = new PropertyPath(nameof(RadioButton.IsChecked)),
            });
        }
    }
}
