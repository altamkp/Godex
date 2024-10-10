using Godot;

namespace GodotEx;

/// <summary>
/// Extensions for <see cref="Direction2D"/>.
/// </summary>
public static class Direction2DExtensions {
    /// <summary>
    /// Flips direction.
    /// </summary>
    /// <param name="direction">Direction to flip.</param>
    /// <returns>Flipped direction</returns>
    public static Direction2D Flip(this Direction2D direction) {
        return direction switch {
            Direction2D.Right => Direction2D.Left,
            Direction2D.Left => Direction2D.Right,
            Direction2D.Up => Direction2D.Down,
            Direction2D.Down => Direction2D.Up,
            _ => throw new ArgumentException($"Non-existing direction {direction}.")
        };
    }

    /// <summary>
    /// Converts direction to <see cref="Vector2"/>.
    /// </summary>
    /// <param name="direction">Direction to convert.</param>
    /// <returns>Result vector.</returns>
    /// <exception cref="ArgumentException">Non-existing 2D direction.</exception>
    public static Vector2 ToVector2(this Direction2D direction) {
        return direction switch {
            Direction2D.Right => Vector2.Right,
            Direction2D.Left => Vector2.Left,
            Direction2D.Up => Vector2.Up,
            Direction2D.Down => Vector2.Down,
            _ => throw new ArgumentException($"Non-existing 2D direction {direction}.")
        };
    }
}
