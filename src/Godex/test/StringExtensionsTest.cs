namespace Godex.Tests;

using GdUnit4;
using static GdUnit4.Assertions;

[TestSuite]
public class StringExtensionsTest {
    [TestCase]
    public void ToPascalCaseUpperCasesFirstCharacter() {
        AssertString("label".ToPascalCase()).IsEqual("Label");
        AssertString("myNodeName".ToPascalCase()).IsEqual("MyNodeName");
    }

    [TestCase]
    public void ToPascalCaseLeavesRestUntouched() {
        AssertString("myNODE".ToPascalCase()).IsEqual("MyNODE");
        AssertString("aBC".ToPascalCase()).IsEqual("ABC");
    }

    [TestCase]
    public void ToPascalCaseHandlesSingleCharacter() {
        AssertString("a".ToPascalCase()).IsEqual("A");
    }

    [TestCase]
    public void ToPascalCaseReturnsEmptyStringUnchanged() {
        AssertString(string.Empty.ToPascalCase()).IsEqual(string.Empty);
    }

    [TestCase]
    public void ToPascalCaseIsIdempotent() {
        AssertString("Node".ToPascalCase()).IsEqual("Node");
    }
}