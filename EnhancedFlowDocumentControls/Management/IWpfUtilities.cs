using System;
using System.Windows;
using System.Windows.Input;

namespace EnhancedFlowDocumentControls.Management
{
    internal interface IWpfUtilities
    {
        void AddLoadedEventHandler(FrameworkElement customFindToolBar, RoutedEventHandler loadedHandler);

        void AddPreviewKeyDownEnterOrExecuteHandler(Action handler);

        void Clear();

        void FocusTextBox(Action<Action> dispatcher);
    }
}
