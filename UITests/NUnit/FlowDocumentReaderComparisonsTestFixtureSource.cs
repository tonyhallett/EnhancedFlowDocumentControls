using System.Collections;
using UITests.TestHelpers;

namespace UITests.NUnit
{
    internal sealed class FlowDocumentReaderComparisonsTestFixtureSource : IEnumerable
    {
        public IEnumerator GetEnumerator() => TestFixtureSourceHelper.GetAllCtorPermutations(DemoWindowTypeNames.FlowDocumentReaders);
    }
}
