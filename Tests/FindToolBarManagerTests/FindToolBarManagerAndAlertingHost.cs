using EnhancedFlowDocumentControls.Management;

namespace Tests.FindToolBarManagerTests
{
    internal sealed class FindToolBarManagerAndAlertingHost
    {
        public FindToolBarManagerAndAlertingHost(FindToolBarManager findToolBarManager, AlertingFindToolBarHost alertingFindToolBarHost)
        {
            FindToolBarManager = findToolBarManager;
            AlertingFindToolBarHost = alertingFindToolBarHost;
        }

        public FindToolBarManager FindToolBarManager { get; }

        public AlertingFindToolBarHost AlertingFindToolBarHost { get; }
    }
}
