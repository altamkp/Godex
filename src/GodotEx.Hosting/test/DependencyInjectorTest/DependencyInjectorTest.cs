using Godot;
using System;
using System.Text.RegularExpressions;
using Xunit;

namespace GodotEx.Hosting.Tests;

public partial class DependencyInjectorTest : Node {
    [Inject] private Random _random;
    [Inject] private AudioStreamPlayer _player;

    [Inject] public Regex Regex { get; private set; }

    public override void _Ready() {
        Assert.NotNull(_random);
        Assert.NotNull(_player);
        Assert.NotNull(Regex);
        
        GD.Print("Test passed.");
    }
}
