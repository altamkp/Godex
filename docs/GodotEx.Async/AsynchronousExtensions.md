# Asynchronous Extensions

GodotEx asynchronous extensions are a set of asynchronous extension methods for Godot built-in classes such as [SceneTree](https://docs.godotengine.org/en/stable/classes/class_scenetree.html), [Timer](https://docs.godotengine.org/en/stable/classes/class_timer.html) and [Tween](https://docs.godotengine.org/en/stable/classes/class_tween.html).They are available through the `GodotEx.Async` package.

Please refer to the [GodotEx.Async Api](~/api/GodotEx.Async.yml) for all available asynchronous extensions. Here are some examples:

1. Queue free a node asynchronously:

   Instead of writing

   ```csharp
    node.TreeExited += OnNodeExited;
    node.QueueFree();

    void OnNodeExited() {
        node.TreeExited -= OnNodeExited;
        DoSomething();
    }
   ```

   With the extension you can write

   ```csharp
    await node.QueueFreeAsync();
    DoSomething();
   ```

2. Use Tween properties asynchronously:

   Instead of writing

   ```csharp
    var tween = CreateTween();
    tween.TweenProperty(player, Node3D.PropertyName.GlobalPosition, Vector3.Zero, duration);
    tween.TweenCallback(Callable.From(DoSomething)).SetDelay(duration);
   ```

   With the extension you can write

   ```csharp
   var tween = CreateTween();
   await tween.TweenPropertyAsync(player, Node3D.PropertyName.GlobalPosition, Vector3.Zero, duration);
   DoSomething();
   ```

## Awaiting Signals

Godot allows awaiting object signals with [ToSignal()](https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_signals.html#signals-as-c-events). Some common uses include:

```csharp
await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
await ToSignal(timer, Timer.SignalName.Timeout);
```

With the extensions, you can write:

```csharp
await GetTree().ProcessFrameAsync();
await timer.TimeoutAsync();
```

You can also [cancel signal awaits](CancellableSignalAwaiter.md) by providing a `CancellationToken`. The `ToSignal` asynchronous operation then completes either when the signal is emitted, or when cancellation is requested:

```csharp
public async Task CountdownAsync(CancellationToken cancellationToken) {
    await this.ToSignal(timer, Timer.SignalName.Timeout, cancellationToken);
    if (!cancellationToken.IsCancellationRequested) {
        DoSomething();
    }
}
```
