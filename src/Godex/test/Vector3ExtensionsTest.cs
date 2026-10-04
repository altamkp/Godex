namespace Godex.Tests;

using GdUnit4;
using Godot;
using static GdUnit4.Assertions;

[TestSuite]
public class Vector3ExtensionsTest {
    private static readonly Vector3 VECTOR = new(3f, -4f, 5f);

    #region Set

    [TestCase]
    public void SetXReplacesOnlyX() => AssertVector(VECTOR.SetX(10f)).IsEqual(new Vector3(10f, -4f, 5f));

    [TestCase]
    public void SetYReplacesOnlyY() => AssertVector(VECTOR.SetY(10f)).IsEqual(new Vector3(3f, 10f, 5f));

    [TestCase]
    public void SetZReplacesOnlyZ() => AssertVector(VECTOR.SetZ(10f)).IsEqual(new Vector3(3f, -4f, 10f));

    #endregion

    #region Add

    [TestCase]
    public void AddXAddsToXOnly() => AssertVector(VECTOR.AddX(2f)).IsEqual(new Vector3(5f, -4f, 5f));

    [TestCase]
    public void AddYAddsToYOnly() => AssertVector(VECTOR.AddY(2f)).IsEqual(new Vector3(3f, -2f, 5f));

    [TestCase]
    public void AddZAddsToZOnly() => AssertVector(VECTOR.AddZ(2f)).IsEqual(new Vector3(3f, -4f, 7f));

    #endregion

    #region Scale

    [TestCase]
    public void ScaleXScalesXOnly() => AssertVector(VECTOR.ScaleX(2f)).IsEqual(new Vector3(6f, -4f, 5f));

    [TestCase]
    public void ScaleYScalesYOnly() => AssertVector(VECTOR.ScaleY(2f)).IsEqual(new Vector3(3f, -8f, 5f));

    [TestCase]
    public void ScaleZScalesZOnly() => AssertVector(VECTOR.ScaleZ(2f)).IsEqual(new Vector3(3f, -4f, 10f));

    #endregion

    #region Max / Min

    [TestCase]
    public void MaxReturnsLargestAxisAndItsLength() {
        (Vector3.Axis axis, float length) = new Vector3(3f, -4f, 5f).Max();
        AssertInt((int)axis).IsEqual((int)Vector3.Axis.Z);
        AssertFloat(length).IsEqual(5f);
    }

    [TestCase]
    public void MinReturnsSmallestAxisAndItsLength() {
        (Vector3.Axis axis, float length) = new Vector3(3f, -4f, 5f).Min();
        AssertInt((int)axis).IsEqual((int)Vector3.Axis.Y);
        AssertFloat(length).IsEqual(-4f);
    }

    #endregion

    #region Flip / Trim

    [TestCase]
    public void FlipNegatesAllComponents() => AssertVector(VECTOR.Flip()).IsEqual(new Vector3(-3f, 4f, -5f));

    [TestCase]
    public void TrimRoundsEachComponent() => AssertVector(new Vector3(1.23456f, 2.34567f, 3.45678f).Trim()).IsEqualApprox(new Vector3(1.2346f, 2.3457f, 3.4568f), new Vector3(0.00001f, 0.00001f, 0.00001f));

    [TestCase]
    public void TrimRespectsExplicitPrecision() => AssertVector(new Vector3(1.24f, 5.76f, 9.84f).Trim(0.1f)).IsEqual(new Vector3(1.2f, 5.8f, 9.8f));

    #endregion

    #region Angle conversion

    [TestCase]
    public void DegToRadConvertsDegreesToRadians() {
        AssertVector(new Vector3(180f, 90f, 360f).DegToRad()).IsEqualApprox(new Vector3(Mathf.Pi, Mathf.Pi / 2f, Mathf.Pi * 2f), new Vector3(0.000001f, 0.000001f, 0.000001f));
    }

    [TestCase]
    public void RadToDegConvertsRadiansToDegrees() {
        AssertVector(new Vector3(Mathf.Pi, Mathf.Pi / 2f, Mathf.Pi * 2f).RadToDeg()).IsEqualApprox(new Vector3(180f, 90f, 360f), new Vector3(0.000001f, 0.000001f, 0.000001f));
    }

    [TestCase]
    public void DegToRadAndRadToDegAreInverse() {
        AssertVector(new Vector3(37f, -120f, 15f).DegToRad().RadToDeg()).IsEqualApprox(new Vector3(37f, -120f, 15f), new Vector3(0.000001f, 0.000001f, 0.000001f));
    }

    #endregion

    #region Comparison

    [TestCase]
    public void IsEqualApproxComparesAllComponents() {
        AssertBool(new Vector3(1f, 2f, 3f).IsEqualApprox(new Vector3(1.00005f, 2f, 3f), 0.001f)).IsTrue();
        AssertBool(new Vector3(1f, 2f, 3f).IsEqualApprox(new Vector3(1f, 2f, 3.5f), 0.001f)).IsFalse();
    }

    [TestCase]
    public void IsZeroApproxDetectsNearZeroVectors() {
        AssertBool(new Vector3(0.0005f, 0f, -0.0005f).IsZeroApprox(0.001f)).IsTrue();
        AssertBool(new Vector3(0.5f, 0f, 0f).IsZeroApprox(0.001f)).IsFalse();
    }

    #endregion
}

[TestSuite]
public class Vector3IExtensionsTest {
    private static readonly Vector3I VECTOR = new(3, -4, 5);

    [TestCase]
    public void SetXReplacesOnlyX() => AssertVector(VECTOR.SetX(10)).IsEqual(new Vector3I(10, -4, 5));

    [TestCase]
    public void SetYReplacesOnlyY() => AssertVector(VECTOR.SetY(10)).IsEqual(new Vector3I(3, 10, 5));

    [TestCase]
    public void SetZReplacesOnlyZ() => AssertVector(VECTOR.SetZ(10)).IsEqual(new Vector3I(3, -4, 10));

    [TestCase]
    public void AddXAddsToXOnly() => AssertVector(VECTOR.AddX(2)).IsEqual(new Vector3I(5, -4, 5));

    [TestCase]
    public void AddYAddsToYOnly() => AssertVector(VECTOR.AddY(2)).IsEqual(new Vector3I(3, -2, 5));

    [TestCase]
    public void AddZAddsToZOnly() => AssertVector(VECTOR.AddZ(2)).IsEqual(new Vector3I(3, -4, 7));

    [TestCase]
    public void ScaleXScalesAndRoundsXOnly() => AssertVector(VECTOR.ScaleX(2f)).IsEqual(new Vector3I(6, -4, 5));

    [TestCase]
    public void ScaleYScalesAndRoundsYOnly() => AssertVector(VECTOR.ScaleY(2f)).IsEqual(new Vector3I(3, -8, 5));

    [TestCase]
    public void ScaleZScalesAndRoundsZOnly() => AssertVector(VECTOR.ScaleZ(2f)).IsEqual(new Vector3I(3, -4, 10));

    [TestCase]
    public void ScaleRoundsHalvesToTheNearestEven() {
        // Mathf.RoundToInt rounds a tie to the nearest even number, so 3 * 1.5 = 4.5
        // lands on 4 rather than 5.
        AssertVector(VECTOR.ScaleX(1.5f)).IsEqual(new Vector3I(4, -4, 5));
        AssertVector(new Vector3I(5, 0, 0).ScaleX(1.1f)).IsEqual(new Vector3I(6, 0, 0));
        AssertVector(new Vector3I(0, 0, 5).ScaleZ(1.1f)).IsEqual(new Vector3I(0, 0, 6));
    }

    [TestCase]
    public void MaxReturnsLargestAxisAndItsLength() {
        (Vector3I.Axis axis, float length) = new Vector3I(3, -4, 5).Max();
        AssertInt((int)axis).IsEqual((int)Vector3I.Axis.Z);
        AssertFloat(length).IsEqual(5f);
    }

    [TestCase]
    public void MinReturnsSmallestAxisAndItsLength() {
        (Vector3I.Axis axis, float length) = new Vector3I(3, -4, 5).Min();
        AssertInt((int)axis).IsEqual((int)Vector3I.Axis.Y);
        AssertFloat(length).IsEqual(-4f);
    }

    [TestCase]
    public void FlipNegatesAllComponents() => AssertVector(VECTOR.Flip()).IsEqual(new Vector3I(-3, 4, -5));
}