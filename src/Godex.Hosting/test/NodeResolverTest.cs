namespace Godex.Hosting.Tests;

using GdUnit4;
using Godex;
using Godex.Hosting;
using Godot;
using static GdUnit4.Assertions;

[TestSuite]
[RequireGodotRuntime]
public partial class NodeResolverTest {
    private partial class Consumer : Node {
        [NodePath] private Label _label;
        [NodePath("Branch/Deep")] private Node3D _explicitPath;
        [NodePath] public Node2D Auto { get; set; }

        public Label Label => _label;
        public Node3D ExplicitPath => _explicitPath;
    }

    /// <summary>A subtree built before it enters the tree, which is what triggers resolving.</summary>
    private static Consumer CreateConsumer() {
        Consumer consumer = new Consumer();

        Label label = new();
        label.Name = "Label";
        consumer.AddChild(label);

        Node2D auto = new();
        auto.Name = "Auto";
        consumer.AddChild(auto);

        Node branch = new() { Name = "Branch" };
        consumer.AddChild(branch);
        branch.AddChild(new Node3D { Name = "Deep" });

        return consumer;
    }

    [TestCase]
    public void ResolverFillsMembersWhenTheSubtreeIsAdded() {
        Consumer consumer = AddNode(CreateConsumer(), true);

        AssertObject(consumer.Label).IsNotNull();
        AssertObject(consumer.ExplicitPath).IsNotNull();
        AssertObject(consumer.Auto).IsNotNull();
    }

    [TestCase]
    public void ResolverMarksTheNodeAsResolved() {
        Consumer consumer = AddNode(CreateConsumer(), true);

        AssertBool(consumer.IsResolved()).IsTrue();
    }

    [TestCase]
    public void ResolverResolvesAnExplicitPath() {
        Consumer consumer = AddNode(CreateConsumer(), true);

        AssertString(consumer.ExplicitPath.Name).IsEqual("Deep");
    }
}
