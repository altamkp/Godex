# Resolving Bit Flags

## Problems

In the Project Settings window, we can set the names of several physics, render and navigation layers.

![Layer settings](~/images/LayerSettings.png)

There often comes a time when we want to modify the list of layer names by either inserting a new layer, swapping around 2 layers, etc. Doing so break our game since these settings are static, which then requires us to open up all the scenes involved to change the layers or masks properties manually one by one.

## Solution

By using the `[BitFlags]` attributes, you do not need to worry about breaking the game when you change your layers.

Let's say you have the following scene which represents your character (note that collision shapes are omitted for demonstration):

![Layer structure](~/images/LayerStructure.png)

All of the above nodes contain either properties represented by bit flags.

| `CharacterBody3D`                                     | `Area3D`                            | `Camera3D`                             |
| ----------------------------------------------------- | ----------------------------------- | -------------------------------------- |
| ![CharacterBody3D](~/images/CharacterBody3DLayer.png) | ![Area3D](~/images/Area3DLayer.png) | ![Camera3D](~/images/Camera3DMask.png) |

Normally, you would manually apply the properties according to your project settings, but the downside were already discussed previously.

With this method, you would fill in the layer names in the project settings. Then, you need to create custom enum types to represent each of the specific layer setting that your game use. For our `Character`, create the enums `PhysicsLayers3D` and `RenderLayers3D` respectively.

> [!Tip]
> If you are unsure which layer setting a node uses, click the more button next to the property and click edit, which will highlight the targeted layer setting.
> ![View layer settings](~/images/ViewLayerSettings.png)

<table>
<tr>
<td> Layer Settings </td> <td> Enums <td>
</tr>
<tr>
<td>

![3D Physics Layer](~/images/PhysicsLayers3DSettings.png)

</td>
<td>

```csharp
public enum PhysicsLayers3D : uint {
    None = 0,

    Player = 1 << 0,
    Melee = 1 << 1,
    Projectile = 1 << 2,
    Ground = 1 << 3,

    NonPlayer = ~Player
}
```

</td>
</tr>
<tr>
<td>

![3D Render Layer](~/images/RenderLayers3DSettings.png)

</td>
<td>

```csharp
public enum RenderLayers3D : uint {
    Player = 1 << 0,
    Enemy = 1 << 1,

    All = uint.MaxValue
}
```

</td>
</tr>
</table>

> [!Tip]
> Learn more about [bit flags](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/enum#enumeration-types-as-bit-flags) and [bitwise/shift operators](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/bitwise-and-shift-operators#left-shift-operator-).

At this point, create and attach a script to the `Character` node. Add private fields as reference to the children nodes, then add the `[BitFlags]` attributes and fill in the property names and targeted bit flags.

> [!Tip]
> You can view a node's property name by hovering over the property in the inspector.
> ![View property name](~/images/ViewPropertyName.png)

```csharp
[BitFlags("collision_layer", PhysicsLayers3D.Player)]
[BitFlags("collision_mask", PhysicsLayers3D.NonPlayer)]
public partial class Character : CharacterBody3D {
    [BitFlags("collision_layer", PhysicsLayers3D.None)]
    [BitFlags("collision_mask", PhysicsLayers3D.Melee | PhysicsLayers3D.Projectile)]
    private Area3D _area3D;

    [BitFlags("cull_mask", RenderLayers3D.Player)]
    private Camera3D _camera3D;

    public override void _Ready() {
        _area3D = GetNode<Area3D>("Area3D");
        _camera3D = GetNode<Camera3D>("Camera3D");

        this.Resolve();
    }
}
```

From now on, when you need to modify your list of layer names, simply modify you enums and the changes will be reflected.

`this.Resolve()` resolves node paths and groups as well, use `this.ResolveBitFlags()` if a node only has bit flags to resolve.

## Usage with `[NodePath]`

You can use the `[NodePath]` attribute to [resolve node dependencies](ResolvingNodeDependencies.md) to simplify your code to the following:

```csharp
[BitFlags("collision_layer", PhysicsLayers3D.Player)]
[BitFlags("collision_mask", PhysicsLayers3D.NonPlayer)]
public partial class Character : CharacterBody3D {
    [NodePath]
    [BitFlags("collision_layer", PhysicsLayers3D.None)]
    [BitFlags("collision_mask", PhysicsLayers3D.Melee | PhysicsLayers3D.Projectile)]
    private Area3D _area3D;

    [NodePath]
    [BitFlags("cull_mask", RenderLayers3D.Player)]
    private Camera3D _camera3D;

    public override void _Ready() {
        this.Resolve();
    }
}
```

## Usage with `Godex.Hosting.Host`

It is **highly recommended** to use the `[BitFlags]` resolving functionality together with `Godex.Hosting.Host` since it provides a hosted service for resolving nodes as they enter the scene tree, hence you don't need to call `this.Resolve()` on all nodes that require resolving. Learn how to set up an application scoped [autoload host](~/Godex.Hosting/Hosting.md#setting-up-an-autoload-host) here.
