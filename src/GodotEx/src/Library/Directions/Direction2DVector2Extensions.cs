using DotEx.Maths;
using Godot;

namespace GodotEx;

/// <summary>
/// Extensions for <see cref="Direction2D"/> related to <see cref="Vector3"/>.
/// </summary>
public static class Direction2DVector2Extensions {
    /// <summary>
    /// Converts <see cref="Vector2"/> to <see cref="Direction2D"/>.
    /// </summary>
    /// <param name="vector">Vector to convert.</param>
    /// <returns>Direction converted from the input vector.</returns>
    /// <exception cref="ArgumentException">Vector is not orthogonal to any directions.</exception>
    public static Direction2D ToDirection(this Vector2 vector) {
        if (vector.IsEqualApprox(Vector2.Left, (float)MathDef.DEFAULT_PRECISION)) {
            return Direction2D.Left;
        } else if (vector.IsEqualApprox(Vector2.Right, (float)MathDef.DEFAULT_PRECISION)) {
            return Direction2D.Right;
        } else if (vector.IsEqualApprox(Vector2.Up, (float)MathDef.DEFAULT_PRECISION)) {
            return Direction2D.Up;
        } else if (vector.IsEqualApprox(Vector2.Down, (float)MathDef.DEFAULT_PRECISION)) {
            return Direction2D.Down;
        } else {
            throw new ArgumentException($"Vector {vector} is not orthogonal to the any of the directions.");
        }
    }
}
