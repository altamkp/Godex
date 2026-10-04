using System.Reflection;

namespace Godex;

/// <summary>
/// Extensions for <see cref="Type"/>.
/// </summary>
public static class TypeExtensions {
    /// <summary>
    /// Returns all fields of <paramref name="type"/> labeled by <typeparamref name="TAttribute"/>,
    /// together with the attributes applied to each of them.
    /// </summary>
    /// <typeparam name="TAttribute">Attribute to look for.</typeparam>
    /// <param name="type">Type to search.</param>
    /// <param name="flags">Binding flags to use.</param>
    /// <returns>Fields matching <typeparamref name="TAttribute"/> and their attributes.</returns>
    public static IEnumerable<(FieldInfo FieldInfo, TAttribute[] Attributes)> GetFieldsAndAttributes<TAttribute>(
        this Type type, BindingFlags flags) where TAttribute : Attribute {
        foreach (var field in type.GetFields(flags)) {
            var attributes = field.GetCustomAttributes<TAttribute>().ToArray();
            if (attributes.Any()) {
                yield return (field, attributes);
            }
        }
    }

    /// <summary>
    /// Returns all properties of <paramref name="type"/> labeled by <typeparamref name="TAttribute"/>,
    /// together with the attributes applied to each of them.
    /// </summary>
    /// <typeparam name="TAttribute">Attribute to look for.</typeparam>
    /// <param name="type">Type to search.</param>
    /// <param name="flags">Binding flags to use.</param>
    /// <returns>Properties matching <typeparamref name="TAttribute"/> and their attributes.</returns>
    public static IEnumerable<(PropertyInfo PropertyInfo, TAttribute[] Attributes)> GetPropertiesAndAttributes<TAttribute>(
        this Type type, BindingFlags flags) where TAttribute : Attribute {
        foreach (var property in type.GetProperties(flags)) {
            var attributes = property.GetCustomAttributes<TAttribute>().ToArray();
            if (attributes.Any()) {
                yield return (property, attributes);
            }
        }
    }

    /// <summary>
    /// Returns all fields of <paramref name="type"/> labeled by <typeparamref name="TAttribute"/>.
    /// </summary>
    /// <typeparam name="TAttribute">Attribute to look for.</typeparam>
    /// <param name="type">Type to search.</param>
    /// <param name="flags">Binding flags to use.</param>
    /// <returns>Fields matching <typeparamref name="TAttribute"/>.</returns>
    public static IEnumerable<FieldInfo> GetFieldsWithAttribute<TAttribute>(
        this Type type, BindingFlags flags) where TAttribute : Attribute {
        return type.GetFieldsAndAttributes<TAttribute>(flags).Select(field => field.FieldInfo);
    }

    /// <summary>
    /// Returns all properties of <paramref name="type"/> labeled by <typeparamref name="TAttribute"/>.
    /// </summary>
    /// <typeparam name="TAttribute">Attribute to look for.</typeparam>
    /// <param name="type">Type to search.</param>
    /// <param name="flags">Binding flags to use.</param>
    /// <returns>Properties matching <typeparamref name="TAttribute"/>.</returns>
    public static IEnumerable<PropertyInfo> GetPropertiesWithAttribute<TAttribute>(
        this Type type, BindingFlags flags) where TAttribute : Attribute {
        return type.GetPropertiesAndAttributes<TAttribute>(flags).Select(property => property.PropertyInfo);
    }
}
