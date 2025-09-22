using System;
using System.Threading;
using EnhancedFlowDocumentControls.ViewModel;
using NUnit.Framework;

namespace Tests
{
    internal sealed class FindToolBarViewModelTests
    {
        private FindToolBarViewModel _findToolBarViewModel;

        private class DummyFinder : IFinder
        {
            public IFindParameters FindParameters { get; private set; }

            public void Find(IFindParameters findParameters) => FindParameters = findParameters;
        }

        private DummyFinder _dummyFinder;

        [SetUp]
        public void Setup()
        {
            _dummyFinder = new DummyFinder();
            _findToolBarViewModel = new FindToolBarViewModel(_dummyFinder, null);
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
        public void Should_Be_Search_Down_Initially() => AssertFindToolBarViewModelIsSearchUp(false);

        [Test]
        public void Should_Find_Search_Up_From_Button()
        {
            _findToolBarViewModel.AllowSearchingWhenEmptyText = true;
            _findToolBarViewModel.PreviousCommand.Execute(null);

            AssertFindToolBarViewModelIsSearchUp(true);
            Assert.That(_dummyFinder.FindParameters.IsSearchUp.Value, Is.True);
            Assert.That(_dummyFinder.FindParameters.IsSearchUp.Changed, Is.True);
        }

        [Test]
        public void Should_Find_Search_Down_From_Button()
        {
            _findToolBarViewModel.FindText = "abc";
            _findToolBarViewModel.NextCommand.Execute(null);

            AssertFindToolBarViewModelIsSearchUp(false);
            Assert.That(_dummyFinder.FindParameters.IsSearchUp.Value, Is.False);
            Assert.That(_dummyFinder.FindParameters.IsSearchUp.Changed, Is.False);
        }

        [Test]
        public void Should_Find_With_SearchUp_True()
        {
            _findToolBarViewModel.Find(true);

            AssertFindToolBarViewModelIsSearchUp(true);
            Assert.That(_dummyFinder.FindParameters.IsSearchUp.Value, Is.True);
            Assert.That(_dummyFinder.FindParameters.IsSearchUp.Changed, Is.True);
        }

        [Test]
        public void Should_Find_With_SearchUp_False()
        {
            _findToolBarViewModel.Find(false);

            AssertFindToolBarViewModelIsSearchUp(false);
            Assert.That(_dummyFinder.FindParameters.IsSearchUp.Value, Is.False);
            Assert.That(_dummyFinder.FindParameters.IsSearchUp.Changed, Is.False);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Should_Search_With_Current_Search_Direction_When_Find(bool currentIsSearchUp)
        {
            _findToolBarViewModel.Find(currentIsSearchUp);
            _dummyFinder.FindParameters.IsSearchUp.Reset();

            _findToolBarViewModel.Find();

            AssertFindToolBarViewModelIsSearchUp(currentIsSearchUp);
            Assert.That(_dummyFinder.FindParameters.IsSearchUp.Value, Is.EqualTo(currentIsSearchUp));
            Assert.That(_dummyFinder.FindParameters.IsSearchUp.Changed, Is.False);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Should_Have_Expected_FindText_FindParameter(bool setFindText)
        {
            if (setFindText)
            {
                _findToolBarViewModel.FindText = "abc";
            }

            _findToolBarViewModel.Find();
            Assert.That(_dummyFinder.FindParameters.FindText.Value, Is.EqualTo(_findToolBarViewModel.FindText));
            Assert.That(_dummyFinder.FindParameters.FindText.Changed, Is.EqualTo(setFindText));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Should_Have_Expected_MatchAlefHamza_FindParameter(bool selectMatchAlefHamza)
            => MenuItemTest(selectMatchAlefHamza, () => _dummyFinder.FindParameters.MatchAlefHamza, () => _findToolBarViewModel.MatchAlefHamza = true);

        [TestCase(true)]
        [TestCase(false)]
        public void Should_Have_Expected_MatchCase_FindParameter(bool selectMatchCase)
            => MenuItemTest(selectMatchCase, () => _dummyFinder.FindParameters.MatchCase, () => _findToolBarViewModel.MatchCase = true);

        [TestCase(true)]
        [TestCase(false)]
        public void Should_Have_Expected_MatchDiacritic_FindParameter(bool selectMatchDiacritic)
            => MenuItemTest(selectMatchDiacritic, () => _dummyFinder.FindParameters.MatchDiacritic, () => _findToolBarViewModel.MatchDiacritic = true);

        [TestCase(true)]
        [TestCase(false)]
        public void Should_Have_Expected_MatchKashida_FindParameter(bool selectMatchKashida)
            => MenuItemTest(selectMatchKashida, () => _dummyFinder.FindParameters.MatchKashida, () => _findToolBarViewModel.MatchKashida = true);

        [TestCase(true)]
        [TestCase(false)]
        public void Should_Have_Expected_MatchWholeWord_FindParameter(bool selectMatchWholeWord)
            => MenuItemTest(selectMatchWholeWord, () => _dummyFinder.FindParameters.MatchWholeWord, () => _findToolBarViewModel.MatchWholeWord = true);

        private void MenuItemTest(bool selectMenuItem, Func<IFindParameter<bool>> getFindParameter, Action setFindToolBarMenuItem)
        {
            if (selectMenuItem)
            {
                setFindToolBarMenuItem();
            }

            _findToolBarViewModel.Find();

            IFindParameter<bool> findParameter = getFindParameter();
            Assert.That(findParameter.Value, Is.EqualTo(selectMenuItem));
            Assert.That(findParameter.Changed, Is.EqualTo(selectMenuItem));
        }

        [Test]
        public void Should_Not_Have_Find_Parameter_Changed_If_Original()
        {
            _findToolBarViewModel.FindText = "abc";
            _findToolBarViewModel.Find();

            _dummyFinder.FindParameters.FindText.Reset();

            _findToolBarViewModel.FindText = "ab";
            _findToolBarViewModel.FindText = "abc";

            _findToolBarViewModel.Find();
            Assert.That(_dummyFinder.FindParameters.FindText.Changed, Is.False);
        }

        [Test]
        public void Should_Not_Have_OriginalDataContext_When_OriginalDataContextElement_Is_Null()
            => Assert.That(_findToolBarViewModel.OriginalDataContext, Is.Null);

        [Test]
        [RequiresThread(ApartmentState.STA)]
        public void Should_Have_OriginalDataContext_When_Required_From_OriginalDataContextElement()
        {
            var originalDataContextElement = new System.Windows.Controls.Button
            {
                DataContext = "InitialDataContext",
            };
            var findToolBarViewModel = new FindToolBarViewModel(null, originalDataContextElement);
            Assert.That(findToolBarViewModel.OriginalDataContext, Is.EqualTo("InitialDataContext"));
        }

        [Test]
        [RequiresThread(ApartmentState.STA)]
        public void Should_Notify_When_OriginalDataContext_Changes()
        {
            var originalDataContextElement = new System.Windows.Controls.Button
            {
                DataContext = "InitialDataContext",
            };
            var findToolBarViewModel = new FindToolBarViewModel(null, originalDataContextElement);

            object newOriginalDataContext = null;
            findToolBarViewModel.PropertyChanged += (s, args) =>
            {
                if (args.PropertyName != nameof(findToolBarViewModel.OriginalDataContext))
                {
                    return;
                }

                newOriginalDataContext = findToolBarViewModel.OriginalDataContext;
            };
            originalDataContextElement.DataContext = "NewDataContext";
            Assert.That(newOriginalDataContext, Is.EqualTo("NewDataContext"));
        }

        private void AssertFindToolBarViewModelIsSearchUp(bool expectedIsSearchUp)
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
                findToolBarViewModel.NextCommand.CanExecuteChanged += (_, __) => _nextCanExecuteChanged = true;
                findToolBarViewModel.PreviousCommand.CanExecuteChanged += (_, __) => _previousCanExecuteChanged = true;
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
