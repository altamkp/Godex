using System.Reflection;

namespace Godex;

/// <summary>
/// Extensions for <see cref="MemberInfo"/>.
/// </summary>
public static class MemberInfoExtensions {
    /// <summary>
    /// Returns the type of the field or property that <paramref name="member"/> represents.
    /// </summary>
    /// <param name="member">Member to get the type of.</param>
    /// <returns>Type of <paramref name="member"/>.</returns>
    /// <exception cref="ArgumentException">Member is neither a field nor a property.</exception>
    public static Type GetMemberType(this MemberInfo member) {
        return member switch {
            FieldInfo field => field.FieldType,
            PropertyInfo property => property.PropertyType,
            _ => throw NotFieldOrProperty(member),
        };
    }

    /// <summary>
    /// Returns the value of the field or property that <paramref name="member"/> represents.
    /// </summary>
    /// <param name="member">Member to get the value of.</param>
    /// <param name="target">Object to get the value from.</param>
    /// <returns>Value of <paramref name="member"/>.</returns>
    /// <exception cref="ArgumentException">Member is neither a field nor a property.</exception>
    public static object? GetValue(this MemberInfo member, object? target) {
        return member switch {
            FieldInfo field => field.GetValue(target),
            PropertyInfo property => property.GetValue(target),
            _ => throw NotFieldOrProperty(member),
        };
    }

    /// <summary>
    /// Sets the value of the field or property that <paramref name="member"/> represents.
    /// </summary>
    /// <param name="member">Member to set the value of.</param>
    /// <param name="target">Object to set the value on.</param>
    /// <param name="value">Value to set.</param>
    /// <exception cref="ArgumentException">Member is neither a field nor a property.</exception>
    public static void SetValue(this MemberInfo member, object? target, object? value) {
        switch (member) {
            case FieldInfo field:
                field.SetValue(target, value);
                break;
            case PropertyInfo property:
                property.SetValue(target, value);
                break;
            default:
                throw NotFieldOrProperty(member);
        }
    }

    private static ArgumentException NotFieldOrProperty(MemberInfo member) {
        return new ArgumentException($"Member {member.Name} is neither a field nor a property.", nameof(member));
    }
}
