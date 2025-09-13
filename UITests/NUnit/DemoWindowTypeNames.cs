namespace UITests.NUnit
{
    internal static class DemoWindowTypeNames
    {
        public static readonly string EnhancedFlowDocumentReader = nameof(Demo.EnhancedFlowDocumentReaderWindow);
        public static readonly string OriginalDataContext = nameof(Demo.OriginalDataContextWindow);
        public static readonly string InputBindings = nameof(Demo.InputBindingsWindow);
        public static readonly string FindToolbarViewModelAware = nameof(Demo.FindToolbarViewModelAwareWindow);
        public static readonly string FindingTextBoxStackPanelChild = nameof(Demo.FindingTextBoxStackPanelChildWindow);

        public static readonly IEnumerable<string> FlowDocumentReaders =
        [
            EnhancedFlowDocumentReader,
            nameof(Demo.NormalFlowDocumentReaderWindow),
        ];

        public static readonly IEnumerable<string> NonFlowDocumentReaders =
        [
            nameof(Demo.EnhancedFlowDocumentPageViewerWindow),
            nameof(Demo.NormalFlowDocumentPageViewerWindow),

            nameof(Demo.EnhancedFlowDocumentScrollViewerWindow),
            nameof(Demo.NormalFlowDocumentScrollViewerWindow),
        ];
    }
}
