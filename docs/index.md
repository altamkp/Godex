# Godex

**Godot** **Ex**tra provides a set of extension libraries for Godot in C#.

## [Godex](Godex/index.md)

Basic extension library for Godot:

- Extension methods for Godot classes such as [InputEvent](https://docs.godotengine.org/en/stable/classes/class_inputevent.html), [Node](https://docs.godotengine.org/en/stable/classes/class_node.html), [Transform3D](https://docs.godotengine.org/en/stable/classes/class_transform3d.html), etc.
- Utilities for [node path resolving](Godex/ResolvingNodeDependencies.md), [packed scene instantiation](Godex/InstantiatingPackedScenes.md), [raycast](Godex/Raycast.md), [input handling](Godex/InputHandling.md), [object pooling](Godex/ObjectPooling.md), etc.

## [Godex.Async](Godex.Async/index.md)

Asynchronous extension library for Godot:

- Awaitables for common Godot object signals such as [Timer.Timeout](https://docs.godotengine.org/en/stable/classes/class_timer.html#:~:text=%C2%B6-,timeout) and [SceneTree.ProcessFrame](https://docs.godotengine.org/en/stable/classes/class_scenetree.html#:~:text=the%20SceneTree.-,process_frame)
- `CancellableSignalAwaiter` that wraps the Godot [SignalAwaiter](https://github.com/godotengine/godot/blob/master/modules/mono/glue/GodotSharp/GodotSharp/Core/SignalAwaiter.cs), provides functionality similar to that of [ToSignal()](https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_signals.html#signals-as-c-events) while also accepting a [CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken?view=net-8.0)

## [Godex.Hosting](Godex.Hosting/index.md)

Hosting extension library for Godot:

- A `Host` node that provides hosting functionalities with [ServiceProvider](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.serviceprovider?view=dotnet-plat-ext-8.0)
- Dependency injection through the above `Host`

## Prerequisites

- [.NET 10.0](https://dotnet.microsoft.com/en-us/download)+
- [Godot Engine - .NET 4.7.2](https://godotengine.org/)+

## License

Distributed under the [MIT License](https://github.com/altamkp/Godex/blob/master/LICENSE.md). Copyright (c) 2024 [altamkp](https://github.com/altamkp).
