using System.Windows;
using System.Windows.Media;

namespace Tests.Helpers
{
    public class TestPresentationSource : PresentationSource
    {
        public override Visual RootVisual { get; set; }

        public override bool IsDisposed { get; }

        protected override CompositionTarget GetCompositionTargetCore() => throw new System.NotImplementedException();
    }
}
