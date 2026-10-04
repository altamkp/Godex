namespace Godex.Tests;

using GdUnit4;
using Godot;
using static GdUnit4.Assertions;

[TestSuite]
[RequireGodotRuntime]
public partial class GDxTest {
    /// <summary>No [PackedScene], so GDx falls back to Activator.CreateInstance.</summary>
    private partial class Plain : Node { }

    [TestCase]
    public void NewWithoutPackedSceneAttributeUsesTheParameterlessConstructor() {
        Plain node = GDx.New<Plain>();
        AutoFree(node);

        AssertObject(node).IsNotNull();
        AssertObject(node).IsInstanceOf<Plain>();
    }

    [TestCase]
    public void NewInstantiatesTheSceneWhenTheAttributeIsPresent() {
        TestLabel label = GDx.New<TestLabel>();
        AutoFree(label);

        // "Hello world" only exists in TestLabel.tscn, so this proves the scene was
        // used rather than a bare TestLabel.
        AssertString(label.Text).IsEqual("Hello world");
    }

    [TestCase]
    public void NewHonoursAnExplicitScenePath() {
        TestImage image = GDx.New<TestImage>();
        AutoFree(image);

        AssertObject(image).IsNotNull();
        AssertObject(image).IsInstanceOf<TextureRect>();
    }

    [TestCase]
    public void NewAcceptsAnExplicitPath() {
        Node node = GDx.New("res://Fixtures/TestLabel.tscn");
        AutoFree(node);

        AssertObject(node).IsInstanceOf<TestLabel>();
    }

    [TestCase]
    public void NewAcceptsAType() {
        Node node = GDx.New(typeof(TestLabel));
        AutoFree(node);

        AssertObject(node).IsInstanceOf<TestLabel>();
    }

    [TestCase]
    public void NewResolvesTheNodeItCreates() {
        TestLabel label = GDx.New<TestLabel>();
        AutoFree(label);

        AssertBool(label.IsResolved()).IsTrue();
    }

    [TestCase]
    public void NewInvokesTheSetupAction() {
        bool called = false;

        Plain node = GDx.New<Plain>(created => {
            called = true;
            created.Name = "Renamed";
        });
        AutoFree(node);

        AssertBool(called).IsTrue();
        AssertString(node.Name).IsEqual("Renamed");
    }

    [TestCase]
    public void GetPackedSceneReturnsTheSceneOfTheAttribute() {
        PackedScene scene = PackedSceneUtils.GetPackedScene<TestLabel>();

        AssertObject(scene).IsNotNull();
        AssertObject(scene.Instantiate()).IsInstanceOf<TestLabel>();
    }

    [TestCase]
    public void GetPackedSceneThrowsWithoutTheAttribute() {
        AssertThrown(() => PackedSceneUtils.GetPackedScene<Plain>())
            .HasMessage("[PackedScene] attribute is not defined on Plain.");
    }

    [TestCase]
    public void PackedSceneAttributeKeepsTheGivenPath() {
        AssertString(new PackedSceneAttribute("res://Fixtures/TestImage.tscn").ScenePath)
            .IsEqual("res://Fixtures/TestImage.tscn");
        AssertObject(new PackedSceneAttribute().ScenePath).IsNull();
    }
}