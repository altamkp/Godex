using Godot;

namespace GodotEx;

/// <summary>
/// Extensions for <see cref="Node2D"/>.
/// </summary>
public static class Node2DExtensions {
    /// <summary>
    /// Calculates the distance from <paramref name="node"/> to <paramref name="target"/>.
    /// </summary>
    /// <param name="node">Starting node.</param>
    /// <param name="target">Target to use.</param>
    /// <returns>Distance from <paramref name="node"/> to <paramref name="target"/>.</returns>
    public static float DistanceTo(this Node2D node, Node2D target) {
        return node.GlobalPosition.DistanceTo(target.GlobalPosition);
    }

    /// <summary>
    /// Calculates the distance from <paramref name="node"/> to <paramref name="target"/>.
    /// </summary>
    /// <param name="node">Starting node.</param>
    /// <param name="target">Target to use.</param>
    /// <returns>Distance from <paramref name="node"/> to <paramref name="target"/>.</returns>
    public static float DistanceTo(this Node2D node, Vector2 target) {
        return node.GlobalPosition.DistanceTo(target);
    }

    /// <summary>
    /// Returns the unit vector pointing to the right relative to the node in local space.
    /// </summary>
    /// <param name="node">Node to use.</param>
    /// <returns>Unit vector pointing to the right relative to the node in local space.</returns>
    public static Vector2 Right(this Node2D node) => node.Transform.Right();

    /// <summary>
    /// Returns the unit vector pointing to the left relative to the node in local space.
    /// </summary>
    /// <param name="node">Node to use.</param>
    /// <returns>Unit vector pointing to the left relative to the node in local space.</returns>
    public static Vector2 Left(this Node2D node) => node.Transform.Left();

    /// <summary>
    /// Returns the unit vector pointing up relative to the node in local space.
    /// </summary>
    /// <param name="node">Node to use.</param>
    /// <returns>Unit vector pointing up relative to the node in local space.</returns>
    public static Vector2 Up(this Node2D node) => node.Transform.Up();

    /// <summary>
    /// Returns the unit vector pointing down relative to the node in local space.
    /// </summary>
    /// <param name="node">Node to use.</param>
    /// <returns>Unit vector pointing down relative to the node in local space.</returns>
    public static Vector2 Down(this Node2D node) => node.Transform.Down();

    /// <summary>
    /// Returns the unit vector pointing to the right relative to the node in global space.
    /// </summary>
    /// <param name="node">Node to use.</param>
    /// <returns>Unit vector pointing to the right relative to the node in global space.</returns>
    public static Vector2 GlobalRight(this Node2D node) => node.GlobalTransform.Right();

    /// <summary>
    /// Returns the unit vector pointing to the left relative to the node in global space.
    /// </summary>
    /// <param name="node">Node to use.</param>
    /// <returns>Unit vector pointing to the left relative to the node in global space.</returns>
    public static Vector2 GlobalLeft(this Node2D node) => node.GlobalTransform.Left();

    /// <summary>
    /// Returns the unit vector pointing up relative to the node in global space.
    /// </summary>
    /// <param name="node">Node to use.</param>
    /// <returns>Unit vector pointing up relative to the node in global space.</returns>
    public static Vector2 GlobalUp(this Node2D node) => node.GlobalTransform.Up();

    /// <summary>
    /// Returns the unit vector pointing down relative to the node in global space.
    /// </summary>
    /// <param name="node">Node to use.</param>
    /// <returns>Unit vector pointing down relative to the node in global space.</returns>
    public static Vector2 GlobalDown(this Node2D node) => node.GlobalTransform.Down();
}
