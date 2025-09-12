using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using EnhancedFlowDocumentControls.Commands;
using EnhancedFlowDocumentControls.Management;

namespace EnhancedFlowDocumentControls.ViewModel
{
    public class FindToolBarViewModel : INotifyPropertyChanged, IFindParameters
    {
        private readonly IFinder _finder;
        private string _findText;
        private bool _isSearchUp;
        private bool _matchWholeWord;
        private bool _matchCase;
        private bool _matchDiacritic;
        private bool _matchKashida;
        private bool _matchAlefHamza;
        private object _originalDataContext;

        public event PropertyChangedEventHandler PropertyChanged;

        public object OriginalDataContext
        {
            get => _originalDataContext;
            set
            {
                if (_originalDataContext == value)
                {
                    return;
                }

                _originalDataContext = value;
                OnPropertyChanged(nameof(OriginalDataContext));
            }
        }

        internal FindToolBarViewModel(IFinder finder, FrameworkElement originalDataContextElement)
        {
            _finder = finder;
            NextCommand = new RelayCommand(FindNext, CanFind);
            PreviousCommand = new RelayCommand(FindPrevious, CanFind);
            SetUpOriginalDataContextIfRequired(originalDataContextElement);
        }

        private void SetUpOriginalDataContextIfRequired(FrameworkElement originalDataContextElement)
        {
            if (originalDataContextElement == null)
            {
                return;
            }

            OriginalDataContext = originalDataContextElement.DataContext;
            originalDataContextElement.DataContextChanged += (s, e) => OriginalDataContext = originalDataContextElement.DataContext;
        }

        public string FindText
        {
            get => _findText;
            set
            {
                if (_findText == value)
                {
                    return;
                }

                _findText = value;
                OnPropertyChanged(nameof(FindText));
                RaiseCommandCanExecuteChanged();
            }
        }

        #region menu items
        public bool MatchWholeWord
        {
            get => _matchWholeWord;
            set => SetProperty(ref _matchWholeWord, value);
        }

        public bool MatchCase
        {
            get => _matchCase;
            set => SetProperty(ref _matchCase, value);
        }

        public bool MatchDiacritic
        {
            get => _matchDiacritic;
            set => SetProperty(ref _matchDiacritic, value);
        }

        public bool MatchKashida
        {
            get => _matchKashida;
            set => SetProperty(ref _matchKashida, value);
        }

        public bool MatchAlefHamza
        {
            get => _matchAlefHamza;
            set => SetProperty(ref _matchAlefHamza, value);
        }
        #endregion

        #region IsSearchUp / IsSearchDown
        public bool IsSearchUp
        {
            get => _isSearchUp;
            private set
            {
                if (_isSearchUp == value)
                {
                    return;
                }

                _isSearchUp = value;
                OnPropertyChanged(nameof(IsSearchUp));
                OnPropertyChanged(nameof(IsSearchDown));
            }
        }

        public bool IsSearchDown => !IsSearchUp;

        #endregion

        #region find commands
        public ICommand NextCommand { get; }

        public ICommand PreviousCommand { get; }

        private void FindNext() => Find(false);

        private void FindPrevious() => Find(true);

        private bool CanFind() => !string.IsNullOrWhiteSpace(FindText);

        private void RaiseCommandCanExecuteChanged()
        {
            (NextCommand as RelayCommand).NotifyCanExecuteChanged();
            (PreviousCommand as RelayCommand).NotifyCanExecuteChanged();
        }
        #endregion

        internal void Find(bool searchUp)
        {
            IsSearchUp = searchUp;
            Find();
        }

        internal void Find() => _finder.Find(this);

        #region INotifyPropertyChanged
        protected virtual void SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        #endregion
    }
}
