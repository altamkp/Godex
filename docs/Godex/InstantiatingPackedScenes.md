# Instantiating Packed Scenes

## Problems

From time to time, you would need to instantiate packed scenes and cast them to a target type in order to use them.

Let's say you have a project structure as follows:

![](~/images/ScenePathStructure.png)

The official way requires a lot of redundant code:

```csharp
var playerScene = GD.Load<PackedScene>("res://Path/To/Player.tscn");
var player = playerScene.Instantiate<Player>();
```

## Solution

With the `GDx` utilities, you can instantiate packed scenes a lot easier in a few different ways:

1. Providing an explicit scene path with `GDx.New<T>(string path)`

   ```csharp
   var player = GDx.New<Player>("res://Path/To/Player.tscn");
   ```

2. Providing an explicit scene path with the `[PackedScene]` attribute and using `GDx.New<T>()`

   ```csharp
   // Define [PackedScene] in your custom class
   [PackedScene("res://Path/To/Player.tscn")]
   public partial class Player : CharacterBody3D { }

   // Call GDx.New() elsewhere
   var player = GDx.New<Player>();
   ```

3. Defining the `[PackedScene]` attribute with an implicit path and using `GDx.New<T>()`

   ```csharp
   // Define [PackedScene] in your custom class
   [PackedScene]
   public partial class Player : CharacterBody3D { }

   // Call GDx.New() elsewhere
   var player = GDx.New<Player>();
   ```

The third method is the **recommended** way. It uses an implicit file path which discovers the `tscn` file with the name of your custom class located under the folder that holds the `.cs` file defining the class. For this example, the corresponding scene file of the `Player` class should be named as `Player.tscn` and located at `res://Path/To`.

All overloads accept an optional setup action that is invoked on the instantiated node, which is handy for configuring it right away:

```csharp
var player = GDx.New<Player>(p => p.Health = 100);
```

Instantiated nodes have their [node dependencies](ResolvingNodeDependencies.md) resolved before the setup action is invoked, so there is no need to call `Resolve()` on them. Overloads without the `[PackedScene]` attribute simply construct the node with `Activator.CreateInstance()`, which is useful for nodes that have no scene file at all.
