using System.Collections;

namespace UITests.NUnit
{
    internal sealed class CommonComparisonsTestFixtureSource : IEnumerable
    {
        public IEnumerator GetEnumerator() => TestFixtureSourceHelper.GetEnumerator(
            DemoWindowTypeNames.NonFlowDocumentReaders.Concat(DemoWindowTypeNames.FlowDocumentReaders));
    }
}
