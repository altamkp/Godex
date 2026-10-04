# Godex

**Godot** **Ex**tra provides a set of extension libraries for Godot in C#.

Currently available extension libraries:

[Godex](https://altamkp.github.io/Godex/Godex)

Basic extension library for Godot:

- Extension methods for Godot classes such as [InputEvent](https://docs.godotengine.org/en/stable/classes/class_inputevent.html), [Node](https://docs.godotengine.org/en/stable/classes/class_node.html), [Transform3D](https://docs.godotengine.org/en/stable/classes/class_transform3d.html), etc.
- Utilities for [node path resolving](https://altamkp.github.io/Godex/Godex/ResolvingNodeDependencies.html), [packed scene instantiation](https://altamkp.github.io/Godex/Godex/InstantiatingPackedScenes.html), [raycast](https://altamkp.github.io/Godex/Godex/Raycast.html), [input handling](https://altamkp.github.io/Godex/Godex/InputHandling.html), [object pooling](https://altamkp.github.io/Godex/Godex/ObjectPooling.html), etc.

[Godex.Async](https://altamkp.github.io/Godex/Godex.Async)

Asynchronous extension library for Godot:

- Awaitables for common Godot object signals such as [Timer.Timeout](https://docs.godotengine.org/en/stable/classes/class_timer.html#:~:text=%C2%B6-,timeout) and [SceneTree.ProcessFrame](https://docs.godotengine.org/en/stable/classes/class_scenetree.html#:~:text=the%20SceneTree.-,process_frame)
- `CancellableSignalAwaiter` that wraps the Godot [SignalAwaiter](https://github.com/godotengine/godot/blob/master/modules/mono/glue/GodotSharp/GodotSharp/Core/SignalAwaiter.cs), provides functionality similar to that of [ToSignal()](https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_signals.html#signals-as-c-events) while also accepting a [CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken?view=net-8.0)

[Godex.Hosting](https://altamkp.github.io/Godex/Godex.Hosting)

Hosting extension library for Godot:

- A `Host` node that provides hosting functionalities with [ServiceProvider](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.serviceprovider?view=dotnet-plat-ext-8.0)
- Dependency injection through the above `Host`
  
## Prerequisites

- [.NET 10.0](https://dotnet.microsoft.com/en-us/download)+
- [Godot Engine - .NET 4.7.2](https://godotengine.org/)+

## Installation

Choose the package(s) you need and run the following command(s) to install the nuget package(s).

```
dotnet add package Godex
dotnet add package Godex.Async
dotnet add package Godex.Hosting
```

## Documentation

Please refer to [this page](https://altamkp.github.io/Godex) for a detailed documentation on all available extension libraries.

## License

Distributed under the [MIT License](https://github.com/altamkp/Godex/blob/master/LICENSE.md). Copyright (c) 2024 [altamkp](https://github.com/altamkp).
