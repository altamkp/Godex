using Godot;

namespace Godex;

/// <summary>
/// Generic object pool for <see cref="Node"/>s.
/// </summary>
/// <typeparam name="T">Node type.</typeparam>
public class Pool<T> where T : Node {
    private readonly Node _node;
    private readonly int _capacity;
    
    private readonly Queue<T> _queue = [];
    private readonly Action<T>? _setup;

    /// <summary>
    /// Creates a new pool.
    /// </summary>
    /// <param name="node">The node where instances are added to.</param>
    /// <param name="capacity">The capacity of the pool, above which new instances are created.</param>
    /// <param name="setup">Setup action for the instance before added as a child of node.</param>
    public Pool(Node node, int capacity, Action<T>? setup = null) {
        _node = node;
        _capacity = capacity;
        _setup = setup;

        for (int i = 0; i < _capacity; i++) {
            var instance = CreateInstance();
            _queue.Enqueue(instance);
        }
    }

    /// <summary>
    /// Gets an existing instance from the pool if any is available,
    /// otherwise, a new instance is created and returned.
    /// </summary>
    /// <returns>Available instance of type <typeparamref name="T"/>.</returns>
    public T GetInstance() => _queue.TryDequeue(out var instance) ? instance : CreateInstance();

    /// <summary>
    /// Returns an instance to the pool. If the pool is full, ie the number
    /// of instances in the pool >= capacity, the instance will be freed.
    /// </summary>
    /// <param name="instance"></param>
    public void ReturnInstance(T instance) {
        if (_queue.Count < _capacity) {
            _queue.Enqueue(instance);
        } else {
            instance.QueueFree();
        }
    }

    private T CreateInstance() {
        var instance = GDx.New<T>();
        _setup?.Invoke(instance);
        _node.AddChild(instance);
        return instance;
    }
}
