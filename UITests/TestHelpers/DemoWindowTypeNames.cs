namespace UITests.TestHelpers
{
    internal static class DemoWindowTypeNames
    {
        public const string EnhancedFlowDocumentReader = nameof(Demo.EnhancedFlowDocumentReaderWindow);
        public const string OriginalDataContext = nameof(Demo.OriginalDataContextWindow);
        public const string FindToolbarViewModelAwareDataContext = nameof(Demo.FindToolbarViewModelAwarePreserveDataContextWindow);
        public const string FindToolBarViewModelAwareAllowSearchingWhenEmptyText = nameof(Demo.IFindToolBarViewModelAwareAllowSearchingWhenEmptyTextWindow);
        public const string FindingTextBoxStackPanelChild = nameof(Demo.FindingTextBoxStackPanelChildWindow);
        public const string FindToolBarChanged = nameof(Demo.FindToolBarChangedWindow);

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
