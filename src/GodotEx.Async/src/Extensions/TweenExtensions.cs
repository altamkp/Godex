using Godot;

namespace GodotEx.Async;

/// <summary>
/// Extensions for <see cref="Tween"/>.
/// </summary>
public static class TweenExtensions {
    /// <summary>
    /// Sets the tween's Unscaled metadata property to <paramref name="unscaled"/>.
    /// When calling any of the asynchronous extension methods, tweens will run
    /// disregarding the engine's current timescale if this property is set to true.
    /// </summary>
    /// <param name="tween">Tween to set.</param>
    /// <param name="unscaled">True to set tween to ignore engine timescale.</param>
    /// <returns>Set tween.</returns>
    public static Tween Unscaled(this Tween tween, bool unscaled = true) {
        tween.SetMeta(nameof(Unscaled), unscaled);
        return tween;
    }

    /// <summary>
    /// Asynchronously waits for the tween to finish, or returns immediately if tween 
    /// is not currently running.
    /// </summary>
    /// <param name="tween">Tween to wait.</param>
    /// <returns>A task that represents the asynchronous wait operation.</returns>
    public static Task WaitAsync(this Tween tween) {
        if (!tween.IsRunning()) {
            return Task.CompletedTask;
        }

        var tcs = new TaskCompletionSource();
        tween.Finished += OnFinished;
        return tcs.Task;

        void OnFinished() {
            tween.Finished -= OnFinished;
            tcs.SetResult();
        }
    }

    /// <summary>
    /// Asynchronously tweens an object's property between an initial value and 
    /// <paramref name="finalVal"/> in a span of <paramref name="duration"/> seconds.
    /// </summary>
    /// <param name="tween">Tween to use.</param>
    /// <param name="object">Object to tween.</param>
    /// <param name="property">Property of the object to tween.</param>
    /// <param name="finalVal">Tween target.</param>
    /// <param name="duration">Tween duration.</param>
    /// <returns>A task that represents the asynchronous tween operation.</returns>
    public static Task TweenPropertyAsync(this Tween tween, GodotObject @object, NodePath property, Variant finalVal, float duration) {
        tween.TweenProperty(@object, property, finalVal, GetDuration(tween, duration));
        return tween.WaitAsync();
    }

    /// <summary>
    /// Asynchronously tweens the <paramref name="property"/> of an <paramref name="object"/>
    /// between an initial value and  <paramref name="finalVal"/> within a span of 
    /// <paramref name="duration"/> seconds.
    /// </summary>
    /// <param name="tween">Tween to use.</param>
    /// <param name="object">Object to tween.</param>
    /// <param name="property">Property of the object to tween.</param>
    /// <param name="finalVal">Tween target.</param>
    /// <param name="duration">Tween duration.</param>
    /// <returns>A task that represents the asynchronous tween operation.</returns>
    public static Task TweenPropertyAsync(this Tween tween, GodotObject @object, StringName property, Variant finalVal, float duration) {
        tween.TweenProperty(@object, property.ToString(), finalVal, GetDuration(tween, duration));
        return tween.WaitAsync();
    }

    /// <summary>
    /// Asynchronously calls a series of <paramref name="method"/> by supplying the tweened argument
    /// between <paramref name="from"/> and <paramref name="to"/> within a span of 
    /// <paramref name="duration"/> seconds.
    /// </summary>
    /// <param name="tween">Tween to use.</param>
    /// <param name="method">Method to tween.</param>
    /// <param name="from">Initial argument value.</param>
    /// <param name="to">Final argument value.</param>
    /// <param name="duration">Tween duration.</param>
    /// <returns>A task that represents the asynchronous tween operation.</returns>
    public static Task TweenMethodAsync(this Tween tween, Callable method, Variant from, Variant to, float duration) {
        tween.TweenMethod(method, from, to, GetDuration(tween, duration));
        return tween.WaitAsync();
    }

    /// <summary>
    /// Asynchronously calls a series of <paramref name="method"/> by supplying the tweened argument
    /// between <paramref name="from"/> and <paramref name="to"/> within a span of 
    /// <paramref name="duration"/> seconds.
    /// </summary>
    /// <typeparam name="T">Variant type.</typeparam>
    /// <param name="tween">Tween to use.</param>
    /// <param name="method">Method to tween.</param>
    /// <param name="from">Initial argument value.</param>
    /// <param name="to">Final argument value.</param>
    /// <param name="duration">Tween duration.</param>
    /// <returns>A task that represents the asynchronous tween operation.</returns>
    public static Task TweenMethodAsync<[MustBeVariant] T>(this Tween tween, Action<T> method, T from, T to, float duration) {
        tween.TweenMethod(Callable.From(method), Variant.From(from), Variant.From(to), GetDuration(tween, duration));
        return tween.WaitAsync();
    }

    private static float GetDuration(Tween tween, float duration) {
        if (!tween.HasMethod(nameof(Unscaled))) {
            return duration;
        }
        var unscaled = tween.GetMeta(nameof(Unscaled)).AsBool();
        return unscaled ? (float)(duration / Engine.TimeScale) : duration;
    }
}
