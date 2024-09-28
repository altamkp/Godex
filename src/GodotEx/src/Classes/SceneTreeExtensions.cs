using Godot;

namespace GodotEx;

/// <summary>
/// Extensions for <see cref="SceneTree"/>.
/// </summary>
public static class SceneTreeExtensions {
    private static readonly Dictionary<Type, Node> SINGLETON_NODES = new();

    /// <summary>
    /// Adds <paramref name="node"/> to the scene tree as singleton node.
    /// </summary>
    /// <param name="tree">Scene tree where the node is added.</param>
    /// <param name="node">Node to add.</param>
    /// <exception cref="InvalidOperationException">Node with the same type already exists.</exception>
    public static void AddSingleton(this SceneTree tree, Node node) {
        var type = node.GetType();
        if (!SINGLETON_NODES.TryAdd(type, node)) {
            throw new InvalidOperationException($"Singleton node of type {type.Name} already exists.");
        }
    }

    /// <summary>
    /// Removes <paramref name="node"/> from the scene tree as singleton node.
    /// </summary>
    /// <param name="tree">Scene tree where the node is removed.</param>
    /// <param name="node">Node to remove.</param>
    /// <exception cref="InvalidOperationException">Node with the same type not found.</exception>
    public static void RemoveSingleton(this SceneTree tree, Node node) {
        var type = node.GetType();
        if (!SINGLETON_NODES.Remove(type)) {
            throw new InvalidOperationException($"Singleton node of type {type.Name} not found.");
        }
    }

    /// <summary>
    /// Returns singleton node of type <paramref name="type"/>.
    /// </summary>
    /// <param name="tree">Scene tree to search from.</param>
    /// <param name="type">Type to match.</param>
    /// <returns>Node of <paramref name="type"/>, null if not found.</returns>
    public static Node? GetSingle(this SceneTree tree, Type type) {
        SINGLETON_NODES.TryGetValue(type, out var node);
        return node;
    }

    /// <summary>
    /// Returns singleton node of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Type to match.</typeparam>
    /// <param name="tree">Scene tree to search from.</param>
    /// <returns>Node of type <typeparamref name="T"/>, null if not found.</returns>
    public static T? GetSingle<T>(this SceneTree tree) where T : Node {
        SINGLETON_NODES.TryGetValue(typeof(T), out var node);
        return node as T;
    }

    /// <summary>
    /// Returns singleton node of type <paramref name="type"/>.
    /// </summary>
    /// <param name="tree">Scene tree to search from.</param>
    /// <param name="type">Type to match.</param>
    /// <returns>Node of <paramref name="type"/>.</returns>
    /// <exception cref="InvalidOperationException">Node of <paramref name="type"/> not found.</exception>
    public static Node GetRequiredSingle(this SceneTree tree, Type type) {
        return tree.GetSingle(type)
            ?? throw new InvalidOperationException($"Singleton node of type {type.Name} not found.");
    }

    /// <summary>
    /// Returns singleton node of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Type to match.</typeparam>
    /// <param name="tree">Scene tree to search from.</param>
    /// <returns>Node of type <typeparamref name="T"/>.</returns>
    /// <exception cref="InvalidOperationException">Node of type <typeparamref name="T"/> not found.</exception>
    public static T GetRequiredSingle<T>(this SceneTree tree) where T : Node {
        return tree.GetSingle<T>()
            ?? throw new InvalidOperationException($"Singleton node of type {typeof(T).Name} not found.");
    }
}
