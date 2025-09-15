using System.Windows;
using System.Windows.Input;

namespace UITests
{
    internal static class KeyEventArgsCreator
    {
        public static KeyEventArgs Create(Key key)
        {
            KeyboardDevice keyboardDevice = InputManager.Current.PrimaryKeyboardDevice;
            PresentationSource presentationSource = PresentationSource.FromVisual(new UIElement());

            return new KeyEventArgs(
                keyboardDevice,
                presentationSource,
                0,         // timestamp
                key);
        }
    }
}
