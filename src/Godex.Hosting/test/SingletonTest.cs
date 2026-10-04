namespace Godex.Hosting.Tests;

using GdUnit4;
using Godex;
using Godex.Hosting;
using Godot;
using System;
using static GdUnit4.Assertions;

[TestSuite]
[RequireGodotRuntime]
public partial class SingletonTest {
    [Singleton]
    private partial class Service : Node { }

    private partial class NotAService : Node { }

    [TestCase]
    public void SingletonIsRegisteredWhenTheNodeIsAdded() {
        Service service = AddNode(new Service(), true);
        SceneTree tree = (SceneTree)Engine.GetMainLoop();

        AssertObject(tree.GetSingleton<Service>()).IsSame(service);
    }

    [TestCase]
    public void SingletonIsFoundByTheNonGenericOverload() {
        Service service = AddNode(new Service(), true);
        SceneTree tree = (SceneTree)Engine.GetMainLoop();

        AssertObject(tree.GetSingleton(typeof(Service))).IsSame(service);
    }

    [TestCase]
    public void SingletonIsUnregisteredWhenTheNodeLeavesTheTree() {
        Service service = AddNode(new Service(), true);
        SceneTree tree = (SceneTree)Engine.GetMainLoop();
        AssertObject(tree.GetSingleton<Service>()).IsNotNull();

        service.Free();

        AssertObject(tree.GetSingleton<Service>()).IsNull();
    }

    [TestCase]
    public void PlainNodesAreNotRegistered() {
        AddNode(new NotAService(), true);
        SceneTree tree = (SceneTree)Engine.GetMainLoop();

        AssertObject(tree.GetSingleton<NotAService>()).IsNull();
    }

    [TestCase]
    public void GetSingletonOfAnUnregisteredTypeReturnsNull() {
        SceneTree tree = (SceneTree)Engine.GetMainLoop();

        AssertObject(tree.GetSingleton<NotAService>()).IsNull();
    }

    [TestCase]
    public void GetRequiredSingletonThrowsWhenNothingIsRegistered() {
        SceneTree tree = (SceneTree)Engine.GetMainLoop();

        AssertThrown(() => tree.GetRequiredSingleton<NotAService>()).IsInstanceOf<InvalidOperationException>();
    }
}
