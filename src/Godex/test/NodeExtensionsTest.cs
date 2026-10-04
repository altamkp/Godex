namespace Godex.Tests;

using GdUnit4;
using Godot;
using static GdUnit4.Assertions;

[TestSuite]
[RequireGodotRuntime]
public partial class NodeExtensionsTest {
    /// <summary>Nested fixture: does the Godot source generator accept it?</summary>
    private partial class Nested : Node { }

    [TestCase]
    public void NestedFixtureClassCanBeInstantiated() {
        Nested node = AddNode(new Nested(), true);
        AssertObject(node).IsNotNull();
    }

    [TestCase]
    public void GetChildrenReturnsOnlyMatchingChildren() {
        Node root = AddNode(new Node(), true);
        Node2D first = new();
        Label second = new();
        root.AddChild(first);
        root.AddChild(second);

        AssertArray(root.GetChildren<Node2D>()).HasSize(1);
        AssertArray(root.GetChildren<Label>()).HasSize(1);
        AssertArray(root.GetChildren<Control>()).HasSize(1);
        AssertArray(root.GetChildren<Camera2D>()).IsEmpty();
    }

    [TestCase]
    public void GetDescendantsWalksTheWholeTree() {
        Node root = AddNode(new Node(), true);
        Node branch = new();
        root.AddChild(branch);
        Node2D leaf = new();
        branch.AddChild(leaf);

        AssertArray(root.GetDescendants()).HasSize(2);
        AssertArray(root.GetDescendants<Node2D>()).HasSize(1);
        AssertArray(root.GetDescendants<Control>()).IsEmpty();
    }

    [TestCase]
    public void GetDescendantsDoesNotIncludeTheNodeItself() {
        Node root = AddNode(new Node2D(), true);
        AssertArray(root.GetDescendants<Node2D>()).IsEmpty();
    }

    [TestCase]
    public void GetClosestAncestorWalksUpToTheFirstMatch() {
        Node root = AddNode(new Node(), true);
        Node middle = new();
        root.AddChild(middle);
        Node leaf = new();
        middle.AddChild(leaf);

        AssertObject(leaf.GetClosestAncestor<Node>()).IsSame(middle);
        AssertObject(leaf.GetClosestAncestor<Node2D>()).IsNull();
    }

    [TestCase]
    public void GetClosestAncestorReturnsNullForADetachedNode() {
        // A detached node has no parent at all, so there is no ancestor to find.
        Node detached = new();
        AssertObject(detached.GetClosestAncestor<Node>()).IsNull();
    }

    [TestCase]
    public void FreeChildrenRemovesEveryChild() {
        Node root = AddNode(new Node(), true);
        root.AddChild(new Node());
        root.AddChild(new Node());

        root.FreeChildren();
        AssertInt(root.GetChildCount()).IsEqual(0);
    }

    [TestCase]
    public void QueueFreeChildrenMarksEveryChildForDeletion() {
        Node root = AddNode(new Node(), true);
        Node first = new();
        Node second = new();
        root.AddChild(first);
        root.AddChild(second);

        root.QueueFreeChildren();
        // QueueFree is deferred to the end of the frame, so the children are still
        // attached but flagged. That is the whole difference against FreeChildren.
        AssertInt(root.GetChildCount()).IsEqual(2);
        AssertBool(first.IsQueuedForDeletion()).IsTrue();
        AssertBool(second.IsQueuedForDeletion()).IsTrue();
    }

    [TestCase]
    public void IsResolvedIsFalseBeforeResolving() {
        Node root = AddNode(new Node(), true);
        AssertBool(root.IsResolved()).IsFalse();
    }

    [TestCase]
    public void ResolveMarksTheNodeAsResolved() {
        Node root = AddNode(new Node(), true);
        root.Resolve();
        AssertBool(root.IsResolved()).IsTrue();
    }

    [TestCase]
    public void ResolveIsIdempotent() {
        Node root = AddNode(new Node(), true);
        root.Resolve();
        root.Resolve();
        AssertBool(root.IsResolved()).IsTrue();
    }
}