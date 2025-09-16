using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using NUnit.Framework;

namespace Tests.Helpers
{

    public class TestPresentationSource : PresentationSource
    {
        public override Visual RootVisual { get; set; }

        public override bool IsDisposed { get; }

        protected override CompositionTarget GetCompositionTargetCore() => throw new System.NotImplementedException();
    }

}
