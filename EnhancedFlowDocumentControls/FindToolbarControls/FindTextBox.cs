using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EnhancedFlowDocumentControls.FindToolbarControls
{
    public class FindTextBox : Control
    {
        static FindTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FindTextBox), new FrameworkPropertyMetadata(typeof(FindTextBox)));
            SelectionBrushProperty = TextBox.SelectionBrushProperty.AddOwner(typeof(FindTextBox));
            SelectionOpacityProperty = TextBox.SelectionOpacityProperty.AddOwner(typeof(FindTextBox));
        }

        public static readonly DependencyProperty SelectionBrushProperty;

        public Brush SelectionBrush
        {
            get => (Brush)GetValue(SelectionBrushProperty);
            set => SetValue(SelectionBrushProperty, value);
        }

        public static readonly DependencyProperty SelectionOpacityProperty;

        public double SelectionOpacity
        {
            get => (double)GetValue(SelectionOpacityProperty);
            set => SetValue(SelectionOpacityProperty, value);
        }

        public bool ShowTooltip
        {
            get => (bool)GetValue(ShowTooltipProperty);
            set => SetValue(ShowTooltipProperty, value);
        }

        public static readonly DependencyProperty ShowTooltipProperty =
            DependencyProperty.Register(nameof(ShowTooltip), typeof(bool), typeof(FindTextBox), new PropertyMetadata(true));

        public string Tooltip
        {
            get => (string)GetValue(TooltipProperty);
            set => SetValue(TooltipProperty, value);
        }

        public static readonly DependencyProperty TooltipProperty =
            DependencyProperty.Register(nameof(Tooltip), typeof(string), typeof(FindTextBox), new PropertyMetadata("Search for a word or phrase in this document."));

        public string HintText
        {
            get => (string)GetValue(HintTextProperty);
            set => SetValue(HintTextProperty, value);
        }

        public static readonly DependencyProperty HintTextProperty =
            DependencyProperty.Register(nameof(HintText), typeof(string), typeof(FindTextBox), new PropertyMetadata("Search…"));

        public double HintOpacity
        {
            get => (double)GetValue(HintOpacityProperty);
            set => SetValue(HintOpacityProperty, value);
        }

        public static readonly DependencyProperty HintOpacityProperty =
            DependencyProperty.Register(nameof(HintOpacity), typeof(double), typeof(FindTextBox), new PropertyMetadata(0.7));

        public FontStyle HintFontStyle
        {
            get => (FontStyle)GetValue(HintFontStyleProperty);
            set => SetValue(HintFontStyleProperty, value);
        }

        public static readonly DependencyProperty HintFontStyleProperty =
            DependencyProperty.Register(nameof(HintFontStyle), typeof(FontStyle), typeof(FindTextBox), new PropertyMetadata(FontStyles.Italic));

        public double TextBoxWidth
        {
            get => (double)GetValue(TextBoxWidthProperty);
            set => SetValue(TextBoxWidthProperty, value);
        }

        public static readonly DependencyProperty TextBoxWidthProperty =
            DependencyProperty.Register(nameof(TextBoxWidth), typeof(double), typeof(FindTextBox), new PropertyMetadata(183D));
    }
}
