using System.Threading;
using System.Windows.Controls;
using EnhancedFlowDocumentControls.Management;
using NUnit.Framework;

namespace Tests
{
    [RequiresThread(ApartmentState.STA)]
    internal sealed class AlertingFindToolBarHostTests
    {
        [Test]
        public void Should_Raise_ShowToolBarEvent_When_Child_Set_To_ToolBar()
        {
            var host = new AlertingFindToolBarHost();

            ToolBar receivedToolBar = null;
            host.ShowToolBarEvent += (_, tb) => receivedToolBar = tb;

            var toolBar = new ToolBar();
            host.Child = toolBar;

            Assert.That(receivedToolBar, Is.SameAs(toolBar));
        }

        [Test]
        public void Should_Raise_CloseToolBarEvent_When_Child_Set_To_Null()
        {
            var host = new AlertingFindToolBarHost();

            bool raised = false;
            host.CloseToolBarEvent += (s, e) => raised = true;

            var toolBar = new ToolBar();
            host.Child = null;

            Assert.That(raised, Is.True);
        }

        [Test]
        public void Should_Return_The_ToolBar_From_Child_Property()
        {
            var host = new AlertingFindToolBarHost();
            var toolBar = new ToolBar();
            host.Child = toolBar;

            Assert.That(host.Child, Is.SameAs(toolBar));
        }
    }
}
