namespace Godex.Tests;

using GdUnit4;
using Godot;
using static GdUnit4.Assertions;

[TestSuite]
public class Vector2ExtensionsTest {
    private static readonly Vector2 VECTOR = new(3f, -4f);

    #region Set

    [TestCase]
    public void SetXReplacesOnlyX() => AssertVector(VECTOR.SetX(10f)).IsEqual(new Vector2(10f, -4f));

    [TestCase]
    public void SetYReplacesOnlyY() => AssertVector(VECTOR.SetY(10f)).IsEqual(new Vector2(3f, 10f));

    #endregion

    #region Add

    [TestCase]
    public void AddXAddsToXOnly() => AssertVector(VECTOR.AddX(2f)).IsEqual(new Vector2(5f, -4f));

    [TestCase]
    public void AddYAddsToYOnly() => AssertVector(VECTOR.AddY(2f)).IsEqual(new Vector2(3f, -2f));

    [TestCase]
    public void AddAcceptsNegativeValues() => AssertVector(VECTOR.AddX(-5f)).IsEqual(new Vector2(-2f, -4f));

    #endregion

    #region Scale

    [TestCase]
    public void ScaleXScalesXOnly() => AssertVector(VECTOR.ScaleX(2f)).IsEqual(new Vector2(6f, -4f));

    [TestCase]
    public void ScaleYScalesYOnly() => AssertVector(VECTOR.ScaleY(2f)).IsEqual(new Vector2(3f, -8f));

    #endregion

    #region Max / Min

    [TestCase]
    public void MaxReturnsLargestAxisAndItsLength() {
        (Vector2.Axis axis, float length) = new Vector2(3f, -4f).Max();
        AssertInt((int)axis).IsEqual((int)Vector2.Axis.X);
        AssertFloat(length).IsEqual(3f);
    }

    [TestCase]
    public void MinReturnsSmallestAxisAndItsLength() {
        (Vector2.Axis axis, float length) = new Vector2(3f, -4f).Min();
        AssertInt((int)axis).IsEqual((int)Vector2.Axis.Y);
        AssertFloat(length).IsEqual(-4f);
    }

    [TestCase]
    public void MaxAndMinReturnAValidAxisWhenComponentsAreEqual() {
        // Which axis wins a tie is Godot's business (MaxAxisIndex/MinAxisIndex use
        // strict comparisons), so only the contract is asserted here: a real axis
        // whose component is the extreme value.
        (Vector2.Axis maxAxis, float max) = new Vector2(2f, 2f).Max();
        (Vector2.Axis minAxis, float min) = new Vector2(2f, 2f).Min();
        AssertFloat(max).IsEqual(2f);
        AssertFloat(min).IsEqual(2f);
        AssertBool(maxAxis is Vector2.Axis.X or Vector2.Axis.Y).IsTrue();
        AssertBool(minAxis is Vector2.Axis.X or Vector2.Axis.Y).IsTrue();
    }

    #endregion

    #region Flip

    [TestCase]
    public void FlipNegatesBothComponents() => AssertVector(VECTOR.Flip()).IsEqual(new Vector2(-3f, 4f));

    [TestCase]
    public void FlipIsItsOwnInverse() => AssertVector(VECTOR.Flip().Flip()).IsEqual(VECTOR);

    #endregion

    #region Trim

    [TestCase]
    public void TrimRoundsEachComponent() => AssertVector(new Vector2(1.23456f, 2.34567f).Trim()).IsEqualApprox(new Vector2(1.2346f, 2.3457f), new Vector2(0.00001f, 0.00001f));

    [TestCase]
    public void TrimRespectsExplicitPrecision() => AssertVector(new Vector2(1.24f, 5.76f).Trim(0.1f)).IsEqual(new Vector2(1.2f, 5.8f));

    #endregion

    #region Angle conversion

    [TestCase]
    public void DegToRadConvertsDegreesToRadians() {
        AssertVector(new Vector2(180f, 90f).DegToRad()).IsEqualApprox(new Vector2(Mathf.Pi, Mathf.Pi / 2f), new Vector2(0.000001f, 0.000001f));
    }

    [TestCase]
    public void RadToDegConvertsRadiansToDegrees() {
        AssertVector(new Vector2(Mathf.Pi, Mathf.Pi / 2f).RadToDeg()).IsEqualApprox(new Vector2(180f, 90f), new Vector2(0.000001f, 0.000001f));
    }

    [TestCase]
    public void DegToRadAndRadToDegAreInverse() {
        AssertVector(new Vector2(37f, -120f).DegToRad().RadToDeg()).IsEqualApprox(new Vector2(37f, -120f), new Vector2(0.000001f, 0.000001f));
    }

    #endregion

    #region Comparison

    [TestCase]
    public void IsEqualApproxComparesBothComponents() {
        AssertBool(new Vector2(1f, 2f).IsEqualApprox(new Vector2(1.00005f, 2f), 0.001f)).IsTrue();
        AssertBool(new Vector2(1f, 2f).IsEqualApprox(new Vector2(1f, 2.5f), 0.001f)).IsFalse();
    }

    [TestCase]
    public void IsZeroApproxDetectsNearZeroVectors() {
        AssertBool(new Vector2(0.0005f, -0.0005f).IsZeroApprox(0.001f)).IsTrue();
        AssertBool(new Vector2(0.5f, 0f).IsZeroApprox(0.001f)).IsFalse();
    }

    #endregion
}

[TestSuite]
public class Vector2IExtensionsTest {
    private static readonly Vector2I VECTOR = new(3, -4);

    [TestCase]
    public void SetXReplacesOnlyX() => AssertVector(VECTOR.SetX(10)).IsEqual(new Vector2I(10, -4));

    [TestCase]
    public void SetYReplacesOnlyY() => AssertVector(VECTOR.SetY(10)).IsEqual(new Vector2I(3, 10));

    [TestCase]
    public void AddXAddsToXOnly() => AssertVector(VECTOR.AddX(2)).IsEqual(new Vector2I(5, -4));

    [TestCase]
    public void AddYAddsToYOnly() => AssertVector(VECTOR.AddY(2)).IsEqual(new Vector2I(3, -2));

    [TestCase]
    public void ScaleXScalesAndRoundsXOnly() {
        AssertVector(VECTOR.ScaleX(2f)).IsEqual(new Vector2I(6, -4));
        AssertVector(VECTOR.ScaleX(0.5f)).IsEqual(new Vector2I(2, -4));
    }

    [TestCase]
    public void ScaleYScalesAndRoundsYOnly() {
        AssertVector(VECTOR.ScaleY(2f)).IsEqual(new Vector2I(3, -8));
        AssertVector(VECTOR.ScaleY(0.5f)).IsEqual(new Vector2I(3, -2));
    }

    [TestCase]
    public void ScaleRoundsHalvesToTheNearestEven() {
        // Mathf.RoundToInt rounds a tie to the nearest even number, so 3 * 1.5 = 4.5
        // lands on 4 rather than 5.
        AssertVector(VECTOR.ScaleX(1.5f)).IsEqual(new Vector2I(4, -4));
        AssertVector(new Vector2I(5, 0).ScaleX(1.1f)).IsEqual(new Vector2I(6, 0));
        AssertVector(new Vector2I(0, 5).ScaleY(1.1f)).IsEqual(new Vector2I(0, 6));
    }

    [TestCase]
    public void MaxReturnsLargestAxisAndItsLength() {
        (Vector2I.Axis axis, float length) = new Vector2I(3, -4).Max();
        AssertInt((int)axis).IsEqual((int)Vector2I.Axis.X);
        AssertFloat(length).IsEqual(3f);
    }

    [TestCase]
    public void MinReturnsSmallestAxisAndItsLength() {
        (Vector2I.Axis axis, float length) = new Vector2I(3, -4).Min();
        AssertInt((int)axis).IsEqual((int)Vector2I.Axis.Y);
        AssertFloat(length).IsEqual(-4f);
    }

    [TestCase]
    public void FlipNegatesBothComponents() => AssertVector(VECTOR.Flip()).IsEqual(new Vector2I(-3, 4));
}