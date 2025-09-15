using System.Windows;
using System.Windows.Controls;
using EnhancedFlowDocumentControls.ViewModel;

namespace EnhancedFlowDocumentControls.FindToolbarControls
{
    public class FindToolBar : ToolBar, IFindToolBarViewModelAware
    {
        static FindToolBar() => DefaultStyleKeyProperty.OverrideMetadata(
                typeof(FindToolBar),
                new FrameworkPropertyMetadata(typeof(FindToolBar)));

        public IFindToolBarViewModel FindToolBarViewModel
        {
            get => (FindToolBarViewModel)GetValue(FindToolBarViewModelProperty);
            set => SetValue(FindToolBarViewModelProperty, value);
        }

        public static readonly DependencyProperty FindToolBarViewModelProperty =
            DependencyProperty.Register(nameof(FindToolBarViewModel), typeof(FindToolBarViewModel), typeof(FindToolBar), new PropertyMetadata(null));
    }
}
