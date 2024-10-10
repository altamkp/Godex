using Godot;

namespace GodotEx;

/// <summary>
/// Extensions for <see cref="Vector2I"/>.
/// </summary>
public static class Vector2IExtensions {
    /// <summary>
    /// Sets <paramref name="vector"/>.X to <paramref name="x"/>.
    /// </summary>
    /// <param name="vector">Vector to modify.</param>
    /// <param name="x">Value to apply.</param>
    /// <returns>Modified vector.</returns>
    public static Vector2I SetX(this Vector2I vector, int x) => new(x, vector.Y);

    /// <summary>
    /// Sets <paramref name="vector"/>.Y to <paramref name="y"/>.
    /// </summary>
    /// <param name="vector">Vector to modify.</param>
    /// <param name="y">Value to apply.</param>
    /// <returns>Modified vector.</returns>
    public static Vector2I SetY(this Vector2I vector, int y) => new(vector.X, y);

    /// <summary>
    /// Adds <paramref name="vector"/>.X by <paramref name="x"/>.
    /// </summary>
    /// <param name="vector">Vector to modify.</param>
    /// <param name="x">Value to add.</param>
    /// <returns>Modified vector.</returns>
    public static Vector2I AddX(this Vector2I vector, int x) => vector + new Vector2I(x, 0);

    /// <summary>
    /// Adds <paramref name="vector"/>.Y by <paramref name="y"/>.
    /// </summary>
    /// <param name="vector">Vector to modify.</param>
    /// <param name="y">Value to add.</param>
    /// <returns>Modified vector.</returns>
    public static Vector2I AddY(this Vector2I vector, int y) => vector + new Vector2I(0, y);

    /// <summary>
    /// Scales <paramref name="vector"/>.X by <paramref name="x"/>.
    /// </summary>
    /// <param name="vector">Vector to modify.</param>
    /// <param name="x">Value to scale by.</param>
    /// <returns>Modified vector.</returns>
    public static Vector2I ScaleX(this Vector2I vector, float x) => new(Mathf.RoundToInt(vector.X * x), vector.Y);

    /// <summary>
    /// Scales <paramref name="vector"/>.Y by <paramref name="y"/>.
    /// </summary>
    /// <param name="vector">Vector to modify.</param>
    /// <param name="y">Value to scale by.</param>
    /// <returns>Modified vector.</returns>
    public static Vector2I ScaleY(this Vector2I vector, float y) => new(vector.X, Mathf.RoundToInt(vector.Y * y));

    /// <summary>
    /// Returns the vector's maximum axis and its length.
    /// </summary>
    /// <param name="vector">Vector to use.</param>
    /// <returns>Maximum axis and its length.</returns>
    public static (Vector2I.Axis Axis, float Length) Max(this Vector2I vector) {
        var axis = vector.MaxAxisIndex();
        return (axis, vector[(int)axis]);
    }

    /// <summary>
    /// Returns the vector's minimum axis and its length.
    /// </summary>
    /// <param name="vector">Vector to use.</param>
    /// <returns>Minimum axis and its length.</returns>
    public static (Vector2I.Axis Axis, float Length) Min(this Vector2I vector) {
        var axis = vector.MinAxisIndex();
        return (axis, vector[(int)axis]);
    }

    /// <summary>
    /// Flips the vector, multiplying it by -1.
    /// </summary>
    /// <param name="vector">Vector to flip.</param>
    /// <returns>Flipped vector.</returns>
    public static Vector2I Flip(this Vector2I vector) => -vector;
}
