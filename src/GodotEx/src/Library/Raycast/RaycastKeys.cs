namespace GodotEx;

/// <summary>
/// Raycast dictionary keys as defined by Godot, see 
/// https://docs.godotengine.org/en/stable/classes/class_physicsdirectspacestate3d.html#:~:text=Dictionary%20intersect_ray%20(,is%20returned%20instead.
/// </summary>
internal static class RaycastKeys {
    public const string POSITION = "position";
    public const string NORMAL = "normal";
    public const string COLLIDER = "collider";
    public const string COLLIDER_ID = "collider_id";
    public const string RID = "rid";
    public const string SHAPE = "shape";
    public const string METADATA = "metadata";
}
