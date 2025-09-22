using NUnit.Framework;

namespace Tests.FindToolBarManagerTests
{
    internal sealed class ToggleFalseTests : TestsBase
    {
        [SetUp]
        public void SetUp()
        {
            SetupAndShowToolBar();
            AlertingFindToolBarHost.Child = null;
        }

        [Test]
        public void Should_Remove_The_Custom_FindToolBar_From_The_Original_Host() => Assert.That(OriginalHost.Child, Is.Null);

        [Test]
        public void Should_ToggleFindToolBarHost_False()
            => MockDocumentViewHelper.Verify(documentViewerHelper => documentViewerHelper.ToggleFindToolBarHost(OriginalHost, false));

        [Test]
        public void Should_Clear_WpfUtilities() => DummyWpfUtilities.Mock.Verify(wpfUtilities => wpfUtilities.Clear());
    }
}
