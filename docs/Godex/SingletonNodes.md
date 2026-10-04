# Singleton Nodes

## Introduction

A singleton node is a concept similar to [singleton](https://refactoring.guru/design-patterns/singleton) that is managed by the [SceneTree](https://docs.godotengine.org/en/stable/classes/class_scenetree.html).

Nodes can be registered to the `SceneTree` as singleton nodes and can be accessed globally. There may only be one instance per singleton node type within a `SceneTree`, registering another node of the same type throws an `InvalidOperationException`.

The following demonstrates a use case where the `CinematicCamera` is registered as a singleton node. In a level scattered with several `PointOfInterest`s, when the `Player` enters a point of interest, the `PointOfInterest` accesses the `CinematicCamera` via the `SceneTree` and activates it such that the camera animates from its current position to the player's position.

```csharp
public partial class Player : CharacterBody3D { }

public partial class PointOfInterest : Area3D {
    public override void _Ready() {
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node3D node) {
        if (node is Player player) {
            var camera = GetTree().GetRequiredSingleton<CinematicCamera>();
            camera.Activate(player);
        }
    }
}

public partial class CinematicCamera : Camera3D {
    [Export] public float TweenDuration { get; set; } = 60;

    public Node3D Subject { get; set; }

    public override void _EnterTree() => GetTree().AddSingleton(this);

    public override void _Ready() => SetProcess(false);

    public override void _Process(double delta) => LookAt(Subject);

    public override void _ExitTree() => GetTree().RemoveSingleton(this);

    public void Activate(Node3D subject) {
        SetProcess(true);
        var target = subject.GlobalTransform.Basis.Z * 10;
        var tween = CreateTween();
        tween.TweenProperty(this, Node3D.PropertyName.GlobalPosition, target, TweenDuration);
        tween.TweenCallback(Callable.From(() => SetProcess(false))).SetDelay(TweenDuration);
    }
}
```

There are 4 extensions for looking up singleton nodes: `GetSingleton<T>()` and `GetSingleton(Type)` return null if no singleton node is registered for the type, while `GetRequiredSingleton<T>()` and `GetRequiredSingleton(Type)` throw an `InvalidOperationException` instead.

## Usage with `Godex.Hosting.Host`

It is **highly recommended** to use singleton nodes together with `Godex.Hosting.Host` since it provides a hosted service for adding/removing singleton nodes as they enter/exit the scene tree, hence you don't need to call `GetTree().AddSingleton(this)` and `GetTree().RemoveSingleton(this)` manually in their respective virtual methods. Learn how to set up an application scoped [autoload host](~/Godex.Hosting/Hosting.md#setting-up-an-autoload-host) here.

With this approach, you can simply label the class with the `[Singleton]` attribute.

```csharp
[Singleton]
public partial class CinematicCamera : Camera3D { 
    // Properties and methods neglected
}
```