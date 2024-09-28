using DotEx.Maths;
using Godot;

namespace GodotEx;

/// <summary>
/// Extensions for <see cref="Direction3D"/> related to <see cref="Vector3"/>.
/// </summary>
public static class DirectionVector3Extensions {
    /// <summary>
    /// Converts <see cref="Vector3"/> to <see cref="Direction3D"/>.
    /// </summary>
    /// <param name="vector">Vector to convert.</param>
    /// <returns>Direction converted from the input vector.</returns>
    /// <exception cref="ArgumentException">Vector is not orthogonal to any directions.</exception>
    public static Direction3D ToDirection(this Vector3 vector) {
        if (vector.IsEqualApprox(Vector3.Forward, (float)MathDef.DEFAULT_PRECISION)) {
            return Direction3D.Forward;
        } else if (vector.IsEqualApprox(Vector3.Back, (float)MathDef.DEFAULT_PRECISION)) {
            return Direction3D.Back;
        } else if (vector.IsEqualApprox(Vector3.Left, (float)MathDef.DEFAULT_PRECISION)) {
            return Direction3D.Left;
        } else if (vector.IsEqualApprox(Vector3.Right, (float)MathDef.DEFAULT_PRECISION)) {
            return Direction3D.Right;
        } else if (vector.IsEqualApprox(Vector3.Up, (float)MathDef.DEFAULT_PRECISION)) {
            return Direction3D.Up;
        } else if (vector.IsEqualApprox(Vector3.Down, (float)MathDef.DEFAULT_PRECISION)) {
            return Direction3D.Down;
        } else {
            throw new ArgumentException($"Vector {vector} is not orthogonal to the any of the directions.");
        }
    }
}
