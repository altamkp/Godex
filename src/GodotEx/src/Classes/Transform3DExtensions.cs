using Godot;

namespace GodotEx;

/// <summary>
/// Extensions for <see cref="Transform3D"/>.
/// </summary>
public static class Transform3DExtensions {
    /// <summary>
    /// Returns the unit vector pointing to the right relative to the transform object.
    /// </summary>
    /// <param name="transform">Transform to use.</param>
    /// <returns>Unit vector pointing to the right.</returns>
    public static Vector3 Right(this Transform3D transform) => transform.Basis.X;

    /// <summary>
    /// Returns the unit vector pointing to the left relative to the transform object.
    /// </summary>
    /// <param name="transform">Transform to use.</param>
    /// <returns>Unit vector pointing to the left.</returns>
    public static Vector3 Left(this Transform3D transform) => -transform.Basis.X;

    /// <summary>
    /// Returns the unit vector pointing up relative to the transform object.
    /// </summary>
    /// <param name="transform">Transform to use.</param>
    /// <returns>Unit vector pointing up.</returns>
    public static Vector3 Up(this Transform3D transform) => transform.Basis.Y;

    /// <summary>
    /// Returns the unit vector pointing down relative to the transform object.
    /// </summary>
    /// <param name="transform">Transform to use.</param>
    /// <returns>Unit vector pointing down.</returns>
    public static Vector3 Down(this Transform3D transform) => -transform.Basis.Y;

    /// <summary>
    /// Returns the unit vector pointing forward relative to the transform object.
    /// </summary>
    /// <param name="transform">Transform to use.</param>
    /// <returns>Unit vector pointing forward.</returns>
    public static Vector3 Forward(this Transform3D transform) => -transform.Basis.Z;

    /// <summary>
    /// Returns the unit vector pointing back relative to the transform object.
    /// </summary>
    /// <param name="transform">Transform to use.</param>
    /// <returns>Unit vector pointing back.</returns>
    public static Vector3 Back(this Transform3D transform) => transform.Basis.Z;
}
