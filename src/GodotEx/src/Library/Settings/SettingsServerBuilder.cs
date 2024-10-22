using Godot;

namespace GodotEx;

/// <summary>
/// Builder used for constructing <see cref="SettingsServer"/> with user configured settings.
/// </summary>
public class SettingsServerBuilder {
    private readonly Dictionary<string, Dictionary<string, object>> _settings = [];
    private readonly ConfigFile _file;
    private readonly string _path;
    private string _section = "Settings";
    private Mode _mode;

    /// <summary>
    /// Returns a new <see cref="SettingsServerBuilder"/> for constructing
    /// <see cref="SettingsServer"/> with user configured settings.
    /// </summary>
    /// <param name="path">Path of the settings config file.</param>
    public SettingsServerBuilder(string path = "user://settings.cfg") {
        _file = new ConfigFile();
        _path = path;
        _mode = _file.Load(path) == Error.Ok ? Mode.Read : Mode.Create;
    }

    /// <summary>
    /// Configures a new section, further settings will be added to this section.
    /// </summary>
    /// <param name="section">Section name.</param>
    /// <returns>The current <see cref="SettingsServerBuilder"/> instance.</returns>
    public SettingsServerBuilder WithSection(string section) {
        _section = section;
        return this;
    }

    /// <summary>
    /// Configures a new setting and adds it to the server.
    /// </summary>
    /// <typeparam name="T">Setting parameter type.</typeparam>
    /// <param name="key">Key where the setting is mapped from.</param>
    /// <param name="default">Setting default value.</param>
    /// <param name="setter">Action to perform when the setter of <see cref="Setting{T}.Value"/> is called.</param>
    /// <param name="predicate">Predicate to decide whether setting is allowed.</param>
    /// <returns>The current <see cref="SettingsServerBuilder"/> instance.</returns>
    /// <exception cref="ArgumentException">Duplicate key.</exception>
    public SettingsServerBuilder WithSetting<[MustBeVariant] T>(
            string key,
            T @default,
            Action<T> setter,
            Func<T, bool>? predicate = null) where T : notnull {
        if (!_settings.TryGetValue(_section, out var dict)) {
            dict = [];
            _settings.Add(_section, dict);
        }
        if (dict.ContainsKey(key)) {
            throw new ArgumentException($"Duplicate key {_section}-{key}.");
        }

        Setting<T> setting = default!;
        if (_mode.HasFlag(Mode.Create)) {
            _file.SetValue(_section, key, Variant.From(@default));
        } else if (!_file.HasSectionKey(_section, key)) {
            _file.SetValue(_section, key, Variant.From(@default));
            _mode |= Mode.Update;
        } else {
            var value = _file.GetValue(_section, key, Variant.From(@default)).As<T>();
            setting = new Setting<T>(_section, key, value, @default, setter, predicate);
        }
        
        setting ??= new Setting<T>(_section, key, @default, @default, setter, predicate);
        dict.Add(key, setting);

        setting.Updated += value => {
            _file.SetValue(setting.Section, setting.Key, Variant.From(value));
            _file.Save(_path);
        };

        return this;
    }

    /// <summary>
    /// Finalize the builder and creates a new <see cref="SettingsServer"/> with configured settings.
    /// </summary>
    /// <returns><see cref="SettingsServer"/> with the configured settings.</returns>
    public SettingsServer Build() {
        if (_mode != Mode.Read) {
            _file.Save(_path);
        }
        return new SettingsServer(_settings);
    }

    [Flags]
    private enum Mode {
        Read = 0,
        Create = 0b01,
        Update = 0b10
    }
}
