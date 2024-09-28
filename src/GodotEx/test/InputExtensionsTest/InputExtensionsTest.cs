using Godot;

namespace GodotEx.Tests;

public partial class InputExtensionsTest : Node {
    public override void _ShortcutInput(InputEvent @event) {
        if (@event.IsKeyPressed(Key.Space, KeyModifierMask.MaskCtrl)) {
            GD.Print($"{Key.Space} pressed.");
        }
    }
}
