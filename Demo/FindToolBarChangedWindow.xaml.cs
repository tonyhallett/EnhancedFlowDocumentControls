using System;
using System.Windows;
using System.Windows.Threading;

namespace Demo
{
    /// <summary>
    /// Interaction logic for FindToolBarChangedWindow.xaml.
    /// </summary>
    internal sealed partial class FindToolBarChangedWindow : Window
    {
        public FindToolBarChangedWindow() => InitializeComponent();

        public void ChangeDelayedClick(object sender, RoutedEventArgs e)
        {
            var t = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1),
            };
            t.Tick += (s, args) =>
            {
                t.Stop();
                DoChangeFindToolBar();
            };
            t.Start();
        }

        public void ChangeClick(object sender, RoutedEventArgs e) => DoChangeFindToolBar();

        private void DoChangeFindToolBar()
        {
            var newFindToolBar = Resources["NewFindToolbar"] as FrameworkElement;
            EnhancedFlowDocumentReader.FindToolBar = newFindToolBar;
        }
    }
}
