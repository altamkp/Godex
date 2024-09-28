using Godot;

namespace GodotEx;

/// <summary>
/// Extensions for <see cref="InputEvent"/>.
/// </summary>
public static class InputEventExtensions {
    /// <summary>
    /// Checks if mouse <paramref name="button"/> is pressed.
    /// </summary>
    /// <param name="event">Input event to check.</param>
    /// <param name="button">Mouse button to compare to.</param>
    /// <returns>True if mouse <paramref name="button"/> is pressed.</returns>
    public static bool IsMousePressed(this InputEvent @event, MouseButton button) {
        return @event is InputEventMouseButton buttonEvent
            && buttonEvent.ButtonIndex == button && buttonEvent.IsPressed();
    }

    /// <summary>
    /// Checks if mouse <paramref name="button"/> is released.
    /// </summary>
    /// <param name="event">Input event to check.</param>
    /// <param name="button">Mouse button to compare to.</param>
    /// <returns>True if mouse <paramref name="button"/> is released.</returns>
    public static bool IsMouseReleased(this InputEvent @event, MouseButton button) {
        return @event is InputEventMouseButton buttonEvent
            && buttonEvent.ButtonIndex == button && !buttonEvent.IsPressed();
    }

    /// <summary>
    /// Checks if keyboard <paramref name="key"/> modified by <paramref name="mask"/> is pressed.
    /// </summary>
    /// <param name="event">Input event to check.</param>
    /// <param name="key">Keyboard key to compare to.</param>
    /// <param name="mask">Modifier masks to match, default to none.</param>
    /// <returns>True if keyboard <paramref name="key"/> is pressed.</returns>
    public static bool IsKeyPressed(this InputEvent @event, Key key, KeyModifierMask mask = 0) {
        return @event is InputEventKey keyEvent
            && keyEvent.Keycode == key && keyEvent.GetModifiersMask() == mask
            && keyEvent.IsPressed();
    }

    /// <summary>
    /// Checks if keyboard <paramref name="key"/> modified by <paramref name="mask"/> is released.
    /// </summary>
    /// <param name="event">Input event to check.</param>
    /// <param name="key">Keyboard key to compare to.</param>
    /// <param name="mask">Modifier masks to match, default to none.</param>
    /// <returns>True if keyboard <paramref name="key"/> is released.</returns>
    public static bool IsKeyReleased(this InputEvent @event, Key key, KeyModifierMask mask = 0) {
        return @event is InputEventKey keyEvent
            && keyEvent.Keycode == key && keyEvent.GetModifiersMask() == mask
            && !keyEvent.IsPressed();
    }
}
