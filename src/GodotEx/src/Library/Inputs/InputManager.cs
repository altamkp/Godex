using Godot;

namespace GodotEx;

/// <summary>
/// Manager class for registering <see cref="InputEvent"/>s and handling them.
/// </summary>
public class InputManager {
    private readonly Dictionary<string, IInputHandler> _handlers = new();
    private readonly Viewport _viewport;

    /// <summary>
    /// Instantiates new <see cref="InputManager"/> for registering <see cref="InputEvent"/>s
    /// and handling them.
    /// </summary>
    /// <param name="viewport"></param>
    public InputManager(Viewport viewport) {
        _viewport = viewport;
    }

    /// <summary>
    /// If true, calling <see cref="Handle(InputEvent)"/> has no effect.
    /// </summary>
    public bool Disabled { get; set; } = false;

    /// <summary>
    /// Adds input handler for <see cref="InputEvent"/>s which matches all input events that satisfy <paramref name="predicate"/>.
    /// </summary>
    /// <typeparam name="TInputEvent">Type of <see cref="InputEvent"/> to handle.</typeparam>
    /// <param name="id">Id of the handler.</param>
    /// <param name="predicate">Predicate which matches event to handle.</param>
    /// <param name="handler">Handler to call if input event matches successfully.</param>
    /// <param name="pass">Whether the input would be passed to its parent after it has been handled.</param>
    public void AddHandler<TInputEvent>(string id,
                                        Predicate<TInputEvent> predicate,
                                        Action<TInputEvent> handler,
                                        bool pass = false)
                                            where TInputEvent : InputEvent {
        if (_handlers.ContainsKey(id)) {
            throw new ArgumentException($"{id} has already been registered.");
        }
        _handlers.Add(id, new InputHandler<TInputEvent>(id, predicate, handler, pass));
    }

    /// <summary>
    /// Adds custom input handler.
    /// </summary>
    /// <param name="handler">Handler to add.</param>
    /// <exception cref="ArgumentException"></exception>
    public void AddHandler<T>(InputHandler<T> handler) where T : InputEvent {
        if (!_handlers.TryAdd(handler.Id, handler)) {
            throw new ArgumentException($"{handler.Id} has already been registered.");
        }
    }

    /// <summary>
    /// Removes input handler with the given <paramref name="id"/>.
    /// </summary>
    /// <param name="id">Name of the handler to remove.</param>
    /// <exception cref="ArgumentException">Handler was not registered.</exception>
    public void RemoveHandler(string id) {
        if (!_handlers.Remove(id)) {
            throw new ArgumentException($"{id} was not registered.");
        }
    }

    /// <summary>
    /// Enables input handler by its given <paramref name="id"/>.
    /// </summary>
    /// <param name="id">Name of the handler to enable.</param>
    /// <exception cref="ArgumentException">Handler was not registered.</exception>
    public void EnableHandler(string id) {
        if (!_handlers.TryGetValue(id, out var handler)) {
            throw new ArgumentException($"{id} was not registered.");
        }
        handler.Disabled = false;
    }

    /// <summary>
    /// Disables input handler by its given <paramref name="id"/>.
    /// </summary>
    /// <param name="id">Name of the handler to disable.</param>
    /// <exception cref="ArgumentException">Handler was not registered.</exception>
    public void DisableHandler(string id) {
        if (!_handlers.TryGetValue(id, out var handler)) {
            throw new ArgumentException($"{id} was not registered.");
        }
        handler.Disabled = true;
    }

    /// <summary>
    /// Handles the provided <paramref name="event"/>. This is normally used within override Godot input methods such as
    /// <see cref="Node._Input(InputEvent)"/>, <see cref="Node._UnhandledInput(InputEvent)"/>, <see cref="Control._GuiInput(InputEvent)"/> etc.
    /// </summary>
    /// <param name="event">Input event to handle.</param>
    public void Handle(InputEvent @event) {
        if (Disabled) {
            return;
        }

        foreach (var (_, handler) in _handlers) {
            if (handler.Handle(@event) && !handler.Pass) {
                _viewport.SetInputAsHandled();
                return;
            }
        }
    }
}
