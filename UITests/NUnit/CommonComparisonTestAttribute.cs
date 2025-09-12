namespace UITests.NUnit
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    internal sealed class CommonComparisonTestAttribute : TestFixtureSourceAttribute
    {
        public CommonComparisonTestAttribute()
            : base(typeof(CommonComparisonsTestFixtureSource))
        {
        }
    }
}
