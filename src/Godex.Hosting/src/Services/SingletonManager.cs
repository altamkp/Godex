using Godot;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Godex.Hosting;

internal class SingletonManager : IHostedService, IDisposable {
    private readonly SceneTree _tree;

    public SingletonManager(SceneTree tree) {
        _tree = tree;
        _tree.NodeAdded += OnNodeAdded;
        _tree.NodeRemoved += OnNodeRemoved;
    }

    public Task StartAsync(CancellationToken _) => Task.CompletedTask;
    public Task StopAsync(CancellationToken _) => Task.CompletedTask;

    public void Dispose() {
        _tree.NodeAdded -= OnNodeAdded;
        _tree.NodeRemoved -= OnNodeRemoved;
    }

    private void OnNodeAdded(Node node) {
        if (node.GetType().IsDefined(typeof(SingletonAttribute))) {
            _tree.AddSingleton(node);
        }
    }

    private void OnNodeRemoved(Node node) {
        if (node.GetType().IsDefined(typeof(SingletonAttribute))) {
            _tree.RemoveSingleton(node);
        }
    }
}
