namespace Godex;

/// <summary>
/// Extensions for <see cref="float"/>.
/// </summary>
public static class FloatExtensions {
    /// <summary>
    /// Rounds <paramref name="value"/> to the nearest multiple of <paramref name="precision"/>.
    /// </summary>
    /// <param name="value">Value to trim.</param>
    /// <param name="precision">Precision to use.</param>
    /// <returns>Trimmed value.</returns>
    public static float Trim(this float value, float precision = MathfDef.DEFAULT_PRECISION) {
        return MathF.Round(value / precision) * precision;
    }

    /// <summary>
    /// Checks if the two values are approximately equal, using <see cref="MathfDef.DEFAULT_PRECISION"/>.
    /// </summary>
    /// <param name="a">First value.</param>
    /// <param name="b">Second value.</param>
    /// <returns>True if the two values are approximately equal.</returns>
    public static bool IsEqualApprox(this float a, float b) => a.IsEqualApprox(b, MathfDef.DEFAULT_PRECISION);

    /// <summary>
    /// Checks if the two values are approximately equal.
    /// </summary>
    /// <param name="a">First value.</param>
    /// <param name="b">Second value.</param>
    /// <param name="precision">Precision to use.</param>
    /// <returns>True if the two values are approximately equal.</returns>
    public static bool IsEqualApprox(this float a, float b, float precision) => MathF.Abs(a - b) <= precision;

    /// <summary>
    /// Checks if the value is approximately zero.
    /// </summary>
    /// <param name="value">Value to check.</param>
    /// <param name="precision">Precision to use.</param>
    /// <returns>True if the value is approximately zero.</returns>
    public static bool IsZeroApprox(this float value, float precision) => MathF.Abs(value) <= precision;
}
