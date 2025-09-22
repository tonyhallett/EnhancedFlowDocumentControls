namespace UITests.NUnit
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    internal sealed class FrameworkVersionsTestAttribute : TestFixtureSourceAttribute
    {
        public FrameworkVersionsTestAttribute()
            : base(typeof(FrameworkVersionsFixtureSource), nameof(FrameworkVersionsFixtureSource.FixtureArgs))
        {
        }
    }
}
