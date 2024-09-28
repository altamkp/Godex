# Cancellable Signal Awaiter

Godot's [ToSignal()](https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_signals.html#signals-as-c-events) method returns a type called `SignalAwaiter` which completes when the awaited node emits a specified signal, then continuing with any further code execution.

With `ToSignal()`, you can write asynchronous code as follows. Let's say you have a `RailGun` class that charges up when the space key is pressed and shoots after 5 seconds:

```csharp
public partial class RailGun : Node3D {
    [Export] public int ChargeTime { get; set; } = 5;

    [Export] private Timer _timer;

    public override void _ShortcutInput(InputEvent @event) {
        if (@event is InputEventKey keyEvent) {
            if (keyEvent.KeyCode == Key.Space && keyEvent.IsPressed()) {
                _ = CountdownAsync(ChargeTime);
            }
        }
    }

    private async Task CountdownAsync(int seconds) {
        _timer.Start(seconds);
        await this.ToSignal(_timer, Timer.SignalName.Timeout);

        var charge = ChargeTime - _timer.TimeLeft; 
        Shoot(charge); // Charge always equals ChargeTime since TimeLeft is 0.
    }

    private void Shoot(float charge) {
        // Discharge logic and animations
    }
}
```

> [!Tip]
> The `CountdownAsync(int)` method uses the task asynchronous programming [(TAP)](https://learn.microsoft.com/en-us/dotnet/csharp/asynchronous-programming/task-asynchronous-programming-model) model which allows the writing of asynchronous codes that look synchronous, by enabling code to be written and executed in a top-down manner. Callback and event based asynchronous models would also lead to the same result but would cause problems such as stack overflow if the operation completes synchronously. You can read more on [C# async/await](https://devblogs.microsoft.com/dotnet/how-async-await-really-works/) if you like, but this is outside the scope of this documentation.

There is one missing part to the `ToSignal()` functionality provided with Godot, which is cancellation. Taking the above as example, you may want to stop and reset the countdown any time because some conditions are met, events are triggered, etc.

Let's say apart from shooting when time is up, `RailGun` can also shoot as soon as the space key is released, with a charge equals to the time passed since the timer has started. The current `ToSignal()` lacks this ability and would only continue to call `Shoot()` when time defined by `ChargeTime` has passed by.

`CancellableSignalAwaiter` allows cancellation on signal awaits. You could either create a new `CancellableSignalAwaiter` with its constructor which takes in `SignalAwaiter` and a `CancellationToken` as parameters, or use the `ToSignal()` extension which takes an additional `CancellationToken` as parameter compared to the original method.

The above example then becomes:

```csharp
public partial class PowerUp : Area3D {
    [Export] public int ChargeTime { get; set; } = 5;

    [Export] private Timer _timer;

    private CancellationTokenSource _cts = new();

    public override void _ShortcutInput(InputEvent @event) {
        if (@event is InputEventKey keyEvent) {
            if (keyEvent.KeyCode == Key.Space) {
                if (keyEvent.IsPressed()) {
                    _ = CountdownAsync(ChargeTime);
                } else {
                    _cts.Cancel();
                }
            }
        }
    }

    private async Task CountdownAsync(int seconds) {
        _timer.Start(seconds);
        await this.ToSignal(_timer, Timer.SignalName.Timeout, _cts.Token);

        var charge = ChargeTime - _timer.TimeLeft; 
        Shoot(charge); // Charge may now vary since TimeLeft depends on when space is released.
    }

    private void Shoot(float charge) {
        // Discharge logic and animations
    }
}
```
