namespace UITests.NUnit
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    internal sealed class FlowDocumentReaderComparisonTestAttribute : TestFixtureSourceAttribute
    {
        public FlowDocumentReaderComparisonTestAttribute()
            : base(typeof(FlowDocumentReaderComparisonsTestFixtureSource))
        {
        }
    }
}
