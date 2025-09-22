using System.Windows;

namespace Demo
{
    /// <summary>
    /// Interaction logic for App.xaml.
    /// </summary>
    internal sealed partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e) => InstantiateWindow(e.Args).Show();

        private string GetWindowTypeName(string[] args) => args.Length > 0 ? args[0] : nameof(EnhancedFlowDocumentPageViewerWindow);

        private Window InstantiateWindow(string[] args) => InstantiateWindow(GetWindowTypeName(args));

        private static Window InstantiateWindow(string windowType)
            => typeof(App).Assembly.GetType("Demo." + windowType).GetConstructor(new System.Type[0]).Invoke(null) as Window;
    }
}
