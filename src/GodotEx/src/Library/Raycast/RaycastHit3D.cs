using Godot;

namespace GodotEx;

/// <summary>
/// A struct that encapsulates raycast result.
/// </summary>
public readonly struct RaycastHit3D {
    internal RaycastHit3D(Vector3 position, Vector3 normal, Node3D collider, string colliderId, Rid rid, int shape) {
        Position = position;
        Normal = normal;
        Collider = collider;
        ColliderId = colliderId;
        Rid = rid;
        Shape = shape;
    }

    /// <summary>
    /// The intersection point.
    /// </summary>
    public Vector3 Position { get; }

    /// <summary>
    /// The object's surface normal at the intersection point, or
    /// <see cref="Vector3.Zero"/> if the  ray starts inside the shape and 
    /// <see cref="PhysicsRayQueryParameters3D.HitFromInside"/> is true.
    /// </summary>
    public Vector3 Normal { get; }

    /// <summary>
    /// The colliding object.
    /// </summary>
    public Node3D Collider { get; }

    /// <summary>
    /// The colliding object's ID.
    /// </summary>
    public string ColliderId { get; }

    /// <summary>
    /// The intersecting object's RID.
    /// </summary>
    public Rid Rid { get; }

    /// <summary>
    /// The shape index of the colliding shape.
    /// </summary>
    public int Shape { get; }
}
