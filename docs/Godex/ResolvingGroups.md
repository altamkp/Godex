# Resolving Groups

## Problems

Nodes that belong to the same [group](https://docs.godotengine.org/en/stable/using_groups.html) have to be added to that group one by one, either in the editor or through `AddToGroup()` calls scattered across the codebase.

## Solution

The `[Group]` attribute lets a node class declare its group, and `ResolveGroup()` adds the node to it:

```csharp
[Group("enemies")]
public partial class Enemy : CharacterBody3D { }
```

The attribute is declared on the class, so the same group name applies to every instance of that class, and the group can then be queried from anywhere in the scene tree:

```csharp
[Group("enemies")]
public partial class Enemy : CharacterBody3D { }

// anywhere in the scene tree
foreach (var enemy in GetTree().GetNodesInGroup("enemies")) {
    // ...
}
```

`ResolveGroup()` does nothing when the class has no `[Group]` attribute.

Just like [node path](ResolvingNodeDependencies.md) and [bit flag](ResolvingBitFlags.md) resolving, this is handled by `Resolve()`, and is done for you when the node is created with `GDx.New()`.

## Usage with `Godex.Hosting.Host`

The host resolves every node as it enters the scene tree, so nodes labeled with `[Group]` are added to their group without any call in their own code. Learn how to set up an application scoped [autoload host](~/Godex.Hosting/Hosting.md#setting-up-an-autoload-host) here.