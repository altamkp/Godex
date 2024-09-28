using Godot;

namespace GodotEx;

/// <summary>
/// An abstraction for any setting fields of type <typeparamref name="T"/> in the game.
/// All settings are managed by the <see cref="SettingsServer"/> such then when you set 
/// <see cref="Setting{T}.Value"/>, the cfg file that stores this value is automatically
/// updated.
/// </summary>
/// <typeparam name="T">Setting parameter type.</typeparam>
public class Setting<[MustBeVariant] T> where T : notnull {
    private static readonly Func<T, bool> TRUE = _ => true;

    private readonly Action<T> _setter;
    private readonly Func<T, bool> _predicate;

    private T _value;
    
    internal Setting(string section, string key, T @default, Action<T> setter, Func<T, bool>? predicate = null) {
        Section = section;
        Key = key;
        _value = @default;
        Default = @default;

        _setter = setter;
        _predicate = predicate ?? TRUE;
    }

    /// <summary>
    /// Fired after the setter has been successfully called.
    /// </summary>
    public event Action<T> Updated = delegate { };

    /// <summary>
    /// Section where the setting is stored.
    /// </summary>
    public string Section { get; }

    /// <summary>
    /// Key where the setting is mapped from.
    /// </summary>
    public string Key { get; }

    /// <summary>
    /// Actual value of the setting.
    /// </summary>
    public T Value {
        get => _value;
        set {
            if (!_predicate(value)) {
                throw new ArgumentException($"Argument {value} does not satisfy predicate.");
            }
            _value = value;
            _setter(_value);
            Updated(_value);
        }
    }

    /// <summary>
    /// Default value of the setting.
    /// </summary>
    public T Default { get; }
}
