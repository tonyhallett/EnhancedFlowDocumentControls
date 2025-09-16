using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace EnhancedFlowDocumentControls.KeyCommands
{
    internal sealed class KeyCommand
    {
        private static readonly List<RoutedUICommand> s_keyCommands = new List<RoutedUICommand>
        {
            ApplicationCommands.Find,
            NavigationCommands.NextPage,
            NavigationCommands.PreviousPage,
            NavigationCommands.LastPage,
            NavigationCommands.FirstPage,
        };

        private static bool IsKeyCommand(ICommand command) => s_keyCommands.Contains(command);

        public static KeyCommand TryCreate(KeyBinding kb)
            => !KeyCommand.IsKeyCommand(kb.Command) || !(kb.Gesture is KeyGesture gesture)
                ? null
                : new KeyCommand(kb.Command as RoutedUICommand, gesture, kb.CommandParameter);

        private readonly KeyGesture _gesture;
        private readonly object _commandParameter;
        private readonly RoutedUICommand _command;

        public KeyCommand(RoutedUICommand command, KeyGesture gesture, object commandParameter)
        {
            _command = command;
            _gesture = gesture;
            _commandParameter = commandParameter;
        }

        private bool GestureMatch(KeyEventArgs e) => e.Key == _gesture.Key && Keyboard.Modifiers == _gesture.Modifiers;

        public bool IsMatch(KeyEventArgs e, UIElement target)
        {
            if (!GestureMatch(e))
            {
                return false;
            }

            if (!_command.CanExecute(_commandParameter, target))
            {
                return true;
            }

            _command.Execute(_commandParameter, target);
            return true;
        }
    }
}
