using FlaUI.Core;
using UIAutomationHelpers;
using UITests.TestHelpers;

namespace UITests.Tests
{
    internal abstract class OpenedFindToolbarTestsBase(string windowTypeName, FrameworkVersion frameworkVersion)
        : FindToolBarTestsBase(windowTypeName, frameworkVersion)
    {
        protected override Application StartApplication()
        {
            Application application = base.StartApplication();

            CommonHelpers.ShowFindToolbarFirstTime(Window);

            AssertShowsFindToolbar();

            return application;
        }
    }
}
