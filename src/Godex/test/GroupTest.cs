namespace Godex.Tests;

using GdUnit4;
using Godot;
using static GdUnit4.Assertions;

[TestSuite]
[RequireGodotRuntime]
public partial class GroupTest {
    [Group("enemies")]
    private partial class Enemy : Node { }

    private partial class Neutral : Node { }

    [TestCase]
    public void ResolveGroupAddsTheNodeToItsGroup() {
        Enemy enemy = AddNode(new Enemy(), true);

        enemy.ResolveGroup();

        AssertBool(enemy.IsInGroup("enemies")).IsTrue();
    }

    [TestCase]
    public void ResolveGroupLeavesUngroupedNodesAlone() {
        Neutral node = AddNode(new Neutral(), true);

        node.ResolveGroup();

        AssertArray(node.GetGroups()).IsEmpty();
    }

    [TestCase]
    public void ResolveAddsTheNodeToItsGroup() {
        Enemy enemy = AddNode(new Enemy(), true);

        enemy.Resolve();

        AssertBool(enemy.IsInGroup("enemies")).IsTrue();
    }
}