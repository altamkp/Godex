using DotEx.Reflections;
using Godot;
using System.Reflection;

namespace GodotEx;

/// <summary>
/// Server that manages <see cref="Setting{T}"/> instances, which are abstractions over a
/// setting stored in a config file. When a setting is updated by assigning a value to
/// <see cref="Setting{T}.Value"/>, the server updates the config file to reflect this change.
/// Upon initialization, the server is also responsible for creating the config file if it 
/// does not exist, or would otherwise load in the settings from the config file directly.
/// <br/><br/>
/// You can obtain a <see cref="Setting{T}"/> by using <see cref="Inject(Node)"/>, which
/// injects settings specified by their respective sections and keys to class properties
/// and fields labeled with <see cref="SettingAttribute"/>s. Alternatively, you can use
/// <see cref="GetSetting{T}(string, string)"/> to retrieve a single setting.
/// </summary>
public class SettingsServer {
    private const BindingFlags FLAGS = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

    private static readonly Dictionary<Type, SettingInfo[]> INFOS = new();

    private readonly Dictionary<string, Dictionary<string, object>> _settings = new();

    internal SettingsServer(Dictionary<string, Dictionary<string, object>> settings) {
        _settings = settings;
    }

    /// <summary>
    /// Injects settings specified by their respective sections and keys to class properties
    /// and fields labeled with <see cref="SettingAttribute"/>s.
    /// </summary>
    /// <param name="node">Node with settings to inject.</param>
    public void Inject(Node node) {
        var type = node.GetType();

        if (!INFOS.TryGetValue(type, out var infos)) {
            infos = type.GetPropertiesAndAttributes<SettingAttribute>(FLAGS)
                    .Select(v => new SettingInfo(v.Attributes.Single(), v.PropertyInfo))
                .Concat(type.GetFieldsAndAttributes<SettingAttribute>(FLAGS)
                    .Select(v => new SettingInfo(v.Attributes.Single(), v.FieldInfo)))
                .ToArray();
            INFOS.Add(type, infos);
        }

        foreach (var info in infos) {
            info.Inject(this, node);
        }
    }

    /// <summary>
    /// Returns a <see cref="Setting{T}"/> specified by its section and key.
    /// </summary>
    /// <typeparam name="T">Setting parameter type.</typeparam>
    /// <param name="section">Section where the setting is stored.</param>
    /// <param name="key">Key where the setting is mapped from.</param>
    /// <returns><see cref="Setting{T}"/> specified by its section and key.</returns>
    /// <exception cref="ArgumentException">Section or key not found.</exception>
    /// <exception cref="InvalidCastException">Setting is not of type <typeparamref name="T"/>.</exception>
    public Setting<T> GetSetting<[MustBeVariant] T>(string section, string key) where T : notnull {
        if (!_settings.TryGetValue(section, out var dict)) {
            throw new ArgumentException($"Section {section} not found.");
        }
        if (!dict.TryGetValue(key, out var obj)) {
            throw new ArgumentException($"Setting {section}-{key} not found.");
        }
        if (obj is not Setting<T> setting) {
            throw new InvalidCastException($"Setting {section}-{key} is not of type {typeof(T)}.");
        }
        return setting;
    }

    internal object GetSetting(string section, string key, Type type) {
        if (!_settings.TryGetValue(section, out var dict)) {
            throw new ArgumentException($"Section {section} not found.");
        }
        if (!dict.TryGetValue(key, out var setting)) {
            throw new ArgumentException($"Setting {section}-{key} not found.");
        }

        if (setting.GetType().GetGenericArguments()[0] != type) {
            throw new InvalidCastException($"Setting {section}-{key} is not of type {type}.");
        }
        return setting;
    }

    private class SettingInfo {
        private static readonly Type SETTING_TYPE = typeof(Setting<>);

        private readonly string _section;
        private readonly string _key;
        private readonly MemberInfo _memberInfo;
        private readonly Type _type;

        public SettingInfo(SettingAttribute attribute, MemberInfo memberInfo) {
            _section = attribute.Section;
            _key = attribute.Key;

            var memberType = memberInfo.GetMemberType();
            if (!memberType.IsGenericType || memberType.GetGenericTypeDefinition() != SETTING_TYPE) {
                throw new InvalidOperationException($"Member {memberInfo.Name} is not of type Setting.");
            }

            _memberInfo = memberInfo;
            _type = memberType.GetGenericArguments()[0];
        }


        public void Inject(SettingsServer server, Node node) {
            var setting = server.GetSetting(_section, _key, _type);
            _memberInfo.SetValue(node, setting);
        }
    }
}
