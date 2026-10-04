namespace Godex.Tests;

using GdUnit4;
using Godot;
using static GdUnit4.Assertions;

[TestSuite]
[RequireGodotRuntime]
public class InputEventExtensionsTest {
    private static InputEventKey KeyEvent(Key key, bool pressed, KeyModifierMask mask = 0) {
        InputEventKey @event = new() {
            Keycode = key,
            Pressed = pressed,
        };
        if (mask.HasFlag(KeyModifierMask.MaskCtrl)) {
            @event.CtrlPressed = true;
        }
        if (mask.HasFlag(KeyModifierMask.MaskShift)) {
            @event.ShiftPressed = true;
        }
        if (mask.HasFlag(KeyModifierMask.MaskAlt)) {
            @event.AltPressed = true;
        }
        return @event;
    }

    [TestCase]
    public void IsKeyPressedMatchesTheKeyAndModifiers() {
        AssertBool(KeyEvent(Key.Space, true).IsKeyPressed(Key.Space)).IsTrue();
        AssertBool(KeyEvent(Key.Space, true, KeyModifierMask.MaskCtrl).IsKeyPressed(Key.Space, KeyModifierMask.MaskCtrl)).IsTrue();
    }

    [TestCase]
    public void IsKeyPressedRejectsOtherKeysAndModifiers() {
        AssertBool(KeyEvent(Key.Space, true).IsKeyPressed(Key.Enter)).IsFalse();
        AssertBool(KeyEvent(Key.Space, true, KeyModifierMask.MaskCtrl).IsKeyPressed(Key.Space)).IsFalse();
        AssertBool(KeyEvent(Key.Space, true).IsKeyPressed(Key.Space, KeyModifierMask.MaskShift)).IsFalse();
    }

    [TestCase]
    public void IsKeyPressedRejectsReleases() {
        AssertBool(KeyEvent(Key.Space, false).IsKeyPressed(Key.Space)).IsFalse();
    }

    [TestCase]
    public void IsKeyReleasedIsTheInverseOfIsKeyPressed() {
        AssertBool(KeyEvent(Key.Space, false).IsKeyReleased(Key.Space)).IsTrue();
        AssertBool(KeyEvent(Key.Space, true).IsKeyReleased(Key.Space)).IsFalse();
    }

    [TestCase]
    public void KeyChecksIgnoreOtherEventTypes() {
        AssertBool(new InputEventMouseButton().IsKeyPressed(Key.Space)).IsFalse();
    }

    [TestCase]
    public void IsMousePressedMatchesTheButton() {
        InputEventMouseButton @event = new() {
            ButtonIndex = MouseButton.Left,
            Pressed = true,
        };

        AssertBool(@event.IsMousePressed(MouseButton.Left)).IsTrue();
        AssertBool(@event.IsMousePressed(MouseButton.Right)).IsFalse();
        AssertBool(@event.IsMouseReleased(MouseButton.Left)).IsFalse();
    }

    [TestCase]
    public void IsMouseReleasedMatchesTheButton() {
        InputEventMouseButton @event = new() {
            ButtonIndex = MouseButton.Right,
            Pressed = false,
        };

        AssertBool(@event.IsMouseReleased(MouseButton.Right)).IsTrue();
        AssertBool(@event.IsMousePressed(MouseButton.Right)).IsFalse();
    }
}