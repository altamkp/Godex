namespace GodotEx;

/// <summary>
/// Attribute that is recognized by <see cref="NodeExtensions.ResolveBitFlags(Godot.Node)"/> to set
/// the target node's layer property to the provided flags.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
public class BitFlagsAttribute : Attribute {
    /// <summary>
    /// Returns a new <see cref="BitFlagsAttribute"/> which is used by <see cref="NodeExtensions.ResolveBitFlags(Godot.Node)"/>
    /// to set the target node's layer property to the provided flags. The node must have exactly one property
    /// with name containing "layer" and flags must be castable to <see cref="uint"/>.
    /// </summary>
    /// <param name="propertyName">Name of the property to set.</param>
    /// <param name="flags">Flags to apply</param>
    /// <exception cref="InvalidCastException"><paramref name="flags"/> is not of type uint.</exception>
    public BitFlagsAttribute(string propertyName, object flags) {
        try {
            PropertyName = propertyName;
            Flags = (uint)flags;
        } catch (Exception) {
            throw new InvalidCastException($"Failed to cast flags of type {flags.GetType().Name} to uint.");
        }
    }

    internal string PropertyName { get; }
    internal uint Flags { get; }
}
