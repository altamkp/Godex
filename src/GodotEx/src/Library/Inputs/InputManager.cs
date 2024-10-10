using Godot;

namespace GodotEx;

/// <summary>
/// Input manager for handling <see cref="InputEvent"/>s.
/// </summary>
public class InputManager {
    private static readonly Func<InputEvent, bool> TRUE = e => true;

    private readonly List<IInputHandler> _inputHandlers = new();
    private readonly Viewport _viewport;

    /// <summary>
    /// Create new instance of <see cref="InputManager"/>.
    /// </summary>
    /// <param name="viewport">Viewport to use.</param>
    public InputManager(Viewport viewport) {
        _viewport = viewport;
    }

    /// <summary>
    /// Add input handler for InputEventMouseMotion, since InputEventMouseMotion does not support
    /// <see cref="InputEvent.IsMatch(InputEvent, bool)"/>, it should be handled .
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="pass">should pass the event to upper leven even handled</param>
    public void AddMouseMotionHandler(Action<InputEventMouseMotion> handler, bool pass = false) {
        AddMouseMotionHandler(TRUE, handler, pass);
    }

    /// <summary>
    /// Add input handler for InputEventMouseMotion, since InputEventMouseMotion does not support action,
    /// it should be handled specifically.
    /// </summary>
    /// <param name="predicate"> Predicate if the handler should be used.</param>
    /// <param name="handler"></param>
    /// <param name="pass">should pass the event to upper leven even handled</param>
    public void AddMouseMotionHandler(Func<InputEventMouseMotion, bool> predicate,
                                      Action<InputEventMouseMotion> handler,
                                      bool pass = false) {
        _inputHandlers.Add(new InputHandler<InputEventMouseMotion>(predicate, handler, pass));
    }

    /// <summary>
    /// Add input handler for InputEventKey, This would match pressed key and require all modifiers
    /// like shift, ctrl, alt to *not* pressed.
    /// </summary>
    /// <param name="key">key pressed</param>
    /// <param name="handler">the handler to call if inputEvent matches</param>
    /// <param name="pass">should pass the event to upper leven even handled</param>
    /// <exception cref="ArgumentException">if inputEvent is InputEventMouseMotion or name has already been registered</exception>
    public void AddKeyHandler(Key key, Action<InputEventKey> handler, bool pass = false) {
        AddHandler<InputEventKey>(e => e.Keycode == key
            && e.Pressed == true
            && e.AltPressed == false
            && e.CtrlPressed == false
            && e.ShiftPressed == false, handler, pass);
    }

    /// <summary>
    /// Add input handler for InputEventKey, This would match pressed key and match modifiers
    /// like shift, ctrl, alt to modifierMask.
    /// </summary>
    /// <param name="key">key pressed</param>
    /// <param name="modifierMask">the modifiers to match</param>
    /// <param name="handler">the handler to call if inputEvent matches</param>
    /// <param name="pass">should pass the event to upper leven even handled</param>
    /// <exception cref="ArgumentException">if inputEvent is InputEventMouseMotion or name has already been registered</exception>
    public void AddKeyHandler(Key key,
                              KeyModifierMask modifierMask,
                              Action<InputEventKey> handler,
                              bool pass = false) {
        var altPressed = modifierMask.HasFlag(KeyModifierMask.MaskAlt);
        var ctrlPressed = modifierMask.HasFlag(KeyModifierMask.MaskCtrl)
            || modifierMask.HasFlag(KeyModifierMask.MaskCmdOrCtrl);
        var shiftPressed = modifierMask.HasFlag(KeyModifierMask.MaskShift);
        AddHandler<InputEventKey>(e => e.Keycode == key
            && e.Pressed == true
            && e.AltPressed == altPressed
            && e.CtrlPressed == ctrlPressed
            && e.ShiftPressed == shiftPressed, handler, pass);
    }

    /// <summary>
    /// Add input handler for InputEvent, such InputEventKey, InputEventMouseButton, InputEventJoypadButton,
    /// InputEventJoyPadMotion and InputEventAction. This would match both pressed and released event and
    /// ignore all modifiers.
    /// </summary>
    /// <typeparam name="TInputEvent">the type of InputEvent to handler</typeparam>
    /// <param name="inputEvent">the inputEvent to match. </param>
    /// <param name="handler">the handler to call if inputEvent matches</param>
    /// <param name="pass">should pass the event to upper leven even handled</param>
    /// <exception cref="ArgumentException">if inputEvent is InputEventMouseMotion or name has already been registered</exception>
    public void AddHandler<TInputEvent>(TInputEvent inputEvent,
                                        Action<TInputEvent> handler,
                                        bool pass = false)
            where TInputEvent : InputEvent {
        AddHandler(e => e.IsMatch(inputEvent, false), handler, pass);
    }

    /// <summary>
    /// Add input handler for InputEvent, such InputEventKey, InputEventMouseButton, InputEventJoypadButton,
    /// InputEventJoyPadMotion and InputEventAction. This would match only pressed or released event depends
    /// on <paramref name="matchPressed"/> and match modifiers depends on <paramref name="matchModifiers"/>.
    /// </summary>
    /// <typeparam name="TInputEvent">the type of InputEvent to handler</typeparam>
    /// <param name="inputEvent">the inputEvent to match. </param>
    /// <param name="matchPressed">match pressed specified in <paramref name="inputEvent"/> if true, otherwise ignore pressed</param>
    /// <param name="matchModifiers">match modifier specified in <paramref name="inputEvent"/> if true, otherwise ignore all modifiers</param>
    /// <param name="handler">the handler to call if inputEvent matches</param>
    /// <param name="pass">should pass the event to upper leven even handled</param>
    /// <exception cref="ArgumentException">if inputEvent is InputEventMouseMotion or name has already been registered</exception>
    public void AddHandler<TInputEvent>(TInputEvent inputEvent,
                                        bool matchPressed,
                                        bool matchModifiers,
                                        Action<TInputEvent> handler,
                                        bool pass = false)
            where TInputEvent : InputEvent {
        Func<TInputEvent, bool> predicate = matchPressed
            ? e => e.IsPressed() == inputEvent.IsPressed() && e.IsMatch(inputEvent, matchModifiers)
            : e => e.IsMatch(inputEvent, matchModifiers);
        AddHandler(predicate, handler, pass);
    }

    /// <summary>
    /// Add input handler for InputEvent, such InputEventKey, InputEventMouseButton, InputEventJoypadButton,
    /// InputEventJoyPadMotion and InputEventAction. This would match all input events that match
    /// <paramref name="predicate"/>.
    /// </summary>
    /// <typeparam name="TInputEvent">the type of InputEvent to handler</typeparam>
    /// <param name="predicate">the predicate which matches event to handle</param>
    /// <param name="handler">the handler to call if inputEvent matches</param>
    /// <param name="pass">should pass the event to upper leven even handled</param>
    /// <exception cref="ArgumentException">if inputEvent is InputEventMouseMotion or name has already been registered</exception>
    public void AddHandler<TInputEvent>(Func<TInputEvent, bool> predicate,
                                        Action<TInputEvent> handler,
                                        bool pass = false)
            where TInputEvent : InputEvent {
        if (typeof(TInputEvent) == typeof(InputEventMouseMotion)) {
            throw new ArgumentException($"Use AddMouseMotionInputHandler instead.");
        }
        _inputHandlers.Add(new InputHandler<TInputEvent>(predicate, handler, pass));
    }

    /// <summary>
    /// Handle <see cref="InputEvent"/>.
    /// </summary>
    /// <param name="inputEvent">Input event to handle.</param>
    /// <returns>If handled, return true.</returns>
    public bool Handle(InputEvent inputEvent) {
        foreach (IInputHandler inputHandler in _inputHandlers) {
            if (inputHandler.Handle(inputEvent) && !inputHandler.Pass) {
                _viewport.SetInputAsHandled();
                return true;
            }
        }
        return false;
    }

    private interface IInputHandler {
        public bool Pass { get; }

        bool Handle(InputEvent inputEvent);
    }

    private class InputHandler<TInputEvent> : IInputHandler where TInputEvent : InputEvent {
        private readonly Func<TInputEvent, bool> _predicate;
        private readonly Action<TInputEvent> _handler;

        public InputHandler(Func<TInputEvent, bool> predicate, Action<TInputEvent> handler, bool pass) {
            Pass = pass;
            _predicate = predicate;
            _handler = handler;
        }

        public string Name { get; }
        public bool Pass { get; }

        public bool Handle(InputEvent inputEvent) {
            if (inputEvent is not TInputEvent tInputEvent) {
                return false;
            }

            if (!_predicate(tInputEvent)) {
                return false;
            }

            _handler(tInputEvent);
            return true;
        }
    }
}
