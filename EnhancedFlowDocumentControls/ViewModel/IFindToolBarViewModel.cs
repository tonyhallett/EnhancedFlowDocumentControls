using System.Windows.Input;

namespace EnhancedFlowDocumentControls.ViewModel
{
    public interface IFindToolBarViewModel
    {
        bool AllowSearchingWhenEmptyText { get; set; }

        string FindText { get; set; }

        bool IsSearchDown { get; }

        bool IsSearchUp { get; }

        bool MatchAlefHamza { get; set; }

        bool MatchCase { get; set; }

        bool MatchDiacritic { get; set; }

        bool MatchKashida { get; set; }

        bool MatchWholeWord { get; set; }

        ICommand NextCommand { get; }

        ICommand PreviousCommand { get; }

        object OriginalDataContext { get; }
    }
}
