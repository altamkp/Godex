using Godot;

namespace GodotEx;

/// <summary>
/// Extensions for <see cref="GodotObject"/>.
/// </summary>
public static class GodotObjectExtensions {
    /// <summary>
    /// Returns the object's metadata value for the given entry name.
    /// </summary>
    /// <typeparam name="T">Type to cast the result to.</typeparam>
    /// <param name="obj">Object that holds the metadata.</param>
    /// <param name="name">Metadata name.</param>
    /// <returns>Metadata value.</returns>
    /// <exception cref="ArgumentException">No metadata named <paramref name="name"/> is found.</exception>
    public static T GetMeta<[MustBeVariant] T>(this GodotObject obj, string name) {
        if (!obj.TryGetMeta<T>(name, out var meta)) {
            throw new ArgumentException($"No metadata named {name} found in {obj.GetClass()}.");
        }
        return meta;
    }

    /// <summary>
    /// Tries returning the object's metadata value for the given entry name.
    /// </summary>
    /// <param name="obj">Object that holds the metadata.</param>
    /// <param name="name">Metadata name.</param>
    /// <param name="meta">Metadata value.</param>
    /// <returns>True if metadata named <paramref name="name"/> is found, otherwise false.</returns>
    public static bool TryGetMeta(this GodotObject obj, string name, out Variant meta) {
        var res = obj.HasMeta(name);
        meta = res ? obj.GetMeta(name) : default;
        return res;
    }

    /// <summary>
    /// Tries returning the object's metadata value for the given entry name.
    /// </summary>
    /// <typeparam name="T">Type to cast the result to.</typeparam>
    /// <param name="obj">Object that holds the metadata.</param>
    /// <param name="name">Metadata name.</param>
    /// <param name="meta">Metadata value.</param>
    /// <returns>True if metadata named <paramref name="name"/> is found, otherwise false.</returns>
    /// <exception cref="InvalidCastException">Object has metadata <paramref name="name"/> 
    /// but has failed to cast to type <typeparamref name="T"/>.</exception>
    public static bool TryGetMeta<[MustBeVariant] T>(this GodotObject obj, string name, out T meta) {
        var res = obj.HasMeta(name);
        meta = !res
            ? default!
            : obj.GetMeta(name).As<T>() 
                ?? throw new InvalidCastException($"Failed to cast {name} to {typeof(T).Name}.");
        return res;
    }
}
