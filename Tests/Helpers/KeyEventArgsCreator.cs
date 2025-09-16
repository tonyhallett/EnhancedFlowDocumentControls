using System.Windows;
using System.Windows.Input;

namespace Tests.Helpers
{
    internal static class KeyEventArgsCreator
    {
        public static KeyEventArgs Create(Key key, ModifierKeys modifierKeys = ModifierKeys.None, RoutedEvent routedEvent = null)
            => new KeyEventArgs(
                   new TestKeyboardDevice(modifierKeys),
                   new TestPresentationSource(),
                   0,
                   key)
            {
                RoutedEvent = routedEvent ?? Keyboard.KeyDownEvent,
            };
    }
}
