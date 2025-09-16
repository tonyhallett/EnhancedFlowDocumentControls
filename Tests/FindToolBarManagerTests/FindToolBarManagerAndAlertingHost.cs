using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EnhancedFlowDocumentControls.Management;
using Moq;
using NUnit.Framework;

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
