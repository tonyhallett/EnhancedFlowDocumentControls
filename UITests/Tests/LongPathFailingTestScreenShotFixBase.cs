using System.IO;
using FlaUI.TestUtilities;

namespace UITests.Tests
{
    /*
        FlaUITestBase.UITestBaseTearDown => TakeScreenShot => TestContext.AddTestAttachment
        if there is an exception CloseApplication in teardown is not run.
        https://github.com/FlaUI/FlaUI/issues/706
        https://github.com/nunit/nunit/issues/4353
    */
    internal abstract class LongPathFailingTestScreenShotFixBase : FlaUITestBase
    {
        private bool _takeScreenshots = true;

        protected override bool TakeScreenshots => _takeScreenshots;

        protected void SetTakeScreenshots(bool takeScreenshots) => _takeScreenshots = takeScreenshots;

        public override void UITestBaseTearDown()
        {
            try
            {
                base.UITestBaseTearDown();
            }
            catch (FileNotFoundException fileNotFoundException) when (IsAddTestAttachmentException(fileNotFoundException))
            {
                SafelyCloseApplication();
            }
        }

        private void SafelyCloseApplication()
        {
            // Safely close the application
            _takeScreenshots = false;
            base.UITestBaseTearDown();
        }

        private bool IsAddTestAttachmentException(FileNotFoundException fileNotFoundException)
        {
            System.Reflection.MethodBase? targetSite = fileNotFoundException.TargetSite;
            return targetSite?.Name == nameof(TestContext.AddTestAttachment) && targetSite.DeclaringType == typeof(TestContext);
        }
    }
}
