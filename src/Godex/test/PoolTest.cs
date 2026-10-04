namespace Godex.Tests;

using GdUnit4;
using Godot;
using static GdUnit4.Assertions;

[TestSuite]
[RequireGodotRuntime]
public class PoolTest {
    private Node CreateRoot() => AddNode(new Node(), true);

    [TestCase]
    public void ConstructorFillsThePoolUpToCapacity() {
        Node root = CreateRoot();

        Pool<Node> pool = new Pool<Node>(root, 3);

        AssertInt(root.GetChildCount()).IsEqual(3);
        AssertInt(pool.GetInstance() is not null ? 1 : 0).IsEqual(1);
    }

    [TestCase]
    public void ConstructorRunsTheSetupActionForEveryInstance() {
        Node root = CreateRoot();
        int created = 0;

        Pool<Node> pool = new Pool<Node>(root, 2, node => {
            created++;
            // Unique names, otherwise AddChild renames the duplicates.
            node.Name = $"Pooled{created}";
        });

        AssertInt(created).IsEqual(2);
        AssertString(root.GetChild(0).Name).IsEqual("Pooled1");
        AssertString(root.GetChild(1).Name).IsEqual("Pooled2");
    }

    [TestCase]
    public void SetupActionAlsoRunsForInstancesCreatedOnDemand() {
        Node root = CreateRoot();
        int created = 0;

        Pool<Node> pool = new Pool<Node>(root, 1, _ => created++);
        // Empty the pool so the next call has to create an instance.
        pool.GetInstance();

        pool.GetInstance();

        AssertInt(created).IsEqual(2);
    }

    [TestCase]
    public void GetInstanceReusesAPooledInstance() {
        Node root = CreateRoot();
        Pool<Node> pool = new Pool<Node>(root, 1);

        Node first = pool.GetInstance();
        pool.ReturnInstance(first);
        Node second = pool.GetInstance();

        AssertObject(second).IsSame(first);
    }

    [TestCase]
    public void GetInstanceCreatesANewInstanceWhenThePoolIsEmpty() {
        Node root = CreateRoot();
        Pool<Node> pool = new Pool<Node>(root, 1);

        Node first = pool.GetInstance();
        Node second = pool.GetInstance();

        AssertObject(second).IsNotSame(first);
        AssertInt(root.GetChildCount()).IsEqual(2);
    }

    [TestCase]
    public void GetInstanceWithZeroCapacityAlwaysCreates() {
        Node root = CreateRoot();
        Pool<Node> pool = new Pool<Node>(root, 0);

        Node first = pool.GetInstance();
        Node second = pool.GetInstance();

        AssertObject(second).IsNotSame(first);
    }

    [TestCase]
    public void ReturnedInstancesAreHandedOutInFifoOrder() {
        Node root = CreateRoot();
        Pool<Node> pool = new Pool<Node>(root, 3);

        Node first = pool.GetInstance();
        Node second = pool.GetInstance();
        Node third = pool.GetInstance();
        pool.ReturnInstance(third);
        pool.ReturnInstance(second);
        pool.ReturnInstance(first);

        AssertObject(pool.GetInstance()).IsSame(third);
        AssertObject(pool.GetInstance()).IsSame(second);
        AssertObject(pool.GetInstance()).IsSame(first);
    }

    [TestCase]
    public void ReturnInstanceFreesTheNodeWhenThePoolIsFull() {
        Node root = CreateRoot();
        Pool<Node> pool = new Pool<Node>(root, 1);

        Node first = pool.GetInstance();
        Node second = pool.GetInstance();
        pool.ReturnInstance(first);
        // The pool now holds one instance, so this one is over the limit.
        pool.ReturnInstance(second);

        AssertBool(second.IsQueuedForDeletion()).IsTrue();
        AssertBool(first.IsQueuedForDeletion()).IsFalse();
    }

    [TestCase]
    public void PooledInstancesAreAttachedToTheGivenNode() {
        Node root = CreateRoot();
        Node container = new();
        root.AddChild(container);

        Pool<Node> pool = new Pool<Node>(container, 2);

        AssertInt(container.GetChildCount()).IsEqual(2);
        AssertObject(pool).IsNotNull();
    }

    [TestCase]
    public void PooledInstancesAreResolved() {
        Node root = CreateRoot();

        Pool<TestLabel> pool = new Pool<TestLabel>(root, 1);

        TestLabel label = pool.GetInstance();
        AssertBool(label.IsResolved()).IsTrue();
    }
}