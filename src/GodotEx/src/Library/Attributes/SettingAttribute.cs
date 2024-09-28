namespace GodotEx;

/// <summary>
/// Recognized by <see cref="SettingsServer"/> to inject <see cref="Setting{T}"/>
/// instances to properties and fields labeled with this attribute.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class SettingAttribute : Attribute {
    /// <summary>
    /// Creates a new <see cref="SettingAttribute"/> that is recognized by 
    /// <see cref="SettingsServer"/> to inject <see cref="Setting{T}"/>
    /// instances to properties and fields labeled with this attribute.
    /// </summary>
    /// <param name="section">Section where the setting is stored.</param>
    /// <param name="key">Key where the setting is mapped from.</param>
    public SettingAttribute(string section, string key) {
        Section = section;
        Key = key;
    }

    internal string Section { get; }
    internal string Key { get; }
}
