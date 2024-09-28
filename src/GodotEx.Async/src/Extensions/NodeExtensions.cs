using Godot;

namespace GodotEx.Async;

/// <summary>
/// Extensions for <see cref="Node"/>.
/// </summary>
public static class NodeExtensions {
    /// <summary>
    /// Returns a <see cref="SignalAwaiter"/> that completes when the node enters the scene tree.
    /// </summary>
    /// <param name="node">Node to await.</param>
    /// <returns>A <see cref="SignalAwaiter"/> that completes when the node enters the scene tree.</returns>
    public static SignalAwaiter TreeEnteredAsync(this Node node) {
        return node.ToSignal(node, Node.SignalName.TreeEntered);
    }

    /// <summary>
    /// Returns a <see cref="SignalAwaiter"/> that completes when the node enters the scene tree
    /// or when cancellation is requested.
    /// </summary>
    /// <param name="node">Node to await.</param>
    /// <param name="cancellationToken">Cancellation token to provide the awaiter with.</param>
    /// <returns>A <see cref="CancellableSignalAwaiter"/> that completes when the node enters 
    /// the scene tree or when cancellation is requested.</returns>
    public static CancellableSignalAwaiter TreeEnteredAsync(this Node node, CancellationToken cancellationToken) {
        return node.ToSignal(node, Node.SignalName.TreeEntered, cancellationToken);
    }

    /// <summary>
    /// Returns a <see cref="SignalAwaiter"/> that completes when the node is ready.
    /// </summary>
    /// <param name="node">Node to await.</param>
    /// <returns>A <see cref="SignalAwaiter"/> that completes when the node is ready.</returns>
    public static SignalAwaiter ReadyAsync(this Node node) {
        return node.ToSignal(node, Node.SignalName.Ready);
    }

    /// <summary>
    /// Returns a <see cref="SignalAwaiter"/> that completes when the node is ready
    /// or when cancellation is requested.
    /// </summary>
    /// <param name="node">Node to await.</param>
    /// <param name="cancellationToken">Cancellation token to provide the awaiter with.</param>
    /// <returns>A <see cref="CancellableSignalAwaiter"/> that completes when the node 
    /// is ready or when cancellation is requested.</returns>
    public static CancellableSignalAwaiter ReadyAsync(this Node node, CancellationToken cancellationToken) {
        return node.ToSignal(node, Node.SignalName.Ready, cancellationToken);
    }

    /// <summary>
    /// Returns a <see cref="SignalAwaiter"/> that completes when the node is exiting the scene tree.
    /// </summary>
    /// <param name="node">Node to await.</param>
    /// <returns>A <see cref="SignalAwaiter"/> that completes when the node is exiting the scene tree.</returns>
    public static SignalAwaiter TreeExitingAsync(this Node node) {
        return node.ToSignal(node, Node.SignalName.TreeExiting);
    }

    /// <summary>
    /// Returns a <see cref="SignalAwaiter"/> that completes when the node is exiting the scene tree
    /// or when cancellation is requested.
    /// </summary>
    /// <param name="node">Node to await.</param>
    /// <param name="cancellationToken">Cancellation token to provide the awaiter with.</param>
    /// <returns>A <see cref="CancellableSignalAwaiter"/> that completes when the node is 
    /// exiting the scene tree or when cancellation is requested.</returns>
    public static CancellableSignalAwaiter TreeExitingAsync(this Node node, CancellationToken cancellationToken) {
        return node.ToSignal(node, Node.SignalName.TreeExiting, cancellationToken);
    }

    /// <summary>
    /// Returns a <see cref="SignalAwaiter"/> that completes when the node exits the scene tree.
    /// </summary>
    /// <param name="node">Node to await.</param>
    /// <returns>A <see cref="SignalAwaiter"/> that completes when the node exits the scene tree.</returns>
    public static SignalAwaiter TreeExitedAsync(this Node node) {
        return node.ToSignal(node, Node.SignalName.TreeExited);
    }

    /// <summary>
    /// Returns a <see cref="SignalAwaiter"/> that completes when the node exits the scene tree
    /// or when cancellation is requested.
    /// </summary>
    /// <param name="node">Node to await.</param>
    /// <param name="cancellationToken">Cancellation token to provide the awaiter with.</param>
    /// <returns>A <see cref="CancellableSignalAwaiter"/> that completes when the node
    /// exits the scene tree or when cancellation is requested.</returns>
    public static CancellableSignalAwaiter TreeExitedAsync(this Node node, CancellationToken cancellationToken) {
        return node.ToSignal(node, Node.SignalName.TreeExited, cancellationToken);
    }

    /// <summary>
    /// Asynchronously queue frees <paramref name="node"/>.
    /// </summary>
    /// <param name="node">Node to queue free.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static Task QueueFreeAsync(this Node node) {
        var tcs = new TaskCompletionSource();
        node.TreeExited += OnExited;
        node.QueueFree();
        return tcs.Task;

        void OnExited() {
            node.TreeExited -= OnExited;
            tcs.SetResult();
        }
    }

    /// <summary>
    /// Asynchronously queue frees all children of <paramref name="node"/>.
    /// </summary>
    /// <param name="node">Node of which children to queue free.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static Task QueueFreeChildrenAsync(this Node node) {
        var count = node.GetChildCount();
        var tasks = new Task[count];
        for (int i = count - 1; i >= 0; i--) {
            tasks[i] = node.GetChild(i).QueueFreeAsync();
        }
        return Task.WhenAll(tasks);
    }
}
