using Godot;
using System.Reflection;

namespace GodotEx;

public static partial class NodeExtensions {
    private static readonly Dictionary<Type, GroupAttribute?> GROUP_INFOS = new();

    /// <summary>
    /// Adds <paramref name="node"/> to group specified by <see cref="GroupAttribute"/>.
    /// </summary>
    /// <param name="node">Node to resolve.</param>
    public static void ResolveGroup(this Node node) {
        var type = node.GetType();
        if (!GROUP_INFOS.TryGetValue(node.GetType(), out var group)) {
            group = type.GetCustomAttribute<GroupAttribute>();
            GROUP_INFOS.Add(type, group);
        }

        if (group != null) {
            node.AddToGroup(group.Name);
        }
    }
}
