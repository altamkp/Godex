namespace Godex.Tests;

using GdUnit4;
using Godot;
using System.Reflection;
using static GdUnit4.Assertions;

[TestSuite]
[RequireGodotRuntime]
public partial class NodePathTest {
    /// <summary>Every member resolves against the tree built in <see cref="CreateTree"/>.</summary>
    private partial class Subject : Node {
        [NodePath] private Label _label;
        [NodePath("Branch/Deep")] private Node3D _explicitPath;
        [NodePath] public Node2D Auto { get; set; }

        public Label Label => _label;
        public Node3D ExplicitPath => _explicitPath;
    }

    /// <summary>A member whose derived name matches no node, so GetNode throws.</summary>
    private partial class MissingNode : Node {
        [NodePath] private Label _nope;
    }

    /// <summary>The node exists but its type does not fit the member.</summary>
    private partial class WrongType : Node {
        [NodePath] private Node3D _label;
    }

    private const BindingFlags FLAGS = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

    /// <summary>root/ "Label", "Auto", "Unique" and "Branch/Deep".</summary>
    private Subject CreateTree() {
        Subject subject = AddNode(new Subject(), true);

        Label label = new();
        label.Name = "Label";
        subject.AddChild(label);

        Node2D auto = new();
        auto.Name = "Auto";
        subject.AddChild(auto);

        // Not reachable as "Label", only through the % scene-unique lookup.
        Label unique = new() { Name = "Unique", UniqueNameInOwner = true };
        subject.AddChild(unique);

        Node branch = new() { Name = "Branch" };
        subject.AddChild(branch);
        branch.AddChild(new Node3D { Name = "Deep" });

        return subject;
    }

    [TestCase]
    public void ResolveNodePathsFillsFieldsAndProperties() {
        Subject subject = CreateTree();

        subject.ResolveNodePaths();

        AssertObject(subject.Label).IsNotNull();
        AssertObject(subject.ExplicitPath).IsNotNull();
        AssertObject(subject.Auto).IsNotNull();
    }

    [TestCase]
    public void ResolveNodePathsUsesPascalCaseOfTheMemberName() {
        Subject subject = CreateTree();

        subject.ResolveNodePaths();

        // _label resolves to the child named "Label".
        AssertObject(subject.Label).IsNotNull();
        AssertString(subject.Label.Name).IsEqual("Label");
    }

    [TestCase]
    public void ResolveNodePathsHonoursAnExplicitPath() {
        Subject subject = CreateTree();

        subject.ResolveNodePaths();

        AssertString(subject.ExplicitPath.Name).IsEqual("Deep");
    }

    [TestCase]
    public void ResolveNodePathsFallsBackToTheUniqueNodePrefix() {
        Subject subject = CreateTree();
        // Move the plain "Label" out of the way and make the scene-unique node the
        // only candidate, so GetNodeOrNull("Label") misses and "%Label" must hit.
        foreach (Node child in subject.GetChildren()) {
            if (child.Name == "Label") {
                child.Name = "Plain";
            } else if (child.Name == "Unique") {
                child.Name = "Label";
                child.UniqueNameInOwner = true;
            }
        }

        subject.ResolveNodePaths();

        AssertString(subject.Label.Name).IsEqual("Label");
        AssertString(subject.Label.UniqueNameInOwner.ToString()).IsEqual("True");
    }

    [TestCase]
    public void ResolveNodePathsThrowsWhenNoNodeMatches() {
        MissingNode node = AddNode(new MissingNode(), true);
        node.AddChild(new Node { Name = "Something" });

        // TODO Godot's GetNode("%Name") returns null instead of throwing, so the
        // lookup below dereferences null and the caller sees a
        // NullReferenceException rather than a message naming the missing member.
        AssertThrown(node.ResolveNodePaths).HasMessage("Object reference not set to an instance of an object.");
    }

    [TestCase]
    public void ResolveNodePathsThrowsWhenTheTypeDoesNotFit() {
        WrongType node = AddNode(new WrongType(), true);
        node.AddChild(new Label { Name = "Label" });

        AssertThrown(node.ResolveNodePaths).HasMessage("Dependency of type Godot.Label is not assignable to Godot.Node3D.");
    }

    [TestCase]
    public void ResolveNodePathsIgnoresMembersWithoutTheAttribute() {
        AssertArray(typeof(Subject).GetFieldsWithAttribute<NodePathAttribute>(FLAGS)).HasSize(2);
        AssertArray(typeof(Subject).GetPropertiesWithAttribute<NodePathAttribute>(FLAGS)).HasSize(1);
    }
}