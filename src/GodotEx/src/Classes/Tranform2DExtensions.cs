using Godot;

namespace GodotEx;

/// <summary>
/// Extensions for <see cref="Transform2D"/>.
/// </summary>
public static class Transform2DExtensions {
    /// <summary>
    /// Returns the unit vector pointing to the right relative to the transform object.
    /// </summary>
    /// <param name="transform">Transform to use.</param>
    /// <returns>Unit vector pointing to the right.</returns>
    public static Vector2 Right(this Transform2D transform) => transform.X;

    /// <summary>
    /// Returns the unit vector pointing to the left relative to the transform object.
    /// </summary>
    /// <param name="transform">Transform to use.</param>
    /// <returns>Unit vector pointing to the left.</returns>
    public static Vector2 Left(this Transform2D transform) => -transform.X;

    /// <summary>
    /// Returns the unit vector pointing up relative to the transform object.
    /// </summary>
    /// <param name="transform">Transform to use.</param>
    /// <returns>Unit vector pointing up.</returns>
    public static Vector2 Up(this Transform2D transform) => -transform.Y;

    /// <summary>
    /// Returns the unit vector pointing down relative to the transform object.
    /// </summary>
    /// <param name="transform">Transform to use.</param>
    /// <returns>Unit vector pointing down.</returns>
    public static Vector2 Down(this Transform2D transform) => transform.Y;
}
