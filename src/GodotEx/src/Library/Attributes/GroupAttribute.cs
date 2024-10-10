namespace GodotEx;

/// <summary>
/// Attribute that is recognized by <see cref="NodeExtensions.ResolveGroup(Godot.Node)"/> to set
/// the target node's layer property to the provided flags.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class GroupAttribute : Attribute {
    /// <summary>
    /// Returns a new <see cref="GroupAttribute"/> which is used by <see cref="NodeExtensions.ResolveGroup(Godot.Node)"/>
    /// to set the target node's group.
    /// </summary>
    /// <param name="name">Name of the group.</param>
    public GroupAttribute(string name) {
        Name = name;
    }

    internal string Name { get; }
}
