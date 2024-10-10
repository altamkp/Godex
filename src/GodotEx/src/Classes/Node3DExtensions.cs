using Godot;

namespace GodotEx;

/// <summary>
/// Extensions for <see cref="Node3D"/>.
/// </summary>
public static class Node3DExtensions {
    /// <summary>
    /// Calculates the distance from <paramref name="node"/> to <paramref name="target"/>.
    /// </summary>
    /// <param name="node">Starting node.</param>
    /// <param name="target">Target to use.</param>
    /// <returns>Distance from <paramref name="node"/> to <paramref name="target"/>.</returns>
    public static float DistanceTo(this Node3D node, Node3D target) {
        return node.GlobalPosition.DistanceTo(target.GlobalPosition);
    }

    /// <summary>
    /// Calculates the distance from <paramref name="node"/> to <paramref name="target"/>.
    /// </summary>
    /// <param name="node">Starting node.</param>
    /// <param name="target">Target to use.</param>
    /// <returns>Distance from <paramref name="node"/> to <paramref name="target"/>.</returns>
    public static float DistanceTo(this Node3D node, Vector3 target) {
        return node.GlobalPosition.DistanceTo(target);
    }

    /// <summary>
    /// Returns the unit vector pointing to the right relative to the node in local space.
    /// </summary>
    /// <param name="node">Node to use.</param>
    /// <returns>Unit vector pointing to the right relative to the node in local space.</returns>
    public static Vector3 Right(this Node3D node) => node.Transform.Right();

    /// <summary>
    /// Returns the unit vector pointing to the left relative to the node in local space.
    /// </summary>
    /// <param name="node">Node to use.</param>
    /// <returns>Unit vector pointing to the left relative to the node in local space.</returns>
    public static Vector3 Left(this Node3D node) => node.Transform.Left();

    /// <summary>
    /// Returns the unit vector pointing up relative to the node in local space.
    /// </summary>
    /// <param name="node">Node to use.</param>
    /// <returns>Unit vector pointing up relative to the node in local space.</returns>
    public static Vector3 Up(this Node3D node) => node.Transform.Up();

    /// <summary>
    /// Returns the unit vector pointing down relative to the node in local space.
    /// </summary>
    /// <param name="node">Node to use.</param>
    /// <returns>Unit vector pointing down relative to the node in local space.</returns>
    public static Vector3 Down(this Node3D node) => node.Transform.Down();

    /// <summary>
    /// Returns the unit vector pointing forward relative to the node in local space.
    /// </summary>
    /// <param name="node">Node to use.</param>
    /// <returns>Unit vector pointing forward relative to the node in local space.</returns>
    public static Vector3 Forward(this Node3D node) => node.Transform.Forward();

    /// <summary>
    /// Returns the unit vector pointing back relative to the node in local space.
    /// </summary>
    /// <param name="node">Node to use.</param>
    /// <returns>Unit vector pointing back relative to the node in local space.</returns>
    public static Vector3 Back(this Node3D node) => node.Transform.Back();

    /// <summary>
    /// Returns the unit vector pointing to the right relative to the node in global space.
    /// </summary>
    /// <param name="node">Node to use.</param>
    /// <returns>Unit vector pointing to the right relative to the node in global space.</returns>
    public static Vector3 GlobalRight(this Node3D node) => node.GlobalTransform.Right();

    /// <summary>
    /// Returns the unit vector pointing to the left relative to the node in global space.
    /// </summary>
    /// <param name="node">Node to use.</param>
    /// <returns>Unit vector pointing to the left relative to the node in global space.</returns>
    public static Vector3 GlobalLeft(this Node3D node) => node.GlobalTransform.Left();

    /// <summary>
    /// Returns the unit vector pointing up relative to the node in global space.
    /// </summary>
    /// <param name="node">Node to use.</param>
    /// <returns>Unit vector pointing up relative to the node in global space.</returns>
    public static Vector3 GlobalUp(this Node3D node) => node.GlobalTransform.Up();

    /// <summary>
    /// Returns the unit vector pointing down relative to the node in global space.
    /// </summary>
    /// <param name="node">Node to use.</param>
    /// <returns>Unit vector pointing down relative to the node in global space.</returns>
    public static Vector3 GlobalDown(this Node3D node) => node.GlobalTransform.Down();

    /// <summary>
    /// Returns the unit vector pointing forward relative to the node in global space.
    /// </summary>
    /// <param name="node">Node to use.</param>
    /// <returns>Unit vector pointing forward relative to the node in global space.</returns>
    public static Vector3 GlobalForward(this Node3D node) => node.GlobalTransform.Forward();

    /// <summary>
    /// Returns the unit vector pointing back relative to the node in global space.
    /// </summary>
    /// <param name="node">Node to use.</param>
    /// <returns>Unit vector pointing back relative to the node in global space.</returns>
    public static Vector3 GlobalBack(this Node3D node) => node.GlobalTransform.Back();
}
