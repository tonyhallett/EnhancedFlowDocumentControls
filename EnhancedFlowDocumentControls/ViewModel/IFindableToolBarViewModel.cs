namespace EnhancedFlowDocumentControls.ViewModel
{
    internal interface IFindableToolBarViewModel : IFindToolBarViewModel
    {
        void Find();

        void Find(bool searchUp);
    }
}
