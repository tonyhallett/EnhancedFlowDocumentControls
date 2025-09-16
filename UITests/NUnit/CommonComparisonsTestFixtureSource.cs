using System.Collections;
using UITests.TestHelpers;

namespace UITests.NUnit
{
    internal sealed class CommonComparisonsTestFixtureSource : IEnumerable
    {
        public IEnumerator GetEnumerator() => TestFixtureSourceHelper.GetAllCtorPermutations(
            DemoWindowTypeNames.NonFlowDocumentReaders.Concat(DemoWindowTypeNames.FlowDocumentReaders));
    }
}
