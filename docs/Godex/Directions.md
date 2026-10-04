# Directions

`Direction2D` and `Direction3D` are arbitrary enum types that define all orthogonal directions in 2D/3D.

```csharp
public enum Direction2D {
    Right, Left, Up, Down
}
```

```csharp
public enum Direction3D {
    Right, Left, Up, Down, Forward, Back
}
```

Directions are meant to increase code readability since orthogonally defined directions are much more intuitive then vectors or even quaternions.

Let's say you have a free-look camera in your game, occasionally the player may want to set it to look directly forward, down, to the right, etc. You can then create a function for this camera with the help of `Direction3D`:

```csharp
public void LookAt(Direction3D direction) {
    camera.GlobalRotation = direction switch {
        Direction3D.Right => new Vector3(0, -Mathf.Pi / 2, 0);
        Direction3D.Left => new Vector3(0, Mathf.Pi / 2, 0);
        Direction3D.Up => new Vector3(-Mathf.Pi / 2, 0, 0);
        Direction3D.Down => new Vector3(Mathf.Pi / 2, 0, 0);
        Direction3D.Forward => Vector3.Zero;
        Direction3D.Back => new Vector3(0, Mathf.Pi, 0);
    }
}
```

## Extensions

Directions can be converted to and from their orthogonal vectors:

```csharp
Direction3D direction = Direction3D.Up;
Vector3 vector = direction.ToVector3();  // Vector3.Up
Direction3D same = vector.ToDirection();  // Direction3D.Up

Vector2 vector2 = Direction2D.Down.ToVector2();  // Vector2.Down
```

`ToDirection()` throws an `ArgumentException` when the vector is not orthogonal to any of the directions, and compares with the tolerance defined by `MathfDef.DEFAULT_PRECISION`.

Opposite directions are one call away with `Flip()`:

```csharp
Direction3D direction = Direction3D.Forward.Flip(); // Direction3D.Back
```
