namespace Godex.Tests;

using GdUnit4;
using Godot;
using static GdUnit4.Assertions;

[TestSuite]
public class Transform3DExtensionsTest {
    private static Transform3D Rotated(float degrees, Vector3 axis) =>
        new(new Basis(new Quaternion(axis.Normalized(), Mathf.DegToRad(degrees))), Vector3.Zero);

    [TestCase]
    public void IdentityBasisPointsAlongTheGodotAxes() {
        Transform3D transform = Transform3D.Identity;

        AssertVector(transform.Right()).IsEqual(Vector3.Right);
        AssertVector(transform.Left()).IsEqual(Vector3.Left);
        AssertVector(transform.Up()).IsEqual(Vector3.Up);
        AssertVector(transform.Down()).IsEqual(Vector3.Down);
        // Godot looks down -Z, so forward is the negated Z basis vector.
        AssertVector(transform.Forward()).IsEqual(Vector3.Forward);
        AssertVector(transform.Back()).IsEqual(Vector3.Back);
    }

    [TestCase]
    public void OppositesAreAlwaysNegations() {
        Transform3D transform = Rotated(37f, Vector3.Up);

        AssertVector(transform.Left()).IsEqual(-transform.Right());
        AssertVector(transform.Down()).IsEqual(-transform.Up());
        AssertVector(transform.Back()).IsEqual(-transform.Forward());
    }

    [TestCase]
    public void DirectionsRotateWithTheBasis() {
        Transform3D transform = Rotated(90f, Vector3.Up);

        // A quarter turn about +Y carries the X basis vector onto +Z.
        AssertVector(transform.Right()).IsEqualApprox(Vector3.Forward, new Vector3(0.0001f, 0.0001f, 0.0001f));
        AssertVector(transform.Up()).IsEqualApprox(Vector3.Up, new Vector3(0.0001f, 0.0001f, 0.0001f));
    }

    [TestCase]
    public void DirectionsStayUnitLengthUnderRotation() {
        Transform3D transform = Rotated(123f, new Vector3(1f, 2f, 3f));

        AssertFloat(transform.Right().Length()).IsEqualApprox(1.0, 0.00001);
        AssertFloat(transform.Up().Length()).IsEqualApprox(1.0, 0.00001);
        AssertFloat(transform.Forward().Length()).IsEqualApprox(1.0, 0.00001);
    }

    [TestCase]
    public void DirectionsStayOrthogonalUnderRotation() {
        Transform3D transform = Rotated(50f, new Vector3(1f, 1f, 0f));

        AssertFloat(transform.Right().Dot(transform.Up())).IsEqualApprox(0.0, 0.00001);
        AssertFloat(transform.Right().Dot(transform.Forward())).IsEqualApprox(0.0, 0.00001);
    }

    [TestCase]
    public void TranslationDoesNotAffectDirections() {
        Transform3D transform = new(Basis.Identity, new Vector3(100f, 200f, 300f));

        AssertVector(transform.Right()).IsEqual(Vector3.Right);
        AssertVector(transform.Forward()).IsEqual(Vector3.Forward);
    }
}
