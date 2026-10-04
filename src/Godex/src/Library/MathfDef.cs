using Godot;

namespace Godex;

/// <summary>
/// Additional <see cref="Mathf"/> related definitions.
/// </summary>
public static class MathfDef {
    /// <summary>
    /// Half Pi, equivalent to 90 degrees.
    /// </summary>
    public const float HALF_PI = Mathf.Pi / 2;

    /// <summary>
    /// Default tolerance used by <see cref="FloatExtensions.IsEqualApprox(float, float)"/>,
    /// <see cref="FloatExtensions.IsEqualApprox(float, float, float)"/>,
    /// <see cref="FloatExtensions.IsZeroApprox(float, float)"/> and
    /// <see cref="FloatExtensions.Trim(float, float)"/>.
    /// </summary>
    public const float DEFAULT_PRECISION = 0.0001f;
}
