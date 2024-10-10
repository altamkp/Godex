using Godot;

namespace GodotEx;

/// <summary>
/// Extensions for <see cref="Color"/>.
/// </summary>
public static class ColorExtensions {
    /// <summary>
    /// Sets <paramref name="color"/>.R to <paramref name="value"/>.
    /// </summary>
    /// <param name="color">Color to modify.</param>
    /// <param name="value">Value to set.</param>
    /// <returns>Modified color.</returns>
    public static Color SetR(this Color color, float value) => new(value, color.G, color.B, color.A);

    /// <summary>
    /// Sets <paramref name="color"/>.G to <paramref name="value"/>.
    /// </summary>
    /// <param name="color">Color to modify.</param>
    /// <param name="value">Value to set.</param>
    /// <returns>Modified color.</returns>
    public static Color SetG(this Color color, float value) => new(color.R, value, color.B, color.A);

    /// <summary>
    /// Sets <paramref name="color"/>.B to <paramref name="value"/>.
    /// </summary>
    /// <param name="color">Color to modify.</param>
    /// <param name="value">Value to set.</param>
    /// <returns>Modified color.</returns>
    public static Color SetB(this Color color, float value) => new(color.R, color.G, value, color.A);

    /// <summary>
    /// Sets <paramref name="color"/>.A to <paramref name="value"/>.
    /// </summary>
    /// <param name="color">Color to modify.</param>
    /// <param name="value">Value to set.</param>
    /// <returns>Modified color.</returns>
    public static Color SetA(this Color color, float value) => new(color.R, color.G, color.B, value);

    /// <summary>
    /// Sets <paramref name="color"/>.H to <paramref name="value"/>.
    /// </summary>
    /// <param name="color">Color to modify.</param>
    /// <param name="value">Value to set.</param>
    /// <returns>Modified color.</returns>
    public static Color SetH(this Color color, float value) => Color.FromHsv(value, color.S, color.V);

    /// <summary>
    /// Sets <paramref name="color"/>.S to <paramref name="value"/>.
    /// </summary>
    /// <param name="color">Color to modify.</param>
    /// <param name="value">Value to set.</param>
    /// <returns>Modified color.</returns>
    public static Color SetS(this Color color, float value) => Color.FromHsv(color.H, value, color.V);

    /// <summary>
    /// Sets <paramref name="color"/>.V to <paramref name="value"/>.
    /// </summary>
    /// <param name="color">Color to modify.</param>
    /// <param name="value">Value to set.</param>
    /// <returns>Modified color.</returns>
    public static Color SetV(this Color color, float value) => Color.FromHsv(color.H, color.S, value);
}
