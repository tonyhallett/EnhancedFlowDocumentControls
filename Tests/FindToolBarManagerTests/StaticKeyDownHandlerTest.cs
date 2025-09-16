using System.Windows.Input;
using EnhancedFlowDocumentControls.Management;
using NUnit.Framework;
using Tests.Helpers;
using Ftm = EnhancedFlowDocumentControls.Management.FindToolBarManager;

namespace Tests.FindToolBarManagerTests
{
    internal class StaticKeyDownHandlerTest : TestsBase
    {
        [Test]
        public void Should_Defer_To_DocumentViewHelper()
        {
            FindToolBarManager.Setup(EnhancedFlowDocumentControl, FindToolBarViewModelAware);

            KeyEventArgs keyEventArgs = KeyEventArgsCreator.Create(Key.A);
            Ftm.KeyDownHandler(new EnhancedFlowDocumentControlOfManager(FindToolBarManager), keyEventArgs);

            MockDocumentViewHelper.Verify(documentViewHelper => documentViewHelper.KeyDownHelper(keyEventArgs, OriginalHost));
        }

        private class EnhancedFlowDocumentControlOfManager : IEnhancedFlowDocumentControl
        {
            public EnhancedFlowDocumentControlOfManager(Ftm findToolBarManager)
                => FindToolBarManager = findToolBarManager;

            public Ftm FindToolBarManager { get; }
        }
    }
}
