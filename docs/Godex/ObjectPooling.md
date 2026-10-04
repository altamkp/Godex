# Object Pooling

## Problems

Spawning and freeing the same node over and over again, e.g. bullets, damage numbers or enemies, is expensive. Every instantiation loads and instantiates a scene, and freeing the node again has to be deferred to the end of the frame.

## Solution

`Pool<T>` pre-instantiates a fixed number of nodes, parents them to a node of your choice and hands them out on demand. When the game is done with an instance, returning it to the pool reuses it instead of freeing it.

```csharp
public partial class BulletPool : Node2D {
    private Pool<Bullet> _bullets;

    public override void _Ready() {
        _bullets = new Pool<Bullet>(this, 16, bullet => bullet.Visible = false);
    }

    public Bullet Spawn(Vector2 position) {
        var bullet = _bullets.GetInstance();
        bullet.Position = position;
        bullet.Visible = true;
        return bullet;
    }

    public void Despawn(Bullet bullet) {
        bullet.Visible = false;
        _bullets.ReturnInstance(bullet);
    }
}
```

The pool is created with the node that holds the instances, a capacity, and an optional setup action that runs on every instance before it is added as a child. Instances themselves are created with [`GDx.New<T>()`](InstantiatingPackedScenes.md), so label them with `[PackedScene]` if they should be instantiated from a scene file.

`GetInstance()` returns a pooled instance when one is available and creates a new one otherwise, so requesting an instance never blocks. `ReturnInstance(instance)` puts the instance back into the pool, or frees it when the pool is already at capacity.

> [!Tip]
> Instances keep their parent for their whole lifetime, they are only hidden or parked. Disable whatever should not run while an instance is parked, e.g. with `ProcessMode` or a `Visible` property.