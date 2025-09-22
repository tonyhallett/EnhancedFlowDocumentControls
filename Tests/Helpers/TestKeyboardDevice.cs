using System.Collections.Generic;
using System.Windows.Input;

namespace Tests.Helpers
{
    internal class TestKeyboardDevice : KeyboardDevice
    {
        private readonly ModifierKeys _modifierKeys;

        private static readonly Dictionary<ModifierKeys, (Key Left, Key Right)> s_modifierKeyMap =
            new Dictionary<ModifierKeys, (Key Left, Key Right)>
            {
                    { ModifierKeys.Alt,     (Key.LeftAlt, Key.RightAlt) },
                    { ModifierKeys.Control, (Key.LeftCtrl, Key.RightCtrl) },
                    { ModifierKeys.Shift,   (Key.LeftShift, Key.RightShift) },
            };

        public TestKeyboardDevice(ModifierKeys modifierKeys = ModifierKeys.None)
            : base(InputManager.Current) => _modifierKeys = modifierKeys;

        protected override KeyStates GetKeyStatesFromSystem(Key key)
        {
            foreach (KeyValuePair<ModifierKeys, (Key Left, Key Right)> kvp in s_modifierKeyMap)
            {
                if ((_modifierKeys & kvp.Key) != 0 &&
                    (key == kvp.Value.Left || key == kvp.Value.Right))
                {
                    return KeyStates.Down;
                }
            }

            return KeyStates.None;
        }
    }
}
