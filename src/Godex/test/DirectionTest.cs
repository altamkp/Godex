namespace Godex.Tests;

using GdUnit4;
using Godot;
using static GdUnit4.Assertions;

[TestSuite]
public class Direction2DTest {
    [TestCase]
    public void FlipInvertsEveryDirection() {
        AssertInt((int)Direction2D.Right.Flip()).IsEqual((int)Direction2D.Left);
        AssertInt((int)Direction2D.Left.Flip()).IsEqual((int)Direction2D.Right);
        AssertInt((int)Direction2D.Up.Flip()).IsEqual((int)Direction2D.Down);
        AssertInt((int)Direction2D.Down.Flip()).IsEqual((int)Direction2D.Up);
    }

    [TestCase]
    public void FlipThrowsForAnUndefinedDirection() {
        AssertThrown(() => ((Direction2D)99).Flip()).HasMessage("Non-existing direction 99.");
    }

    [TestCase]
    public void ToVector2MapsEveryDirection() {
        AssertVector(Direction2D.Right.ToVector2()).IsEqual(Vector2.Right);
        AssertVector(Direction2D.Left.ToVector2()).IsEqual(Vector2.Left);
        AssertVector(Direction2D.Up.ToVector2()).IsEqual(Vector2.Up);
        AssertVector(Direction2D.Down.ToVector2()).IsEqual(Vector2.Down);
    }

    [TestCase]
    public void ToVector2ThrowsForAnUndefinedDirection() {
        AssertThrown(() => ((Direction2D)99).ToVector2()).HasMessage("Non-existing 2D direction 99.");
    }

    [TestCase]
    public void ToDirectionRecognisesTheFourAxisDirections() {
        AssertInt((int)Vector2.Right.ToDirection()).IsEqual((int)Direction2D.Right);
        AssertInt((int)Vector2.Left.ToDirection()).IsEqual((int)Direction2D.Left);
        AssertInt((int)Vector2.Up.ToDirection()).IsEqual((int)Direction2D.Up);
        AssertInt((int)Vector2.Down.ToDirection()).IsEqual((int)Direction2D.Down);
    }

    [TestCase]
    public void ToDirectionAcceptsVectorsWithinTheDefaultPrecision() {
        AssertInt((int)new Vector2(1f, 0.00005f).ToDirection()).IsEqual((int)Direction2D.Right);
    }

    [TestCase]
    public void ToDirectionThrowsForADiagonalVector() {
        AssertThrown(() => new Vector2(1f, 1f).ToDirection())
            .HasMessage("Vector (1, 1) is not orthogonal to the any of the directions.");
    }

    [TestCase]
    public void ToDirectionRoundTripsThroughToVector2() {
        foreach (Direction2D direction in System.Enum.GetValues<Direction2D>()) {
            AssertInt((int)direction.ToVector2().ToDirection()).IsEqual((int)direction);
        }
    }
}

[TestSuite]
public class Direction3DTest {
    [TestCase]
    public void FlipInvertsEveryDirection() {
        AssertInt((int)Direction3D.Right.Flip()).IsEqual((int)Direction3D.Left);
        AssertInt((int)Direction3D.Left.Flip()).IsEqual((int)Direction3D.Right);
        AssertInt((int)Direction3D.Up.Flip()).IsEqual((int)Direction3D.Down);
        AssertInt((int)Direction3D.Down.Flip()).IsEqual((int)Direction3D.Up);
        AssertInt((int)Direction3D.Forward.Flip()).IsEqual((int)Direction3D.Back);
        AssertInt((int)Direction3D.Back.Flip()).IsEqual((int)Direction3D.Forward);
    }

    [TestCase]
    public void FlipThrowsForAnUndefinedDirection() {
        AssertThrown(() => ((Direction3D)99).Flip()).HasMessage("Non-existing direction 99.");
    }

    [TestCase]
    public void ToVector3MapsEveryDirection() {
        AssertVector(Direction3D.Right.ToVector3()).IsEqual(Vector3.Right);
        AssertVector(Direction3D.Left.ToVector3()).IsEqual(Vector3.Left);
        AssertVector(Direction3D.Up.ToVector3()).IsEqual(Vector3.Up);
        AssertVector(Direction3D.Down.ToVector3()).IsEqual(Vector3.Down);
        AssertVector(Direction3D.Forward.ToVector3()).IsEqual(Vector3.Forward);
        AssertVector(Direction3D.Back.ToVector3()).IsEqual(Vector3.Back);
    }

    [TestCase]
    public void ToVector3ThrowsForAnUndefinedDirection() {
        AssertThrown(() => ((Direction3D)99).ToVector3()).HasMessage("Non-existing 3D direction 99.");
    }

    [TestCase]
    public void ToDirectionRecognisesEveryAxisDirection() {
        AssertInt((int)Vector3.Right.ToDirection()).IsEqual((int)Direction3D.Right);
        AssertInt((int)Vector3.Left.ToDirection()).IsEqual((int)Direction3D.Left);
        AssertInt((int)Vector3.Up.ToDirection()).IsEqual((int)Direction3D.Up);
        AssertInt((int)Vector3.Down.ToDirection()).IsEqual((int)Direction3D.Down);
        AssertInt((int)Vector3.Forward.ToDirection()).IsEqual((int)Direction3D.Forward);
        AssertInt((int)Vector3.Back.ToDirection()).IsEqual((int)Direction3D.Back);
    }

    [TestCase]
    public void ToDirectionThrowsForAnArbitraryVector() {
        AssertThrown(() => new Vector3(1f, 2f, 3f).ToDirection())
            .HasMessage("Vector (1, 2, 3) is not orthogonal to the any of the directions.");
    }

    [TestCase]
    public void ToDirectionRoundTripsThroughToVector3() {
        foreach (Direction3D direction in System.Enum.GetValues<Direction3D>()) {
            AssertInt((int)direction.ToVector3().ToDirection()).IsEqual((int)direction);
        }
    }
}