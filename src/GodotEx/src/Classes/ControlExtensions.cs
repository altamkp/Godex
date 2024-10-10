using Godot;

namespace GodotEx;

/// <summary>
/// Extensions for <see cref="Control"/>.
/// </summary>
public static class ControlExtensions {
    /// <summary>
    /// Calculates the distance from <paramref name="control"/> to <paramref name="target"/>.
    /// </summary>
    /// <param name="control">Starting control.</param>
    /// <param name="target">Target to use.</param>
    /// <returns>Distance from <paramref name="control"/> to <paramref name="target"/>.</returns>
    public static float DistanceTo(this Control control, Control target) {
        return control.GlobalPosition.DistanceTo(target.GlobalPosition);
    }

    /// <summary>
    /// Calculates the distance from <paramref name="control"/> to <paramref name="target"/>.
    /// </summary>
    /// <param name="control">Starting control.</param>
    /// <param name="target">Target to use.</param>
    /// <returns>Distance from <paramref name="control"/> to <paramref name="target"/>.</returns>
    public static float DistanceTo(this Control control, Vector2 target) {
        return control.GlobalPosition.DistanceTo(target);
    }
}
