namespace Godex.Tests;

using GdUnit4;
using Godot;
using System.Collections.Generic;
using static GdUnit4.Assertions;

[TestSuite]
[RequireGodotRuntime]
public class InputManagerTest {
    private Viewport CreateViewport() => AddNode(new SubViewport(), true);

    private static InputEventKey KeyEvent(Key key, bool pressed = true, bool ctrl = false) => new InputEventKey {
        Keycode = key,
        Pressed = pressed,
        CtrlPressed = ctrl,
    };

    [TestCase]
    public void HandleInvokesAMatchingHandler() {
        InputManager manager = new InputManager(CreateViewport());
        List<Key> seen = new List<Key>();
        manager.AddKeyHandler(Key.Space, e => seen.Add(e.Keycode));

        bool handled = manager.Handle(KeyEvent(Key.Space));

        AssertBool(handled).IsTrue();
        AssertArray(seen).ContainsExactly(Key.Space);
    }

    [TestCase]
    public void HandleIgnoresANonMatchingEvent() {
        InputManager manager = new InputManager(CreateViewport());
        int calls = 0;
        manager.AddKeyHandler(Key.Space, _ => calls++);

        bool handled = manager.Handle(KeyEvent(Key.Enter));

        AssertBool(handled).IsFalse();
        AssertInt(calls).IsEqual(0);
    }

    [TestCase]
    public void AddKeyHandlerIgnoresKeyReleases() {
        InputManager manager = new InputManager(CreateViewport());
        int calls = 0;
        manager.AddKeyHandler(Key.Space, _ => calls++);

        manager.Handle(KeyEvent(Key.Space, false));

        AssertInt(calls).IsEqual(0);
    }

    [TestCase]
    public void AddKeyHandlerRequiresNoModifiers() {
        InputManager manager = new InputManager(CreateViewport());
        int calls = 0;
        manager.AddKeyHandler(Key.Space, _ => calls++);

        manager.Handle(KeyEvent(Key.Space, true, ctrl: true));

        AssertInt(calls).IsEqual(0);
    }

    [TestCase]
    public void AddKeyHandlerMatchesTheRequestedModifiers() {
        InputManager manager = new InputManager(CreateViewport());
        int calls = 0;
        manager.AddKeyHandler(Key.Space, KeyModifierMask.MaskCtrl, _ => calls++);

        manager.Handle(KeyEvent(Key.Space, true, ctrl: true));

        AssertInt(calls).IsEqual(1);
    }

    [TestCase]
    public void AddHandlerMatchesAnEventTemplate() {
        InputManager manager = new InputManager(CreateViewport());
        int calls = 0;
        manager.AddHandler(new InputEventKey { Keycode = Key.Enter }, _ => calls++);

        manager.Handle(KeyEvent(Key.Enter));

        AssertInt(calls).IsEqual(1);
    }

    [TestCase]
    public void AddHandlerCanMatchOnlyPressedOrReleased() {
        InputManager manager = new InputManager(CreateViewport());
        int presses = 0;
        manager.AddHandler(new InputEventKey { Keycode = Key.Enter }, true, false, _ => presses++);

        manager.Handle(KeyEvent(Key.Enter, true));
        manager.Handle(KeyEvent(Key.Enter, false));

        AssertInt(presses).IsEqual(1);
    }

    [TestCase]
    public void OnlyTheFirstMatchingHandlerRuns() {
        InputManager manager = new InputManager(CreateViewport());
        int first = 0;
        int second = 0;
        manager.AddKeyHandler(Key.Space, _ => first++);
        manager.AddKeyHandler(Key.Space, _ => second++);

        manager.Handle(KeyEvent(Key.Space));

        AssertInt(first).IsEqual(1);
        AssertInt(second).IsEqual(0);
    }

    [TestCase]
    public void PassLetsTheEventReachTheNextHandler() {
        InputManager manager = new InputManager(CreateViewport());
        int first = 0;
        int second = 0;
        manager.AddKeyHandler(Key.Space, _ => first++, pass: true);
        manager.AddKeyHandler(Key.Space, _ => second++);

        bool handled = manager.Handle(KeyEvent(Key.Space));

        // Both ran, but because the first one passed, the event was not consumed.
        AssertInt(first).IsEqual(1);
        AssertInt(second).IsEqual(1);
        AssertBool(handled).IsTrue();
    }

    [TestCase]
    public void MouseMotionNeedsItsOwnHandler() {
        InputManager manager = new InputManager(CreateViewport());

        AssertThrown(() => manager.AddHandler<InputEventMouseMotion>(_ => true, _ => { }))
            .HasMessage("Use AddMouseMotionInputHandler instead.");
    }

    [TestCase]
    public void MouseMotionHandlerReceivesMotionEvents() {
        InputManager manager = new InputManager(CreateViewport());
        List<Vector2> seen = new List<Vector2>();
        manager.AddMouseMotionHandler(e => seen.Add(e.Position));

        manager.Handle(new InputEventMouseMotion { Position = new Vector2(3f, 4f) });

        AssertArray(seen).HasSize(1);
        AssertVector(seen[0]).IsEqual(new Vector2(3f, 4f));
    }

    [TestCase]
    public void PredicateSelectsWhichEventsAreHandled() {
        InputManager manager = new InputManager(CreateViewport());
        List<Key> seen = new List<Key>();
        manager.AddHandler<InputEventKey>(e => e.Keycode == Key.Space, e => seen.Add(e.Keycode));

        manager.Handle(KeyEvent(Key.Space));
        manager.Handle(KeyEvent(Key.Enter));

        AssertArray(seen).ContainsExactly(Key.Space);
    }
}