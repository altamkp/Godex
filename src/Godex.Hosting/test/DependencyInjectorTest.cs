namespace Godex.Hosting.Tests;

using GdUnit4;
using Godex;
using Godex.Hosting;
using Godot;
using System;
using System.Text.RegularExpressions;
using static GdUnit4.Assertions;

[TestSuite]
[RequireGodotRuntime]
public partial class DependencyInjectorTest {
    /// <summary>Services ApplicationHost.ConfigureServices registers.</summary>
    private partial class Consumer : Node {
        [Inject] private Random _random;
        [Inject] public Regex Regex { get; set; }

        public Random Random => _random;
    }

    private partial class Inheriting : BaseConsumer { }

    private partial class BaseConsumer : Node {
        [Inject] private Random _random;

        public Random Random => _random;
    }

    [TestCase]
    public void InjectFillsFieldsAndPropertiesWhenTheNodeIsAdded() {
        Consumer consumer = AddNode(new Consumer(), true);

        AssertObject(consumer.Random).IsNotNull();
        AssertObject(consumer.Regex).IsNotNull();
    }

    [TestCase]
    public void InjectedPropertiesAreTheRegisteredInstances() {
        Consumer consumer = AddNode(new Consumer(), true);

        AssertString(consumer.Regex.ToString()).IsEqual(string.Empty);
    }

    [TestCase]
    public void InjectAlsoWalksBaseClasses() {
        Inheriting consumer = AddNode(new Inheriting(), true);

        AssertObject(consumer.Random).IsNotNull();
    }

    [TestCase]
    public void TheSameServiceInstanceIsSharedAcrossNodes() {
        Consumer first = AddNode(new Consumer(), true);
        Consumer second = AddNode(new Consumer(), true);

        AssertObject(second.Random).IsSame(first.Random);
    }
}
