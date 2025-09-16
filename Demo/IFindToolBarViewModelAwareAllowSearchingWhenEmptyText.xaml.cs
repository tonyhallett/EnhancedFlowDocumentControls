using System.Windows;
using System.Windows.Controls;
using EnhancedFlowDocumentControls.ViewModel;

namespace Demo
{
    /// <summary>
    /// Interaction logic for IFindToolBarViewModelAwareAllowSearchingWhenEmptyText.xaml.
    /// </summary>
    internal sealed partial class IFindToolBarViewModelAwareAllowSearchingWhenEmptyText : UserControl, IFindToolBarViewModelAware
    {
        public IFindToolBarViewModelAwareAllowSearchingWhenEmptyText() => InitializeComponent();

        public IFindToolBarViewModel FindToolBarViewModel
        {
            get => (IFindToolBarViewModel)GetValue(FindToolBarViewModelProperty);
            set
            {
                value.AllowSearchingWhenEmptyText = true;
                SetValue(FindToolBarViewModelProperty, value);
            }
        }

        public static readonly DependencyProperty FindToolBarViewModelProperty =
            DependencyProperty.Register(nameof(FindToolBarViewModel), typeof(IFindToolBarViewModel), typeof(IFindToolBarViewModelAwareAllowSearchingWhenEmptyText), new PropertyMetadata(null));
    }
}
