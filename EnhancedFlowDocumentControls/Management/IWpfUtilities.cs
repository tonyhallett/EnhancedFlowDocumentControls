using System;
using System.Windows;
using System.Windows.Input;

namespace EnhancedFlowDocumentControls.Management
{
    internal enum F3KeyType
    {
        NotF3,
        F3,
        ShiftF3,
    }

    internal interface IWpfUtilities
    {
        void AddLoadedEventHandler(FrameworkElement customFindToolBar, RoutedEventHandler loadedHandler);

        void AddPreviewKeyDownEnterOrExecuteHandler(Action handler);

        void Clear();

        void FocusTextBox(Action<Action> dispatcher);

        F3KeyType GetF3KeyType(KeyEventArgs e);
    }
}
