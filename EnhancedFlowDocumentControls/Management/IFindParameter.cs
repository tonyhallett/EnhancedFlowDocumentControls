namespace EnhancedFlowDocumentControls.Management
{
    internal interface IFindParameter<T>
    {
        bool Changed { get; }

        T Value { get; }

        void Reset();
    }
}
