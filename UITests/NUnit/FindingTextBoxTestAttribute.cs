namespace UITests.NUnit
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    internal sealed class FindingTextBoxTestAttribute : TestFixtureSourceAttribute
    {
        public FindingTextBoxTestAttribute()
            : base(typeof(FindingTextBoxTestFixtureSource))
        {
        }
    }
}
