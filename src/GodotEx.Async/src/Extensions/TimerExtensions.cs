using Godot;
using Timer = Godot.Timer;

namespace GodotEx.Async;

/// <summary>
/// Extensions for <see cref="Timer"/>.
/// </summary>
public static class TimerExtensions {
    /// <summary>
    /// Returns a <see cref="SignalAwaiter"/> that completes on the timer timeout.
    /// </summary>
    /// <param name="timer">Timer to await.</param>
    /// <returns>A <see cref="SignalAwaiter"/> that completes on timer timeout.</returns>
    public static SignalAwaiter TimeoutAsync(this Timer timer) {
        return timer.ToSignal(timer, Timer.SignalName.Timeout);
    }

    /// <summary>
    /// Returns a <see cref="CancellableSignalAwaiter"/> that completes on timer timeout or cancellation request.
    /// </summary>
    /// <param name="timer">Timer to await.</param>
    /// <param name="cancellationToken">Cancellation token to provide the awaiter with.</param>
    /// <returns>A <see cref="CancellableSignalAwaiter"/> that completes on timer timeout or cancellation request.</returns>
    public static CancellableSignalAwaiter TimeoutAsync(this Timer timer, CancellationToken cancellationToken) {
        return timer.ToSignal(timer, Timer.SignalName.Timeout, cancellationToken);
    }
}
