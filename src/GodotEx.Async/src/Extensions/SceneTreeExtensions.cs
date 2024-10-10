using Godot;

namespace GodotEx.Async;

/// <summary>
/// Extensions for <see cref="SceneTree"/>.
/// </summary>
public static class SceneTreeExtensions {
    /// <summary>
    /// Returns a <see cref="SignalAwaiter"/> that completes on the next process frame.
    /// </summary>
    /// <param name="tree">Scene tree to await.</param>
    /// <returns>A <see cref="SignalAwaiter"/> that completes on the next process frame.</returns>
    public static SignalAwaiter ProcessFrameAsync(this SceneTree tree) {
        return tree.ToSignal(tree, SceneTree.SignalName.ProcessFrame);
    }

    /// <summary>
    /// Returns a <see cref="CancellableSignalAwaiter"/> that completes either on the next process frame or when cancellation is requested.
    /// </summary>
    /// <param name="tree">Scene tree to await.</param>
    /// <param name="cancellationToken">Cancellation token to provide the awaiter with.</param>
    /// <returns>A <see cref="CancellableSignalAwaiter"/> that completes either on the next process frame or when cancellation is requested.</returns>
    public static CancellableSignalAwaiter ProcessFrameAsync(this SceneTree tree, CancellationToken cancellationToken) {
        return tree.ToSignal(tree, SceneTree.SignalName.ProcessFrame, cancellationToken);
    }

    /// <summary>
    /// Returns a <see cref="SignalAwaiter"/> that completes on the next physics frame.
    /// </summary>
    /// <param name="tree">Scene tree to await.</param>
    /// <returns>A <see cref="SignalAwaiter"/> that completes on the next physics frame.</returns>
    public static SignalAwaiter PhysicsFrameAsync(this SceneTree tree) {
        return tree.ToSignal(tree, SceneTree.SignalName.PhysicsFrame);
    }

    /// <summary>
    /// Returns a <see cref="CancellableSignalAwaiter"/> that completes on the next physics frame or when cancellation is requested.
    /// </summary>
    /// <param name="tree">Scene tree to await.</param>
    /// <param name="cancellationToken">Cancellation token to provide the awaiter with.</param>
    /// <returns>A <see cref="CancellableSignalAwaiter"/> that completes on the next physics frame or when cancellation is requested.</returns>
    public static CancellableSignalAwaiter PhysicsFrameAsync(this SceneTree tree, CancellationToken cancellationToken) {
        return tree.ToSignal(tree, SceneTree.SignalName.PhysicsFrame, cancellationToken);
    }
}
