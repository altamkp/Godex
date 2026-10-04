namespace Godex.Tests;

using GdUnit4;
using Godot;
using static GdUnit4.Assertions;

[TestSuite]
public class FloatExtensionsTest {
    #region Trim

    [TestCase]
    public void TrimRoundsToNearestMultipleOfPrecision() {
        AssertFloat(0.123456f.Trim(0.01f)).IsEqual(0.12f);
        AssertFloat(0.129f.Trim(0.01f)).IsEqual(0.13f);
        AssertFloat(7f.Trim()).IsEqual(7f);
    }

    [TestCase]
    public void TrimUsesDefaultPrecision() {
        // DEFAULT_PRECISION is 0.0001, so the third decimal is dropped.
        AssertFloat(1.23456f.Trim()).IsEqual(1.2346f);
    }

    [TestCase]
    public void TrimKeepsSign() {
        AssertFloat(-0.126f.Trim(0.01f)).IsEqual(-0.13f);
    }

    #endregion

    #region IsEqualApprox

    [TestCase]
    public void IsEqualApproxUsesInclusiveComparison() {
        // |1 - 1.0001| sits marginally above DEFAULT_PRECISION in single precision,
        // so the boundary is probed with an exactly representable difference.
        AssertBool(0f.IsEqualApprox(MathfDef.DEFAULT_PRECISION)).IsTrue();
        AssertBool(1f.IsEqualApprox(1f)).IsTrue();
        AssertBool(1f.IsEqualApprox(1.00005f)).IsTrue();
        AssertBool(1f.IsEqualApprox(1.001f)).IsFalse();
    }

    [TestCase]
    public void IsEqualApproxUsesDefaultPrecision() {
        AssertBool(1f.IsEqualApprox(1.00005f)).IsTrue();
        AssertBool(1f.IsEqualApprox(1.001f)).IsFalse();
        // A precision wider than the default accepts what the default rejects.
        AssertBool(1f.IsEqualApprox(1.001f, 0.01f)).IsTrue();
    }

    [TestCase]
    public void IsEqualApproxRespectsExplicitPrecision() {
        AssertBool(1f.IsEqualApprox(1.5f, 1f)).IsTrue();
        AssertBool(1f.IsEqualApprox(1.5f, 0.4f)).IsFalse();
    }

    #endregion

    #region IsZeroApprox

    [TestCase]
    public void IsZeroApproxTreatsValuesWithinPrecisionAsZero() {
        AssertBool(0f.IsZeroApprox(0.001f)).IsTrue();
        AssertBool(0.0009f.IsZeroApprox(0.001f)).IsTrue();
        AssertBool(0.002f.IsZeroApprox(0.001f)).IsFalse();
    }

    [TestCase]
    public void IsZeroApproxIsSymmetricAroundZero() {
        AssertBool((-0.0005f).IsZeroApprox(0.001f)).IsTrue();
        AssertBool((-0.5f).IsZeroApprox(0.001f)).IsFalse();
    }

    #endregion

    #region MathfDef

    [TestCase]
    public void DefaultPrecisionIsPointZeroZeroZeroOne() {
        AssertFloat(MathfDef.DEFAULT_PRECISION).IsEqual(0.0001f);
    }

    [TestCase]
    public void HalfPiIsNinetyDegrees() {
        AssertFloat(MathfDef.HALF_PI).IsEqualApprox(Mathf.Pi / 2f, 0.0000001f);
    }

    #endregion
}