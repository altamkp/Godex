using Godot;
using System.Reflection;

namespace Godex;

/// <summary>
/// Extensions for <see cref="Node"/>.
/// </summary>
public static partial class NodeExtensions {
    private const BindingFlags INSTANCE_FLAGS = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
    private const BindingFlags PUBLIC_INSTANCE_FLAGS = BindingFlags.Instance | BindingFlags.Public;

    private const string RESOLVED = "resolved";

    /// <summary>
    /// Checks if the dependencies of <paramref name="node"/> labeled by 
    /// <see cref="NodePathAttribute"/> has been resolved. Note that nodes created
    /// by <see cref="GDx.New{T}(Action{T}?)"/> and its overload methods automatically resolves
    /// the node as soon as it is instantiated.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public static bool IsResolved(this Node node) {
        return node.TryGetMeta(RESOLVED, out bool resolved) && resolved;
    }

    /// <summary>
    /// Resolve <paramref name="node"/> the node's node paths, bit flags and group.
    /// See <see cref="ResolveNodePaths(Node)"/>, <see cref="ResolveBitFlags(Node)"/> and <see cref="ResolveGroup(Node)"/>.
    /// Resolving more than once has no effect and finishes in time O(1);
    /// </summary>
    /// <param name="node">Node to resolve.</param>
    public static void Resolve(this Node node) {
        if (node.IsResolved()) {
            return;
        }

        node.ResolveNodePaths();
        node.ResolveBitFlags();
        node.ResolveGroup();

        node.SetMeta(RESOLVED, true);
    }

    /// <summary>
    /// Returns all children of type <typeparamref name="T"/> under <paramref name="node"/>.
    /// </summary>
    /// <typeparam name="T">Target type.</typeparam>
    /// <param name="node">Node to search.</param>
    /// <returns>All children matching <typeparamref name="T"/>.</returns>
    public static IEnumerable<T> GetChildren<T>(this Node node) where T : Node {
        foreach (var child in node.GetChildren()) {
            if (child is T t) {
                yield return t;
            }
        }
    }

    /// <summary>
    /// Returns all descendants under <paramref name="node"/> recursively.
    /// </summary>
    /// <param name="node">Node to search.</param>
    /// <returns>All descendants.</returns>
    public static IEnumerable<Node> GetDescendants(this Node node) {
        foreach (var child in node.GetChildren()) {
            yield return child;

            foreach (var descendant in child.GetDescendants()) {
                yield return descendant;
            }
        }
    }

    /// <summary>
    /// Returns all descendants of type <typeparamref name="T"/> under <paramref name="node"/> recursively.
    /// </summary>
    /// <typeparam name="T">Target type.</typeparam>
    /// <param name="node">Node to search.</param>
    /// <returns>All descendants matching <typeparamref name="T"/>.</returns>
    public static IEnumerable<T> GetDescendants<T>(this Node node) where T : Node {
        foreach (var child in node.GetChildren()) {
            if (child is T t) {
                yield return t;
            }

            foreach (var descendant in child.GetDescendants<T>()) {
                yield return descendant;
            }
        }
    }

    /// <summary>
    /// Returns closest ancestor of type <typeparamref name="T"/> of the <paramref name="node"/>, null if not found.
    /// </summary>
    /// <typeparam name="T">Target type.</typeparam>
    /// <param name="node">Node to search.</param>
    /// <returns>Closest ancestor of type <typeparamref name="T"/>, null if not found.</returns>
    public static T? GetClosestAncestor<T>(this Node node) where T : Node {
        var parent = node.GetParent();
        while (parent != null && parent is not T) {
            parent = parent.GetParent();
        }
        return parent as T;
    }

    /// <summary>
    /// Frees all children under <paramref name="node"/>.
    /// </summary>
    /// <param name="node">Node of which children to free.</param>
    public static void FreeChildren(this Node node) {
        var count = node.GetChildCount();
        for (int i = count - 1; i >= 0; i--) {
            node.GetChild(i).Free();
        }
    }

    /// <summary>
    /// Queue frees all children under <paramref name="node"/>.
    /// </summary>
    /// <param name="node">Node of which children to queue free.</param>
    public static void QueueFreeChildren(this Node node) {
        var count = node.GetChildCount();
        for (int i = count - 1; i >= 0; i--) {
            node.GetChild(i).QueueFree();
        }
    }
}
