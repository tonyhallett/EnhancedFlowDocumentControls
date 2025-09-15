using EnhancedFlowDocumentControls.Management;
using EnhancedFlowDocumentControls.ViewModel;
using Moq;

namespace UITests
{
    internal sealed class FindToolBarViewModelTests
    {
        private Mock<IFinder> _mockFinder = null!;
        private FindToolBarViewModel _findToolBarViewModel = null!;

        [SetUp]
        public void Setup()
        {
            _mockFinder = new Mock<IFinder>();
            _findToolBarViewModel = new FindToolBarViewModel(_mockFinder.Object, null);
        }

        [Test]
        public void Should_Not_Be_Able_To_Find_With_Buttons_If_Empty_Text()
            => Assert.That(new CanExecuteHelper(_findToolBarViewModel).ButtonsCanExecute(), Is.False);

        [Test]
        public void Should_Be_Able_To_Find_With_Buttons_If_Empty_Text_And_Allow_It()
        {
            var canExecuteHelper = new CanExecuteHelper(_findToolBarViewModel);
            _findToolBarViewModel.AllowSearchingWhenEmptyText = true;

            Assert.That(canExecuteHelper.ButtonsCanExecute(), Is.True);
        }

        [Test]
        public void Should_Disable_Buttons_When_Text_Emptied()
        {
            _findToolBarViewModel.FindText = "abc";

            var canExecuteHelper = new CanExecuteHelper(_findToolBarViewModel);
            _findToolBarViewModel.FindText = string.Empty;
            Assert.Multiple(() =>
            {
                Assert.That(canExecuteHelper.ButtonsCanExecute(), Is.False);
                Assert.That(canExecuteHelper.BothCanExecuteChanged, Is.True);
            });
        }

        [Test]
        public void Should_Be_Search_Down_Initially() => AssertIsSearchUp(false);

        [Test]
        public void Should_Find_Search_Up_From_Button()
        {
            _findToolBarViewModel.FindText = "abc";
            _findToolBarViewModel.PreviousCommand.Execute(null);
            AssertIsSearchUp(true);
            _mockFinder!.Verify(finder => finder.Find(_findToolBarViewModel));
        }

        [Test]
        public void Should_Find_Search_Down_From_Button()
        {
            _findToolBarViewModel.FindText = "abc";
            _findToolBarViewModel.NextCommand.Execute(null);
            AssertIsSearchUp(false);
            _mockFinder!.Verify(finder => finder.Find(_findToolBarViewModel));
        }

        [Test]
        public void Should_Find_With_SearchUp_True()
        {
            _findToolBarViewModel.Find(true);
            AssertIsSearchUp(true);
            _mockFinder!.Verify(finder => finder.Find(_findToolBarViewModel));
        }

        [Test]
        public void Should_Find_With_SearchUp_False()
        {
            _findToolBarViewModel.Find(false);
            AssertIsSearchUp(false);
            _mockFinder!.Verify(finder => finder.Find(_findToolBarViewModel));
        }

        [TestCase(true)]
        public void Should_Search_With_Current_Search_Direction_When_Find(bool currentIsSearchUp)
        {
            _findToolBarViewModel.Find(currentIsSearchUp);
            _findToolBarViewModel.Find();
            AssertIsSearchUp(currentIsSearchUp);
            _mockFinder!.Verify(finder => finder.Find(_findToolBarViewModel), Times.Exactly(2));
        }

        private void AssertIsSearchUp(bool expectedIsSearchUp)
            => Assert.Multiple(() =>
            {
                Assert.That(_findToolBarViewModel.IsSearchUp, Is.EqualTo(expectedIsSearchUp));
                Assert.That(_findToolBarViewModel.IsSearchDown, Is.EqualTo(!expectedIsSearchUp));
            });

        private sealed class CanExecuteHelper
        {
            private readonly FindToolBarViewModel _findToolBarViewModel;
            private bool _nextCanExecuteChanged;
            private bool _previousCanExecuteChanged;

            public bool BothCanExecuteChanged => _nextCanExecuteChanged && _previousCanExecuteChanged;

            public CanExecuteHelper(FindToolBarViewModel findToolBarViewModel)
            {
                _findToolBarViewModel = findToolBarViewModel;
                findToolBarViewModel.NextCommand.CanExecuteChanged += (_, _) => _nextCanExecuteChanged = true;
                findToolBarViewModel.PreviousCommand.CanExecuteChanged += (_, _) => _previousCanExecuteChanged = true;
            }

            public bool ButtonsCanExecute()
            {
                bool nextCanExecute = _findToolBarViewModel.NextCommand.CanExecute(null);
                bool previousCanExecute = _findToolBarViewModel.PreviousCommand.CanExecute(null);
                return nextCanExecute != previousCanExecute
                    ? throw new InvalidOperationException("Next and Previous command CanExecute should be the same")
                    : nextCanExecute;
            }
        }
    }
}
