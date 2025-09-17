using System.Threading;
using System.Windows.Controls;
using EnhancedFlowDocumentControls.ViewModel;
using Moq;
using NUnit.Framework;

namespace Tests
{
    [RequiresThread(ApartmentState.STA)]
    internal sealed class FindToolBarFinderTests
    {
        private Mock<IFindToolBarReflector> _mockFindToolBarReflector;
        private FindToolBarFinder _findToolBarFinder;

        private class FindParameters : IFindParameters
        {
            public IFindParameter<string> FindText { get; set; } = new FindParameter<string>(false, string.Empty);

            public IFindParameter<bool> IsSearchUp { get; set; } = new FindParameter<bool>(false, false);

            public IFindParameter<bool> MatchAlefHamza { get; set; } = new FindParameter<bool>(false, false);

            public IFindParameter<bool> MatchCase { get; set; } = new FindParameter<bool>(false, false);

            public IFindParameter<bool> MatchDiacritic { get; set; } = new FindParameter<bool>(false, false);

            public IFindParameter<bool> MatchKashida { get; set; } = new FindParameter<bool>(false, false);

            public IFindParameter<bool> MatchWholeWord { get; set; } = new FindParameter<bool>(false, false);
        }

        private class FindParameter<T> : IFindParameter<T>
        {
            public FindParameter(bool changed, T value)
            {
                Changed = changed;
                Value = value;
            }

            public bool Changed { get; }

            public T Value { get; }

            public bool DidReset { get; private set; }

            public void Reset() => DidReset = true;
        }

        [SetUp]
        public void SetUp()
        {
            var findToolBar = new ToolBar();
            var mockFindToolBarReflectorFactory = new Mock<IFindToolBarReflectorFactory>();
            _mockFindToolBarReflector = new Mock<IFindToolBarReflector>(MockBehavior.Strict);
            _ = _mockFindToolBarReflector.Setup(findToolBarReflector => findToolBarReflector.InvokeFind());
            _ = mockFindToolBarReflectorFactory.Setup(findToolBarReflectorFactory => findToolBarReflectorFactory.CreateReflector(findToolBar))
                .Returns(_mockFindToolBarReflector.Object);
            _findToolBarFinder = new FindToolBarFinder(findToolBar, mockFindToolBarReflectorFactory.Object);
        }

        [Test]
        public void Should_GetFindText_And_Set_Text_If_FindText_Changed()
        {
            var findTextBox = new TextBox();
            _ = _mockFindToolBarReflector.Setup(findToolBarReflector => findToolBarReflector.GetFindTextBox())
                .Returns(findTextBox);

            var findParameters = new FindParameters()
            {
                FindText = new FindParameter<string>(true, "FindTextValue"),
            };
            _findToolBarFinder.Find(findParameters);

            Assert.That(findTextBox.Text, Is.EqualTo("FindTextValue"));
        }

        [Test]
        public void Should_GetFindText_Once()
        {
            var findTextBox = new TextBox();
            _ = _mockFindToolBarReflector.Setup(findToolBarReflector => findToolBarReflector.GetFindTextBox())
                .Returns(findTextBox);

            var findParameters = new FindParameters()
            {
                FindText = new FindParameter<string>(true, "FindTextValue"),
            };
            _findToolBarFinder.Find(findParameters);

            _findToolBarFinder.Find(findParameters);

            _mockFindToolBarReflector.Verify(findToolBarReflector => findToolBarReflector.GetFindTextBox(), Times.Once());
        }

        [Test]
        public void Should_Reset_Changed_FindParameters()
        {
            var findTextBox = new TextBox();
            _ = _mockFindToolBarReflector.Setup(findToolBarReflector => findToolBarReflector.GetFindTextBox())
                .Returns(findTextBox);

            var changedFindParameter = new FindParameter<string>(true, "FindTextValue");
            var unchaangedFindParameter = new FindParameter<bool>(false, true);

            var findParameters = new FindParameters()
            {
                FindText = changedFindParameter,
                IsSearchUp = unchaangedFindParameter,
            };
            _findToolBarFinder.Find(findParameters);

            Assert.That(changedFindParameter.DidReset, Is.True);
            Assert.That(unchaangedFindParameter.DidReset, Is.False);
        }

        [Test]
        public void Should_GetMatchWholeWordMenuItem_And_Set_IsChecked_If_MatchWholeWord_Changed()
        {
            var matchWholeWordMenuItem = new MenuItem();
            _ = _mockFindToolBarReflector.Setup(findToolBarReflector => findToolBarReflector.GetMatchWholeWordMenuItem())
                .Returns(matchWholeWordMenuItem);
            var findParameters = new FindParameters()
            {
                MatchWholeWord = new FindParameter<bool>(true, true),
            };

            _findToolBarFinder.Find(findParameters);

            Assert.That(matchWholeWordMenuItem.IsChecked, Is.True);
        }

        [Test]
        public void Should_GetMatchCaseMenuItem_And_Set_IsChecked_If_MatchCase_Changed()
        {
            var matchCaseMenuItem = new MenuItem();
            _ = _mockFindToolBarReflector.Setup(findToolBarReflector => findToolBarReflector.GetMatchCaseMenuItem())
                .Returns(matchCaseMenuItem);
            var findParameters = new FindParameters()
            {
                MatchCase = new FindParameter<bool>(true, true),
            };
            _findToolBarFinder.Find(findParameters);
            Assert.That(matchCaseMenuItem.IsChecked, Is.True);
        }

        [Test]
        public void Should_GetMatchDiacriticMenuItem_And_Set_IsChecked_If_MatchDiacritic_Changed()
        {
            var matchDiacriticMenuItem = new MenuItem();
            _ = _mockFindToolBarReflector.Setup(findToolBarReflector => findToolBarReflector.GetMatchDiacriticMenuItem())
                .Returns(matchDiacriticMenuItem);
            var findParameters = new FindParameters()
            {
                MatchDiacritic = new FindParameter<bool>(true, true),
            };
            _findToolBarFinder.Find(findParameters);
            Assert.That(matchDiacriticMenuItem.IsChecked, Is.True);
        }

        [Test]
        public void Should_GetMatchKashidaMenuItem_And_Set_IsChecked_If_MatchKashida_Changed()
        {
            var matchKashidaMenuItem = new MenuItem();
            _ = _mockFindToolBarReflector.Setup(findToolBarReflector => findToolBarReflector.GetMatchKashidaMenuItem())
                .Returns(matchKashidaMenuItem);
            var findParameters = new FindParameters()
            {
                MatchKashida = new FindParameter<bool>(true, true),
            };
            _findToolBarFinder.Find(findParameters);
            Assert.That(matchKashidaMenuItem.IsChecked, Is.True);
        }

        [Test]
        public void Should_GetMatchAlefHamzaMenuItem_And_Set_IsChecked_If_MatchAlefHamza_Changed()
        {
            var matchAlefHamzaMenuItem = new MenuItem();
            _ = _mockFindToolBarReflector.Setup(findToolBarReflector => findToolBarReflector.GetMatchAlefHamzaMenuItem())
                .Returns(matchAlefHamzaMenuItem);
            var findParameters = new FindParameters()
            {
                MatchAlefHamza = new FindParameter<bool>(true, true),
            };
            _findToolBarFinder.Find(findParameters);
            Assert.That(matchAlefHamzaMenuItem.IsChecked, Is.True);
        }

        [Test]
        public void Should_Get_MenuItem_Just_The_Once()
        {
            var matchAlefHamzaMenuItem = new MenuItem();
            _ = _mockFindToolBarReflector.Setup(findToolBarReflector => findToolBarReflector.GetMatchAlefHamzaMenuItem())
                .Returns(matchAlefHamzaMenuItem);
            var findParameters = new FindParameters()
            {
                MatchAlefHamza = new FindParameter<bool>(true, true),
            };

            _findToolBarFinder.Find(findParameters);
            _findToolBarFinder.Find(findParameters);

            _mockFindToolBarReflector.Verify(findToolBarReflector => findToolBarReflector.GetMatchAlefHamzaMenuItem(), Times.Once());
        }
    }
}
