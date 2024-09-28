using DotEx.Maths;
using Godot;
using System;
using System.Threading.Tasks;
using Xunit;

namespace GodotEx.Tests;

public partial class Node3DExtensionsTest : Node {
    [NodePath] private MeshInstance3D _mesh;

    private Random _rand = new();

    public override async void _Ready() {
        this.ResolveNodePaths();

        await Task.Delay(TimeSpan.FromSeconds(10));
        SetProcess(false);
        GD.Print("Test passed.");
    }

    public override void _Process(double delta) {
        _mesh.Rotate(
            new Vector3(_rand.NextSingle(), _rand.NextSingle(), _rand.NextSingle()).Normalized(),
            _rand.NextSingle());
        Assert.True(_mesh.Right().Length().IsEqualApprox(1));
        Assert.True(_mesh.Left().Length().IsEqualApprox(1));
        Assert.True(_mesh.Up().Length().IsEqualApprox(1));
        Assert.True(_mesh.Down().Length().IsEqualApprox(1));
        Assert.True(_mesh.Forward().Length().IsEqualApprox(1));
        Assert.True(_mesh.Back().Length().IsEqualApprox(1));
    }
}
