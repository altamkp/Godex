using Godot;

namespace GodotEx;

/// <summary>
/// Handler class for handling input event of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T"></typeparam>
public class InputHandler<T> : IInputHandler where T : InputEvent {
    private readonly Predicate<T> _predicate;
    private readonly Action<T> _callback;

    /// <summary>
    /// Constructs an new input handler object.
    /// </summary>
    /// <param name="id">Id of the handler.</param>
    /// <param name="pass">True input would be passed to its parent after it has been handled.</param>
    /// <param name="predicate">Predicate to satisfy upon handling input events.</param>
    /// <param name="callback">Handler to execute when predicate is satisfied.</param>
    public InputHandler(string id, Predicate<T> predicate, Action<T> callback, bool pass = false) {
        Id = id;
        _predicate = predicate;
        _callback = callback;
        Pass = pass;
    }

    /// <summary>
    /// Id of the handler.
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Whether the input would be passed to its parent after it has been handled.
    /// </summary>
    public bool Pass { get; } = false;

    /// <summary>
    /// When disabled, the input event is not handled.
    /// </summary>
    public bool Disabled { get; set; } = false;

    /// <summary>
    /// Handles the given input event.
    /// </summary>
    /// <param name="event">Input event to handle.</param>
    /// <returns>True if successfully handled, otherwise false due to handler being disabled, input event 
    /// type not matching <typeparamref name="T"/>, or predicate not satisfied.</returns>
    public virtual bool Handle(InputEvent @event) {
        if (Disabled || @event is not T tInputEvent || !_predicate(tInputEvent)) {
            return false;
        }
        _callback(tInputEvent);
        return true;
    }
}
