using Godot;
using Microsoft.Extensions.Hosting;

namespace Qkabi.GodotEx.Hosting;

internal class NodeResolver : IHostedService, IDisposable {
    private readonly SceneTree _tree;

    public NodeResolver(SceneTree tree) {
        _tree = tree;
        _tree.NodeAdded += NodeExtensions.Resolve;
    }

    public Task StartAsync(CancellationToken _) => Task.CompletedTask;
    public Task StopAsync(CancellationToken _) => Task.CompletedTask;

    public void Dispose() {
        _tree.NodeAdded -= NodeExtensions.Resolve;
    }
}
