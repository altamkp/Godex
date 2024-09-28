using Godot;

namespace GodotEx.Async;

/// <summary>
/// Extensions for <see cref="GodotObject"/>.
/// </summary>
public static class GodotObjectExtensions {
    /// <summary>
    /// Returns a new <see cref="SignalAwaiter"/> awaiter configured to complete when the instance source emits the signal specified by the signal parameter.
    /// </summary>
    /// <typeparam name="T">Type to cast result to.</typeparam>
    /// <param name="object">Object to operate <see cref="GodotObject.ToSignal(GodotObject, StringName)"/> on.</param>
    /// <param name="source">Source of which signal is awaited.</param>
    /// <param name="signal">Signal to await.</param>
    /// <returns>A <see cref="SignalAwaiter"/> that completes when source emits the signal.</returns>
    /// <exception cref="InvalidOperationException">Signal did not return 1 result.</exception>
    public static async Task<T> ToSignal<[MustBeVariant] T>(this GodotObject @object, GodotObject source, StringName signal) {
        var results = await @object.ToSignal(source, signal);
        if (results.Length != 1) {
            throw new InvalidOperationException($"Signal {signal} of source {source.GetType().Name} did not return 1 result.");
        }
        return results[0].As<T>();
    }

    /// <summary>
    /// Returns a new <see cref="SignalAwaiter"/> awaiter configured to complete when the instance source emits the signal specified by the signal parameter.
    /// </summary>
    /// <typeparam name="T1">Type to cast result 1 to.</typeparam>
    /// <typeparam name="T2">Type to cast result 2 to.</typeparam>
    /// <param name="object">Object to operate <see cref="GodotObject.ToSignal(GodotObject, StringName)"/> on.</param>
    /// <param name="source">Source of which signal is awaited.</param>
    /// <param name="signal">Signal to await.</param>
    /// <returns>A <see cref="SignalAwaiter"/> that completes when source emits the signal.</returns>
    /// <exception cref="InvalidOperationException">Signal did not return 2 results.</exception>
    public static async Task<(T1, T2)> ToSignal<[MustBeVariant] T1, [MustBeVariant] T2>(this GodotObject @object, GodotObject source, StringName signal) {
        var results = await @object.ToSignal(source, signal);
        if (results.Length != 2) {
            throw new InvalidOperationException($"Signal {signal} of source {source.GetType().Name} did not return 2 results.");
        }
        return (results[0].As<T1>(), results[1].As<T2>());
    }

    /// <summary>
    /// Returns a new <see cref="SignalAwaiter"/> awaiter configured to complete when the instance source emits the signal specified by the signal parameter.
    /// </summary>
    /// <typeparam name="T1">Type to cast result 1 to.</typeparam>
    /// <typeparam name="T2">Type to cast result 2 to.</typeparam>
    /// <typeparam name="T3">Type to cast result 3 to.</typeparam>
    /// <param name="object">Object to operate <see cref="GodotObject.ToSignal(GodotObject, StringName)"/> on.</param>
    /// <param name="source">Source of which signal is awaited.</param>
    /// <param name="signal">Signal to await.</param>
    /// <returns>A <see cref="SignalAwaiter"/> that completes when source emits the signal.</returns>
    /// <exception cref="InvalidOperationException">Signal did not return 3 results.</exception>
    public static async Task<(T1, T2, T3)> ToSignal<[MustBeVariant] T1, [MustBeVariant] T2, [MustBeVariant] T3>(this GodotObject @object, GodotObject source, StringName signal) {
        var results = await @object.ToSignal(source, signal);
        if (results.Length != 3) {
            throw new InvalidOperationException($"Signal {signal} of source {source.GetType().Name} did not return 3 results.");
        }
        return (results[0].As<T1>(), results[1].As<T2>(), results[2].As<T3>());
    }

    /// <summary>
    /// Returns a new <see cref="SignalAwaiter"/> awaiter configured to complete when the instance source emits the signal specified by the signal parameter.
    /// </summary>
    /// <typeparam name="T1">Type to cast result 1 to.</typeparam>
    /// <typeparam name="T2">Type to cast result 2 to.</typeparam>
    /// <typeparam name="T3">Type to cast result 3 to.</typeparam>
    /// <typeparam name="T4">Type to cast result 4 to.</typeparam>
    /// <param name="object">Object to operate <see cref="GodotObject.ToSignal(GodotObject, StringName)"/> on.</param>
    /// <param name="source">Source of which signal is awaited.</param>
    /// <param name="signal">Signal to await.</param>
    /// <returns>A <see cref="SignalAwaiter"/> that completes when source emits the signal.</returns>
    /// <exception cref="InvalidOperationException">Signal did not return 4 results.</exception>
    public static async Task<(T1, T2, T3, T4)> ToSignal<[MustBeVariant] T1, [MustBeVariant] T2, [MustBeVariant] T3, [MustBeVariant] T4>(this GodotObject @object, GodotObject source, StringName signal) {
        var results = await @object.ToSignal(source, signal);
        if (results.Length != 4) {
            throw new InvalidOperationException($"Signal {signal} of source {source.GetType().Name} did not return 4 results.");
        }
        return (results[0].As<T1>(), results[1].As<T2>(), results[2].As<T3>(), results[3].As<T4>());
    }

    /// <summary>
    /// Returns a new <see cref="CancellableSignalAwaiter"/> configured to complete either when the instance source 
    /// emits the signal specified by the signal parameter, or when cancellation is requested.
    /// </summary>
    /// <param name="object">Object to operate <see cref="GodotObject.ToSignal(GodotObject, StringName)"/> on.</param>
    /// <param name="source">Source of which signal is awaited.</param>
    /// <param name="signal">Signal to await.</param>
    /// <param name="cancellationToken">Cancellation token to provide the awaiter with.</param>
    /// <returns>A <see cref="CancellableSignalAwaiter"/> that completes either when the source emits the signal
    /// or when cancellation is requested.</returns>
    public static CancellableSignalAwaiter ToSignal(this GodotObject @object, GodotObject source, StringName signal, CancellationToken cancellationToken) {
        var signalAwaiter = @object.ToSignal(source, signal);
        return new CancellableSignalAwaiter(signalAwaiter, cancellationToken);
    }
}
