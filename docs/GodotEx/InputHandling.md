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

        _shortcutManager.AddHandler<InputEventMouseMotion>("TooltipFollow",
            _ => true,
            m => {
                if (GetRect().HasPoint(m.Position)) {
                    _tooltip.Visible = true;
                    _tooltip.GlobalPosition = m.GlobalPosition;
                } else {
                    _tooltip.Visible = true;
                }
            });

        _inputManager.AddHandler<InputEventKey>("Quit",
            k => k.IsEscKeyPressed(),
            _ => GetTree().Quit());

        _inputManager.AddHandler<InputEventKey>("Focus",
            k => k.IsKeyPressed(Key.Tab),
            _ => _lineEdit.GrabFocus());

        _inputManager.AddHandler<InputEventKey>("Copy",
            k => k.IsKeyPressed(Key.C, KeyModifierMask.MaskCtrl),
            _ => _lineEdit.GrabFocus());

        _inputManager.AddHandler<InputEventKey>("Paste",
            k => k.IsKeyPressed(Key.V, KeyModifierMask.MaskCtrl),
            _ => _lineEdit.GrabFocus());
    }

    public override void _ShortcutInput(InputEvent @event) => _shortcutManager.Handle(@event);
    public override void _UnhandledInput(InputEvent @event) => _inputManager.Handle(@event);
}
```

Any input manager can be disabled by calling `inputManager.Disabled = true`. Calling `inputManager.Handle(@event)` on a disabled manager has no effect.

## Input Handlers

`InputHandler`s are the underlying types within an `InputManager` that actually handle the input event delegates. You can you api in `InputManager` to add/remove, disable/enable any handler at runtime. You can also create custom `InputHandler`s and override the default `InputHandler.Handle(InputEvent)` function.

An input handler is defined by 4 parameters:

1. Name
   
   Name that is used for identifying a handler within an `InputManager`.

2. Predicate

   Predicate that determines whether the conditions are satisfied and thus invoke the callback if the check has passed.

3. Callback

   Callback to invoke once the predicate is satisfied.

4. Pass

   Functionality similar to [Control.MouseFilter](https://docs.godotengine.org/en/stable/classes/class_control.html#:~:text=and%20size_flags_vertical.-,enum%20MouseFilter,-%3A). If true, the input event is propagated to the node's parent.

Any input handler can be disabled individually by calling `inputHandler.Disabled = true` or calling `inputManager.DisableHandler(handlerName)`.
