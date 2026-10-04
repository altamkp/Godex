# Raycast

## Problems

Let's say you need to use raycast and after following the official [documentation](https://docs.godotengine.org/en/stable/tutorials/physics/ray-casting.htm) you end up with this code:

```csharp
var spaceState = GetWorld2D().DirectSpaceState;
var query = PhysicsRayQueryParameters2D.Create(Vector2.Zero, new Vector2(50, 100));
var result = spaceState.IntersectRay(query);
```

The `result` from a raycast query is of type [Godot.Collections.Dictionary](https://docs.godotengine.org/en/stable/tutorials/physics/ray-casting.html#:~:text=The%20result%20is%20a%20dictionary). While flexible, we would almost certainly need to access one or more of its properties.

## Solution

Instead of writing:

```csharp
var position = result["position"].AsVector2();
var normal = result["normal"].AsVector2();
GD.Print($"Raycast hit surface at {position} with normal {normal}.");
```

With `RaycastHit2D` you can write:

```csharp
if (result.ToRaycastHit2D() is RaycastHit2D hit) {
    GD.Print($"Raycast hit surface at {hit.Position} with normal {hit.Normal}.");
}
```

`ToRaycastHit2D()` and `ToRaycastHit3D()` return `null` when the dictionary holds no hit.

## Viewport Extensions

There are currently two 3D viewport extension methods regarding the use of raycast. These can be easily used by setting the project in `Project > Project Settings... > 2D/3D Physics` and defining your own `PhysicsLayers3D` enum. 

> [!Tip]
> Learn more about [bit flags](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/enum#enumeration-types-as-bit-flags) and [bitwise/shift operators](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/bitwise-and-shift-operators#left-shift-operator-).

```csharp
[Flags]
public enum PhysicsLayers3D : uint {
    None = 0,
    Ground = 1 << 0,    // Bit 0, value 1
    Player = 1 << 1,    // Bit 1, value 2
    Enemy = 1 << 2,     // Bit 2, value 4
    Obstacles = 1 << 3, // Bit 3, value 8
}
```

After setting up the physics layers, the following extensions can be used:

1. Mouse raycast - getting raycast result from the current mouse position to specified physics layers

   ```csharp
   var hit = GetViewport().GetMouseRaycast(PhysicsLayers3D.Ground | PhysicsLayers3D.Obstacles);
   ```

2. Center raycast - getting raycast result from the center of the viewport to specified physics layers

   ```csharp
   var hit = GetViewport().GetCenterRaycast(PhysicsLayers3D.Player | PhysicsLayers3D.Enemy);
   ```

Both extensions require a [Camera3D](https://docs.godotengine.org/en/stable/classes/class_camera3d.html) in the viewport and return a `RaycastHit3D?`, which is null when the ray does not hit anything. The ray length defaults to `RaycastDef.DEFAULT_RAY_LENGTH` and can be passed as the last argument:

```csharp
var hit = GetViewport().GetCenterRaycast(PhysicsLayers3D.Player | PhysicsLayers3D.Enemy, 500);
```
