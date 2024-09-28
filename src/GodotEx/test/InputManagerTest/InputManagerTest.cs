using Godot;

namespace GodotEx.Tests;

public partial class InputManagerTest : Control {
    private InputManager _inputManager;
    private InputManager _shortcutManager;

    public override void _Ready() {
        _shortcutManager = new(GetViewport());
        _inputManager = new(GetViewport());

        _shortcutManager.AddHandler<InputEventKey>("Space",
            e => e.IsKeyPressed(Key.Space),
            e => GD.Print($"{e.Keycode} pressed"));

        var copyHandler = new InputHandler<InputEventKey>("Copy",
            e => e.IsKeyPressed(Key.C, KeyModifierMask.MaskCtrl),
            e => GD.Print($"{e.AsText()} pressed."));
        _shortcutManager.AddHandler(copyHandler);

        _inputManager.AddHandler<InputEventMouseButton>("Left click",
            e => e.IsMousePressed(MouseButton.Left),
            e => GD.Print($"{e.ButtonIndex} clicked"));

        _inputManager.AddHandler<InputEventMouseMotion>("Mouse motion",
            _ => true,
            e => GD.Print(e.Position));
    }

    public override void _Input(InputEvent @event) {
        _inputManager.Handle(@event);
    }

    public override void _ShortcutInput(InputEvent @event) {
        _shortcutManager.Handle(@event);
    }
}
