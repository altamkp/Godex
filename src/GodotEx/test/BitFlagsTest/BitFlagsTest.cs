using Godot;

namespace GodotEx.Tests;

[BitFlags("collision_layer", PhysicsLayers3D.Player)]
[BitFlags("collision_mask", PhysicsLayers3D.NonPlayer)]
public partial class BitFlagsTest : CharacterBody3D {
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
