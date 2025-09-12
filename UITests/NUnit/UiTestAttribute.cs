using NUnit.Framework.Internal;

namespace UITests.NUnit
{
    internal sealed class UiTestAttribute : TestAttribute
    {
        public new void ApplyToTest(Test test)
        {
            base.ApplyToTest(test);
            new RequiresThreadAttribute(ApartmentState.STA).ApplyToTest(test);
        }
    }
}
