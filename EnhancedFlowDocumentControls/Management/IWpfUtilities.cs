using System;
using System.Windows;

namespace EnhancedFlowDocumentControls.Management
{
    internal interface IWpfUtilities
    {
        void AddLoadedEventHandler(FrameworkElement customFindToolBar, RoutedEventHandler loadedHandler);

        void AddPreviewKeyDownEnterOrExecuteHandler(Action handler);

        void FocusTextBox(Action<Action> dispatcher);
    }
}
