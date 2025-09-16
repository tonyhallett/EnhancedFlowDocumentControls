using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace EnhancedFlowDocumentControls.KeyCommands
{
    internal sealed class KeyCommandHandler
    {
        private readonly UIElement _target;
        private readonly List<KeyCommand> _keyCommands;

        public KeyCommandHandler(UIElement target)
        {
            _target = target ?? throw new ArgumentNullException(nameof(target));
            _target.PreviewKeyDown += OnPreviewKeyDown;

            // Build commands from InputBindings of the target (if any)
            _keyCommands = _target.InputBindings
                .OfType<KeyBinding>()
                .Select(KeyCommand.TryCreate)
                .Where(kc => kc != null)
                .ToList();
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            foreach (KeyCommand keyCommand in _keyCommands)
            {
                if (keyCommand.IsMatch(e, _target))
                {
                    e.Handled = true;
                    return;
                }
            }
        }

        public void Detach() => _target.PreviewKeyDown -= OnPreviewKeyDown;
    }
}
