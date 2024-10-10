using Godot;

namespace GodotEx;

/// <summary>
/// Extensions for <see cref="Vector3I"/>.
/// </summary>
public static class Vector3IExtensions {
    /// <summary>
    /// Sets <paramref name="vector"/>.X to <paramref name="x"/>.
    /// </summary>
    /// <param name="vector">Vector to modify.</param>
    /// <param name="x">Value to apply.</param>
    /// <returns>Modified vector.</returns>
    public static Vector3I SetX(this Vector3I vector, int x) => new(x, vector.Y, vector.Z);

    /// <summary>
    /// Sets <paramref name="vector"/>.Y to <paramref name="y"/>.
    /// </summary>
    /// <param name="vector">Vector to modify.</param>
    /// <param name="y">Value to apply.</param>
    /// <returns>Modified vector.</returns>
    public static Vector3I SetY(this Vector3I vector, int y) => new(vector.X, y, vector.Z);

    /// <summary>
    /// Sets <paramref name="vector"/>.Z to <paramref name="z"/>.
    /// </summary>
    /// <param name="vector">Vector to modify.</param>
    /// <param name="z">Value to apply.</param>
    /// <returns>Modified vector.</returns>
    public static Vector3I SetZ(this Vector3I vector, int z) => new(vector.X, vector.Y, z);

    /// <summary>
    /// Adds <paramref name="vector"/>.X by <paramref name="x"/>.
    /// </summary>
    /// <param name="vector">Vector to modify.</param>
    /// <param name="x">Value to add.</param>
    /// <returns>Modified vector.</returns>
    public static Vector3I AddX(this Vector3I vector, int x) => vector + new Vector3I(x, 0, 0);

    /// <summary>
    /// Adds <paramref name="vector"/>.Y by <paramref name="y"/>.
    /// </summary>
    /// <param name="vector">Vector to modify.</param>
    /// <param name="y">Value to add.</param>
    /// <returns>Modified vector.</returns>
    public static Vector3I AddY(this Vector3I vector, int y) => vector + new Vector3I(0, y, 0);

    /// <summary>
    /// Adds <paramref name="vector"/>.Z by <paramref name="z"/>.
    /// </summary>
    /// <param name="vector">Vector to modify.</param>
    /// <param name="z">Value to add.</param>
    /// <returns>Modified vector.</returns>
    public static Vector3I AddZ(this Vector3I vector, int z) => vector + new Vector3I(0, 0, z);

    /// <summary>
    /// Scales <paramref name="vector"/>.X by <paramref name="x"/>.
    /// </summary>
    /// <param name="vector">Vector to modify.</param>
    /// <param name="x">Value to scale by.</param>
    /// <returns>Modified vector.</returns>
    public static Vector3I ScaleX(this Vector3I vector, float x) => new(Mathf.RoundToInt(vector.X * x), vector.Y, vector.Z);

    /// <summary>
    /// Scales <paramref name="vector"/>.Y by <paramref name="y"/>.
    /// </summary>
    /// <param name="vector">Vector to modify.</param>
    /// <param name="y">Value to scale by.</param>
    /// <returns>Modified vector.</returns>
    public static Vector3I ScaleY(this Vector3I vector, float y) => new(vector.X, Mathf.RoundToInt(vector.Y * y), vector.Z);

    /// <summary>
    /// Scales <paramref name="vector"/>.Z by <paramref name="z"/>.
    /// </summary>
    /// <param name="vector">Vector to modify.</param>
    /// <param name="z">Value to scale by.</param>
    /// <returns>Modified vector.</returns>
    public static Vector3I ScaleZ(this Vector3I vector, float z) => new(vector.X, vector.Y, Mathf.RoundToInt(vector.Z * z));

    /// <summary>
    /// Returns the vector's maximum axis and its length.
    /// </summary>
    /// <param name="vector">Vector to use.</param>
    /// <returns>Maximum axis and its length.</returns>
    public static (Vector3I.Axis Axis, float Length) Max(this Vector3I vector) {
        var axis = vector.MaxAxisIndex();
        return (axis, vector[(int)axis]);
    }

    /// <summary>
    /// Returns the vector's minimum axis and its length.
    /// </summary>
    /// <param name="vector">Vector to use.</param>
    /// <returns>Minimum axis and its length.</returns>
    public static (Vector3I.Axis Axis, float Length) Min(this Vector3I vector) {
        var axis = vector.MinAxisIndex();
        return (axis, vector[(int)axis]);
    }

    /// <summary>
    /// Flips the vector, multiplying it by -1.
    /// </summary>
    /// <param name="vector">Vector to flip.</param>
    /// <returns>Flipped vector.</returns>
    public static Vector3I Flip(this Vector3I vector) => -vector;
}
