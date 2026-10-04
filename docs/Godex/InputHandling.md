# Input Handling

## Problems

Handling input in Godot requires large amount of code that checks for certain conditions. Let's say you have a UI node that handle shortcuts and shows a tooltip that follows the mouse's position:

```csharp
public partial class Display : Control {
    private Control _tooltip;
    private LineEdit _lineEdit;

    public override void _ShortcutInput(InputEvent @event) {
        if (@event is InputEventMouseMotion eventMotion) {
            _tooltip.GlobalPosition = eventMotion.GlobalPosition;
        }
    }

    public override void _UnhandledInput(InputEvent @event) {
        if (@event is InputEventKey eventKey) {
            if (!eventKey.Pressed) {
                return;
            }

            if (eventKey.Keycode == Key.Escape) {
                GetTree().Quit();
            }
            if (eventKey.Keycode == Key.Tab) {
                _lineEdit.GrabFocus();
            }
            if (eventKey.Keycode == Key.C && eventKey.GetModifiersMask() == KeyModifierMask.MaskCtrl) {
                DisplayServer.ClipboardSet(_lineEdit.Text);
            }
            if (eventKey.Keycode == Key.V && eventKey.GetModifiersMask() == KeyModifierMask.MaskCtrl) {
                _lineEdit.Text = DisplayServer.ClipboardGet();
            }
        }
    }
}
```

## Solution

With `InputManager` and the use of some input related [extension](BasicExtensions.md) methods, the clarity of the relationships between input events and their corresponding callbacks are by far more explicit:

```csharp
public partial class Display : Control {
    private Control _tooltip;
    private LineEdit _lineEdit;

    private InputManager _shortcutManager;
    private InputManager _inputManager;

    public override void _Ready() {
        _shortcutManager = new InputManager(GetViewport());
        _inputManager = new InputManager(GetViewport());

        _shortcutManager.AddMouseMotionHandler(
            _ => true,
            m => {
                if (GetRect().HasPoint(m.Position)) {
                    _tooltip.Visible = true;
                    _tooltip.GlobalPosition = m.GlobalPosition;
                } else {
                    _tooltip.Visible = false;
                }
            });

        _inputManager.AddHandler<InputEventKey>(
            k => k.IsKeyPressed(Key.Escape),
            _ => GetTree().Quit());

        _inputManager.AddHandler<InputEventKey>(
            k => k.IsKeyPressed(Key.Tab),
            _ => _lineEdit.GrabFocus());

        _inputManager.AddHandler<InputEventKey>(
            k => k.IsKeyPressed(Key.C, KeyModifierMask.MaskCtrl),
            _ => DisplayServer.ClipboardSet(_lineEdit.Text));

        _inputManager.AddHandler<InputEventKey>(
            k => k.IsKeyPressed(Key.V, KeyModifierMask.MaskCtrl),
            _ => _lineEdit.Text = DisplayServer.ClipboardGet());
    }

    public override void _ShortcutInput(InputEvent @event) => _shortcutManager.Handle(@event);
    public override void _UnhandledInput(InputEvent @event) => _inputManager.Handle(@event);
}
```

Handlers are stored in the order they are added. `Handle(InputEvent)` walks them in that order and stops at the first one that handles the event, unless that handler was added with `pass: true`.

## Input Handlers

An input handler is defined by 3 parameters:

1. Predicate

   Predicate that determines whether the conditions are satisfied and thus invoke the callback if the check has passed.

2. Callback

   Callback to invoke once the predicate is satisfied.

3. Pass

   Functionality similar to [Control.MouseFilter](https://docs.godotengine.org/en/stable/classes/class_control.html#:~:text=and%20size_flags_vertical.-,enum%20MouseFilter,-%3A). If true, the input event is propagated to the node's parent.

There are 4 ways to add a handler to an `InputManager`:

| Method | Matches |
| ------ | ------- |
| `AddHandler<TInputEvent>(TInputEvent inputEvent, Action<TInputEvent> handler, bool pass = false)` | Every event that matches the given input event, ignoring pressed state and modifiers. |
| `AddHandler<TInputEvent>(TInputEvent inputEvent, bool matchPressed, bool matchModifiers, Action<TInputEvent> handler, bool pass = false)` | Same as above, but the pressed state and modifiers of the given input event are optionally matched as well. |
| `AddHandler<TInputEvent>(Func<TInputEvent, bool> predicate, Action<TInputEvent> handler, bool pass = false)` | Every event satisfying the predicate. |
| `AddMouseMotionHandler(...)` / `AddKeyHandler(...)` | Shorthands for mouse motion events, and for keys with or without a `KeyModifierMask`. |

```csharp
// matching an input event prototype
_inputManager.AddHandler(new InputEventMouseButton { ButtonIndex = MouseButton.Left },
                         e => GD.Print("left button"));

// matching a key with a modifier
_inputManager.AddKeyHandler(Key.C, KeyModifierMask.MaskCtrl,
                            _ => DisplayServer.ClipboardSet(_lineEdit.Text));
```

`AddHandler<TInputEvent>` throws an `ArgumentException` for `InputEventMouseMotion`, since mouse motion events do not support [InputEvent.IsMatch()](https://docs.godotengine.org/en/stable/classes/class_inputevent.html#class-inputevent-method-is-match). Use `AddMouseMotionHandler` for those instead.
