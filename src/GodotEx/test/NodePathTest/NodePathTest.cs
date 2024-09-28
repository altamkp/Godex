using DotEx.Reflections;
using Godot;
using System.Reflection;
using Xunit;

namespace GodotEx.Tests;

public partial class NodePathTest : Node {
    [NodePath] 
    private Label _label;
    
    [NodePath("Label/AudioStreamPlayer2D")] 
    private AudioStreamPlayer2D _player;

    [NodePath]
    private Node _canvasLayer;

    [NodePath]
    public Label Label { get; set; }

    public override void _Ready() {
        this.ResolveNodePaths();

        var type = GetType();
        var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        foreach (var property in type.GetPropertiesWithAttribute<NodePathAttribute>(flags)) {
            Assert.NotNull(property.GetValue(this));
        }
        foreach (var field in type.GetFieldsWithAttribute<NodePathAttribute>(flags)) {
            Assert.NotNull(field.GetValue(this));
        }

        GD.Print("Test passed.");
    }
}
