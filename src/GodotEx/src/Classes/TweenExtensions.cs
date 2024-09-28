using Godot;

namespace GodotEx;

/// <summary>
/// Extensions for <see cref="Tween"/>.
/// </summary>
public static class TweenExtensions {
    /// <summary>
    /// Sets the tween to a default behaviour of parallel and in-out ease.
    /// </summary>
    /// <param name="tween">Tween to set.</param>
    /// <returns>Set tween.</returns>
    public static Tween Default(this Tween tween) => tween.SetParallel().SetEase(Tween.EaseType.InOut);

    /// <summary>
    /// Sets the ease type of <paramref name="tween"/> to <see cref="Tween.EaseType.In"/>.
    /// </summary>
    /// <param name="tween"></param>
    /// <returns>Set tween.</returns>
    public static Tween In(this Tween tween) => tween.SetEase(Tween.EaseType.In);

    /// <summary>
    /// Sets the ease type of <paramref name="tween"/> to <see cref="Tween.EaseType.Out"/>.
    /// </summary>
    /// <param name="tween"></param>
    /// <returns>Set tween.</returns>
    public static Tween Out(this Tween tween) => tween.SetEase(Tween.EaseType.Out);

    /// <summary>
    /// Sets the ease type of <paramref name="tween"/> to <see cref="Tween.EaseType.InOut"/>.
    /// </summary>
    /// <param name="tween"></param>
    /// <returns>Set tween.</returns>
    public static Tween InOut(this Tween tween) => tween.SetEase(Tween.EaseType.InOut);

    /// <summary>
    /// Sets the ease type of <paramref name="tween"/> to <see cref="Tween.EaseType.OutIn"/>.
    /// </summary>
    /// <param name="tween"></param>
    /// <returns>Set tween.</returns>
    public static Tween OutIn(this Tween tween) => tween.SetEase(Tween.EaseType.OutIn);

    /// <summary>
    /// Sets the transition type of <paramref name="tween"/> to <see cref="Tween.TransitionType.Linear"/>.
    /// </summary>
    /// <param name="tween">Tween to set.</param>
    /// <returns>Set tween.</returns>
    public static Tween Linear(this Tween tween) => tween.SetTrans(Tween.TransitionType.Linear);

    /// <summary>
    /// Sets the transition type of <paramref name="tween"/> to <see cref="Tween.TransitionType.Sine"/>.
    /// </summary>
    /// <param name="tween">Tween to set.</param>
    /// <returns>Set tween.</returns>
    public static Tween Sine(this Tween tween) => tween.SetTrans(Tween.TransitionType.Sine);

    /// <summary>
    /// Sets the transition type of <paramref name="tween"/> to <see cref="Tween.TransitionType.Quint"/>.
    /// </summary>
    /// <param name="tween">Tween to set.</param>
    /// <returns>Set tween.</returns>
    public static Tween Quint(this Tween tween) => tween.SetTrans(Tween.TransitionType.Quint);

    /// <summary>
    /// Sets the transition type of <paramref name="tween"/> to <see cref="Tween.TransitionType.Quart"/>.
    /// </summary>
    /// <param name="tween">Tween to set.</param>
    /// <returns>Set tween.</returns>
    public static Tween Quart(this Tween tween) => tween.SetTrans(Tween.TransitionType.Quart);

    /// <summary>
    /// Sets the transition type of <paramref name="tween"/> to <see cref="Tween.TransitionType.Quad"/>.
    /// </summary>
    /// <param name="tween">Tween to set.</param>
    /// <returns>Set tween.</returns>
    public static Tween Quad(this Tween tween) => tween.SetTrans(Tween.TransitionType.Quad);

    /// <summary>
    /// Sets the transition type of <paramref name="tween"/> to <see cref="Tween.TransitionType.Expo"/>.
    /// </summary>
    /// <param name="tween">Tween to set.</param>
    /// <returns>Set tween.</returns>
    public static Tween Expo(this Tween tween) => tween.SetTrans(Tween.TransitionType.Expo);

    /// <summary>
    /// Sets the transition type of <paramref name="tween"/> to <see cref="Tween.TransitionType.Elastic"/>.
    /// </summary>
    /// <param name="tween">Tween to set.</param>
    /// <returns>Set tween.</returns>
    public static Tween Elastic(this Tween tween) => tween.SetTrans(Tween.TransitionType.Elastic);

    /// <summary>
    /// Sets the transition type of <paramref name="tween"/> to <see cref="Tween.TransitionType.Cubic"/>.
    /// </summary>
    /// <param name="tween">Tween to set.</param>
    /// <returns>Set tween.</returns>
    public static Tween Cubic(this Tween tween) => tween.SetTrans(Tween.TransitionType.Cubic);

    /// <summary>
    /// Sets the transition type of <paramref name="tween"/> to <see cref="Tween.TransitionType.Circ"/>.
    /// </summary>
    /// <param name="tween">Tween to set.</param>
    /// <returns>Set tween.</returns>
    public static Tween Circ(this Tween tween) => tween.SetTrans(Tween.TransitionType.Circ);

    /// <summary>
    /// Sets the transition type of <paramref name="tween"/> to <see cref="Tween.TransitionType.Bounce"/>.
    /// </summary>
    /// <param name="tween">Tween to set.</param>
    /// <returns>Set tween.</returns>
    public static Tween Bounce(this Tween tween) => tween.SetTrans(Tween.TransitionType.Bounce);

    /// <summary>
    /// Sets the transition type of <paramref name="tween"/> to <see cref="Tween.TransitionType.Back"/>.
    /// </summary>
    /// <param name="tween">Tween to set.</param>
    /// <returns>Set tween.</returns>
    public static Tween Back(this Tween tween) => tween.SetTrans(Tween.TransitionType.Back);

    /// <summary>
    /// Sets the transition type of <paramref name="tween"/> to <see cref="Tween.TransitionType.Spring"/>.
    /// </summary>
    /// <param name="tween">Tween to set.</param>
    /// <returns>Set tween.</returns>
    public static Tween Spring(this Tween tween) => tween.SetTrans(Tween.TransitionType.Spring);

    /// <summary>
    /// Tries aborting all tweening operations if <paramref name="tween"/> is not currently running.
    /// </summary>
    /// <param name="tween">Tween to kill.</param>
    /// <returns>True if <paramref name="tween"/> is not running and thus successfully killed.</returns>
    public static bool TryKill(this Tween tween) {
        if (!tween.IsRunning()) {
            tween.Kill();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Creates and appends a PropertyTweener. This method tweens a property of an object between 
    /// an initial value and finalVal in a span of time equal to duration, in seconds. 
    /// The initial value by default is the property's value at the time the tweening of the PropertyTweener starts.
    /// </summary>
    /// <param name="tween">Tween to use.</param>
    /// <param name="object">Object to tween.</param>
    /// <param name="property">Property of the object to tween.</param>
    /// <param name="finalVal">Tween target.</param>
    /// <param name="duration">Tween duration.</param>
    /// <returns>Result property tweener.</returns>
    public static PropertyTweener TweenProperty(this Tween tween, GodotObject @object, StringName property, Variant finalVal, float duration) {
        return tween.TweenProperty(@object, property.ToString(), finalVal, duration);
    }
}
