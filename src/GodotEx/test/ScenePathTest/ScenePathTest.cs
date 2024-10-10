using Godot;

namespace GodotEx.Tests;

public partial class ScenePathTest : Node {
    public override void _Ready() {
        var image = GDx.New<TestImage>();
        var label = GDx.New<TestLabel>();

        AddChild(image);
        AddChild(label);
    }
}
