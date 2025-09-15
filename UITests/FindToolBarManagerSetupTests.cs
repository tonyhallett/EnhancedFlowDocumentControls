using System.Windows.Controls;
using EnhancedFlowDocumentControls.Management;
using Moq;

namespace UITests
{
    internal class FindToolBarManagerSetupTests
    {
        [Test]
        public void Should_Replace_EnhancedFlowDocumentControl_ToolBarHost_Field_With_Alerting_When_Setup()
        {
            var alertingFindToolBarHost = new AlertingFindToolBarHost();
            IEnhancedFlowDocumentControl enhancedFlowDocumentControl = new Mock<IEnhancedFlowDocumentControl>().Object;
            var mockFlowControlReflectorFactory = new Mock<IFlowControlReflectorFactory>();
            var mockFlowControlReflector = new Mock<IFlowControlReflector>();
            _ = mockFlowControlReflectorFactory
                .Setup(factory => factory.GetReflector(enhancedFlowDocumentControl))
                .Returns(mockFlowControlReflector.Object);

            var findToolbarManager = new FindToolBarManager(
                alertingFindToolBarHost,
                mockFlowControlReflectorFactory.Object,
                null,
                null);

            var customFindToolBar = new Button();
            findToolbarManager.Setup(enhancedFlowDocumentControl, customFindToolBar);

            mockFlowControlReflector.Verify(flowControlReflector => flowControlReflector.SetFindToolBarHost(enhancedFlowDocumentControl, alertingFindToolBarHost));
        }
    }
}
