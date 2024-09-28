using DotEx.Maths;
using Godot;

namespace GodotEx;

/// <summary>
/// Extensions for <see cref="Vector3"/>.
/// </summary>
public static class Vector3Extensions {
    /// <summary>
    /// Sets <paramref name="vector"/>.X to <paramref name="x"/>.
    /// </summary>
    /// <param name="vector">Vector to modify.</param>
    /// <param name="x">Value to apply.</param>
    /// <returns>Modified vector.</returns>
    public static Vector3 SetX(this Vector3 vector, float x) => new(x, vector.Y, vector.Z);

    /// <summary>
    /// Sets <paramref name="vector"/>.Y to <paramref name="y"/>.
    /// </summary>
    /// <param name="vector">Vector to modify.</param>
    /// <param name="y">Value to apply.</param>
    /// <returns>Modified vector.</returns>
    public static Vector3 SetY(this Vector3 vector, float y) => new(vector.X, y, vector.Z);

    /// <summary>
    /// Sets <paramref name="vector"/>.Z to <paramref name="z"/>.
    /// </summary>
    /// <param name="vector">Vector to modify.</param>
    /// <param name="z">Value to apply.</param>
    /// <returns>Modified vector.</returns>
    public static Vector3 SetZ(this Vector3 vector, float z) => new(vector.X, vector.Y, z);

    /// <summary>
    /// Adds <paramref name="vector"/>.X by <paramref name="x"/>.
    /// </summary>
    /// <param name="vector">Vector to modify.</param>
    /// <param name="x">Value to add.</param>
    /// <returns>Modified vector.</returns>
    public static Vector3 AddX(this Vector3 vector, float x) => vector + new Vector3(x, 0, 0);

    /// <summary>
    /// Adds <paramref name="vector"/>.Y by <paramref name="y"/>.
    /// </summary>
    /// <param name="vector">Vector to modify.</param>
    /// <param name="y">Value to add.</param>
    /// <returns>Modified vector.</returns>
    public static Vector3 AddY(this Vector3 vector, float y) => vector + new Vector3(0, y, 0);

    /// <summary>
    /// Adds <paramref name="vector"/>.Z by <paramref name="z"/>.
    /// </summary>
    /// <param name="vector">Vector to modify.</param>
    /// <param name="z">Value to add.</param>
    /// <returns>Modified vector.</returns>
    public static Vector3 AddZ(this Vector3 vector, float z) => vector + new Vector3(0, 0, z);

    /// <summary>
    /// Scales <paramref name="vector"/>.X by <paramref name="x"/>.
    /// </summary>
    /// <param name="vector">Vector to modify.</param>
    /// <param name="x">Value to scale by.</param>
    /// <returns>Modified vector.</returns>
    public static Vector3 ScaleX(this Vector3 vector, float x) => new(vector.X * x, vector.Y, vector.Z);

    /// <summary>
    /// Scales <paramref name="vector"/>.Y by <paramref name="y"/>.
    /// </summary>
    /// <param name="vector">Vector to modify.</param>
    /// <param name="y">Value to scale by.</param>
    /// <returns>Modified vector.</returns>
    public static Vector3 ScaleY(this Vector3 vector, float y) => new(vector.X, vector.Y * y, vector.Z);

    /// <summary>
    /// Scales <paramref name="vector"/>.Z by <paramref name="z"/>.
    /// </summary>
    /// <param name="vector">Vector to modify.</param>
    /// <param name="z">Value to scale by.</param>
    /// <returns>Modified vector.</returns>
    public static Vector3 ScaleZ(this Vector3 vector, float z) => new(vector.X, vector.Y, vector.Z * z);

    /// <summary>
    /// Returns the vector's maximum axis and its length.
    /// </summary>
    /// <param name="vector">Vector to use.</param>
    /// <returns>Maximum axis and its length.</returns>
    public static (Vector3.Axis Axis, float Length) Max(this Vector3 vector) {
        var axis = vector.MaxAxisIndex();
        return (axis, vector[(int)axis]);
    }

    /// <summary>
    /// Returns the vector's minimum axis and its length.
    /// </summary>
    /// <param name="vector">Vector to use.</param>
    /// <returns>Minimum axis and its length.</returns>
    public static (Vector3.Axis Axis, float Length) Min(this Vector3 vector) {
        var axis = vector.MinAxisIndex();
        return (axis, vector[(int)axis]);
    }

    /// <summary>
    /// Flips the vector, multiplying it by -1.
    /// </summary>
    /// <param name="vector">Vector to flip.</param>
    /// <returns>Flipped vector.</returns>
    public static Vector3 Flip(this Vector3 vector) => -vector;

    /// <summary>
    /// Trims vector to a given precision.
    /// </summary>
    /// <param name="vector">Vector to trim.</param>
    /// <param name="precision">Precision to use.</param>
    /// <returns>Trimmed vector.</returns>
    public static Vector3 Trim(this Vector3 vector, float precision = (float)MathDef.DEFAULT_PRECISION) {
        return new(vector.X.Trim(precision), vector.Y.Trim(precision), vector.Z.Trim(precision));
    }

    /// <summary>
    /// Converts angles expressed in degrees to radians.
    /// </summary>
    /// <param name="vector">Vector to convert.</param>
    /// <returns>The same angles expressed in radians.</returns>
    public static Vector3 RadToDeg(this Vector3 vector) => vector * Mathf.Pi / 180f;

    /// <summary>
    /// Converts angles expressed in radians to degrees.
    /// </summary>
    /// <param name="vector">Vector to convert.</param>
    /// <returns>The same angles expressed in degrees.</returns>
    public static Vector3 DegToRad(this Vector3 vector) => vector * 180f / Mathf.Pi;

    /// <summary>
    /// Checks if the two vectors are approximately equal.
    /// </summary>
    /// <param name="a">First vector.</param>
    /// <param name="b">Second vector.</param>
    /// <param name="precision">Precision to use.</param>
    /// <returns>True if the two vectors are approximately equal.</returns>
    public static bool IsEqualApprox(this Vector3 a, Vector3 b, float precision) {
        return a.X.IsEqualApprox(b.X, precision)
            && a.Y.IsEqualApprox(b.Y, precision)
            && a.Z.IsEqualApprox(b.Z, precision);
    }

    /// <summary>
    /// Checks if the vector is approximately zero.
    /// </summary>
    /// <param name="vector">Vector to check.</param>
    /// <param name="precision">Precision to use.</param>
    /// <returns>True if the vector is approximately zero.</returns>
    public static bool IsZeroApprox(this Vector3 vector, float precision) {
        return vector.X.IsZeroApprox(precision)
            && vector.Y.IsZeroApprox(precision)
            && vector.Z.IsZeroApprox(precision);
    }

    /// <summary>
    /// Calculates the distance from <paramref name="vector"/> to <paramref name="target"/>.
    /// </summary>
    /// <param name="vector">Starting position to use.</param>
    /// <param name="target">Target to use.</param>
    /// <returns>Distance from <paramref name="vector"/> to <paramref name="target"/>.</returns>
    public static float DistanceTo(this Vector3 vector, Node3D target) {
        return vector.DistanceTo(target.GlobalPosition);
    }
}
