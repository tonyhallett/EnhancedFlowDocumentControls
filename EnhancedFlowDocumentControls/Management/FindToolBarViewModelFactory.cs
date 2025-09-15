using System.Windows;
using System.Windows.Controls;
using EnhancedFlowDocumentControls.ViewModel;

namespace EnhancedFlowDocumentControls.Management
{
    internal class FindToolBarViewModelFactory : IFindToolBarViewModelFactory
    {
        public IFindableToolBarViewModel Create(ToolBar findToolBar, FrameworkElement originalDataContextElement)
            => new FindToolBarViewModel(new FindToolBarWrapper(findToolBar), originalDataContextElement);
    }
}
