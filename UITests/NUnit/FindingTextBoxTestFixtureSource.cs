using System.Collections;

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
