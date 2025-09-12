using System.Collections;

namespace UITests.NUnit
{
    internal sealed class FlowDocumentReaderComparisonsTestFixtureSource : IEnumerable
    {
        public IEnumerator GetEnumerator() => TestFixtureSourceHelper.GetEnumerator(DemoWindowTypeNames.FlowDocumentReaders);
    }
}
