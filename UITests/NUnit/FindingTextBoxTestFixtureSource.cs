using System.Collections;
using UITests.TestHelpers;

namespace UITests.NUnit
{
    internal sealed class FindingTextBoxTestFixtureSource : IEnumerable
    {
        private readonly List<string> _windowNames = [
            ..DemoWindowTypeNames.NonFlowDocumentReaders,
            ..DemoWindowTypeNames.FlowDocumentReaders,
            DemoWindowTypeNames.FindingTextBoxStackPanelChild,
        ];

        public IEnumerator GetEnumerator() => TestFixtureSourceHelper.GetAllCtorPermutations(
            _windowNames);
    }
}
