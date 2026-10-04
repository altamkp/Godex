namespace Godex.Tests;

using GdUnit4;
using Godot;
using static GdUnit4.Assertions;

[TestSuite]
[RequireGodotRuntime]
public partial class BitFlagsTest {
    private enum PhysicsLayers : uint {
        None = 0,

        Player = 1 << 0,
        Melee = 1 << 1,
        Projectile = 1 << 2,

        NonPlayer = ~Player,
    }

    private enum RenderLayers : uint {
        Player = 1 << 0,
        Enemy = 1 << 1,
    }

    [BitFlags("collision_layer", PhysicsLayers.Player)]
    [BitFlags("collision_mask", PhysicsLayers.NonPlayer)]
    private partial class Body : CharacterBody3D {
        [BitFlags("collision_layer", PhysicsLayers.None)]
        [BitFlags("collision_mask", PhysicsLayers.Melee | PhysicsLayers.Projectile)]
        private Area3D _area3D;

        [BitFlags("cull_mask", RenderLayers.Player)]
        private Camera3D _camera3D;

        public Area3D Area3D {
            get => _area3D;
            set => _area3D = value;
        }

        public Camera3D Camera3D {
            get => _camera3D;
            set => _camera3D = value;
        }
    }

    /// <summary>Flags pointing at a property that is not an int.</summary>
    [BitFlags("position", PhysicsLayers.Player)]
    private partial class NotAnIntProperty : Node3D { }

    /// <summary>Flags pointing at a property the node does not have.</summary>
    [BitFlags("no_such_property", PhysicsLayers.Player)]
    private partial class MissingProperty : Node3D { }

    /// <summary>Flags on a member that is not a Node.</summary>
    private partial class MemberIsNotANode : Node3D {
        [BitFlags("cull_mask", RenderLayers.Player)]
        private int _notANode;
    }

    private Body CreateBody() {
        Body body = AddNode(new Body(), true);
        body.Area3D = new Area3D();
        body.Camera3D = new Camera3D();
        return body;
    }

    [TestCase]
    public void ResolveBitFlagsAppliesClassLevelFlags() {
        Body body = CreateBody();

        body.ResolveBitFlags();

        AssertInt((int)body.CollisionLayer).IsEqual((int)PhysicsLayers.Player);
        AssertInt((int)body.CollisionMask).IsEqual(unchecked((int)PhysicsLayers.NonPlayer));
    }

    [TestCase]
    public void ResolveBitFlagsAppliesMemberLevelFlags() {
        Body body = CreateBody();

        body.ResolveBitFlags();

        AssertInt((int)body.Area3D.CollisionLayer).IsEqual((int)PhysicsLayers.None);
        AssertInt((int)body.Area3D.CollisionMask).IsEqual((int)(PhysicsLayers.Melee | PhysicsLayers.Projectile));
        AssertInt((int)body.Camera3D.CullMask).IsEqual((int)RenderLayers.Player);
    }

    [TestCase]
    public void ResolveBitFlagsThrowsWhenThePropertyIsNotAnInt() {
        NotAnIntProperty node = AddNode(new NotAnIntProperty(), true);

        AssertThrown(node.ResolveBitFlags).HasMessage("Property position of NotAnIntProperty is not of type int.");
    }

    [TestCase]
    public void ResolveBitFlagsThrowsWhenThePropertyIsMissing() {
        MissingProperty node = AddNode(new MissingProperty(), true);

        AssertThrown(node.ResolveBitFlags).HasMessage("MissingProperty does not have a property named no_such_property.");
    }

    [TestCase]
    public void ResolveBitFlagsThrowsWhenTheMemberIsNotANode() {
        MemberIsNotANode node = AddNode(new MemberIsNotANode(), true);

        AssertThrown(node.ResolveBitFlags).HasMessage("Member _notANode of MemberIsNotANode must be derived from Node to use the Layer/Mask attribute.");
    }

    [TestCase]
    public void ResolveRunsBitFlagsAlongsideTheOtherResolvers() {
        Body body = CreateBody();

        body.Resolve();

        AssertInt((int)body.CollisionLayer).IsEqual((int)PhysicsLayers.Player);
        AssertBool(body.IsResolved()).IsTrue();
    }
}
