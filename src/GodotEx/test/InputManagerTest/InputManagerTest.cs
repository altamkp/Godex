using Godot;

namespace Qkabi.GodotEx.Tests;

public partial class InputManagerTest : Control {
    private InputManager _inputManager;
    private InputManager _shortcutManager;

    public override void _Ready() {
        _shortcutManager = new(GetViewport());
        _inputManager = new(GetViewport());

        _shortcutManager.AddHandler<InputEventKey>(
            e => e.IsKeyPressed(Key.Space),
            e => GD.Print($"{e.Keycode} pressed"));

        _inputManager.AddHandler<InputEventMouseButton>(
            e => e.IsMousePressed(MouseButton.Left),
            e => GD.Print($"{e.ButtonIndex} clicked"));

        _inputManager.AddHandler<InputEventMouseMotion>(
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
