# Basic Extensions

GodotEx basic extensions are a set of extension methods for Godot built-in classes such as [InputEvent](https://docs.godotengine.org/en/stable/classes/class_inputevent.html), [Node](https://docs.godotengine.org/en/stable/classes/class_node.html), [Transform3D](https://docs.godotengine.org/en/stable/classes/class_transform3d.html), etc. They are available through the `GodotEx` package.

Please refer to the [GodotEx Api](~/api/GodotEx.yml) for all available extensions. Here are some examples:

1.  Calculating distance between [Node2D](https://docs.godotengine.org/en/stable/classes/class_node2d.html)s/[Node3D](https://docs.godotengine.org/en/stable/classes/class_node3d.html)s

    Instead of writing

    ```csharp
    var distance = node1.GlobalPosition.DistanceTo(node2.GlobalPosition)
    ```

    With the extension you can write

    ```csharp
    var distance = node1.DistanceTo(node2)
    ```

2.  Getting local and global orthogonal vectors for [Node2D](https://docs.godotengine.org/en/stable/classes/class_node2d.html)s/[Node3D](https://docs.godotengine.org/en/stable/classes/class_node3d.html)s

    Instead of writing

    ```csharp
    var forward = node.Transform.Basis.Z;
    var globalForward = node.GlobalTransform.Basis.Z;
    ```

    With the extension you can write

    ```csharp
    var forward = node.Forward();
    var globalForward = node.GlobalForward();
    ```

3.  Matching input events

    Instead of writing

    ```csharp
    public override void _Input(InputEvent @event) {
        if (@event is InputEventMouseButton buttonEvent
                && buttonEvent.ButtonIndex == MouseButton.Left
                && buttonEvent.IsPressed();) {
            GD.Print("Mouse left button pressed.");
        }
    }
    ```

    With the extension you can write

    ```csharp
    public override void _Input(InputEvent @event) {
        if (@event.IsMousePressed(MouseButton.Left)) {
            GD.Print("Mouse left button pressed.");
        }
    }
    ```
