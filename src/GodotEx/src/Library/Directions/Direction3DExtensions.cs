using Godot;

namespace GodotEx;

/// <summary>
/// Extensions for <see cref="Direction3D"/>.
/// </summary>
public static class Direction3DExtensions {
    /// <summary>
    /// Flips direction.
    /// </summary>
    /// <param name="direction">Direction to flip.</param>
    /// <returns>Flipped direction</returns>
    public static Direction3D Flip(this Direction3D direction) {
        return direction switch {
            Direction3D.Right => Direction3D.Left,
            Direction3D.Left => Direction3D.Right,
            Direction3D.Up => Direction3D.Down,
            Direction3D.Down => Direction3D.Up,
            Direction3D.Forward => Direction3D.Back,
            Direction3D.Back => Direction3D.Forward,
            _ => throw new ArgumentException($"Non-existing direction {direction}.")
        };
    }

    /// <summary>
    /// Converts direction to <see cref="Vector3"/>.
    /// </summary>
    /// <param name="direction">Direction to convert.</param>
    /// <returns>Result vector.</returns>
    public static Vector3 ToVector3(this Direction3D direction) {
        return direction switch {
            Direction3D.Right => Vector3.Right,
            Direction3D.Left => Vector3.Left,
            Direction3D.Up => Vector3.Up,
            Direction3D.Down => Vector3.Down,
            Direction3D.Forward => Vector3.Forward,
            Direction3D.Back => Vector3.Back,
            _ => throw new ArgumentException($"Non-existing 3D direction {direction}.")
        };
    }
}
