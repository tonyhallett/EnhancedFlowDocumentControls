using System.Threading;
using NUnit.Framework;

namespace Tests
{
    internal sealed class UiTestAttribute : RequiresThreadAttribute
    {
        public UiTestAttribute()
            : base(ApartmentState.STA)
        {
        }
    }
}
