namespace Godex.Tests;

using GdUnit4;
using Godot;
using System;
using System.Linq;
using System.Reflection;
using static GdUnit4.Assertions;

[TestSuite]
public class TypeExtensionsTest {
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    private sealed class MarkerAttribute : Attribute {
        public string Name { get; }

        public MarkerAttribute(string name) {
            Name = name;
        }
    }

    private sealed class Subject {
        [Marker("field")]
        private int _markedField;

        [Marker("publicField")]
        public int MarkedPublicField;

        private int _plainField;

        [Marker("property")]
        public int MarkedProperty { get; set; }

        public int PlainProperty { get; set; }
    }

    private const BindingFlags FLAGS = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

    [TestCase]
    public void GetFieldsWithAttributeReturnsOnlyAnnotatedFields() {
        string[] names = Enumerable
            .Select(typeof(Subject).GetFieldsWithAttribute<MarkerAttribute>(FLAGS), field => field.Name)
            .ToArray();

        AssertArray(names).ContainsExactly("_markedField", "MarkedPublicField");
    }

    [TestCase]
    public void GetPropertiesWithAttributeReturnsOnlyAnnotatedProperties() {
        string[] names = Enumerable
            .Select(typeof(Subject).GetPropertiesWithAttribute<MarkerAttribute>(FLAGS), property => property.Name)
            .ToArray();

        AssertArray(names).ContainsExactly("MarkedProperty");
    }

    [TestCase]
    public void GetFieldsWithAttributeHonoursBindingFlags() {
        AssertArray(typeof(Subject).GetFieldsWithAttribute<MarkerAttribute>(BindingFlags.Instance | BindingFlags.Public))
            .HasSize(1);
    }

    [TestCase]
    public void GetFieldsAndAttributesCarriesTheAttributeInstances() {
        (FieldInfo FieldInfo, MarkerAttribute[] Attributes)[] infos = typeof(Subject)
            .GetFieldsAndAttributes<MarkerAttribute>(FLAGS)
            .ToArray();

        AssertInt(infos.Length).IsEqual(2);
        AssertString(infos[0].FieldInfo.Name).IsEqual("_markedField");
        AssertString(infos[0].Attributes[0].Name).IsEqual("field");
    }

    [TestCase]
    public void GetPropertiesAndAttributesCarriesTheAttributeInstances() {
        (PropertyInfo PropertyInfo, MarkerAttribute[] Attributes)[] infos = typeof(Subject)
            .GetPropertiesAndAttributes<MarkerAttribute>(FLAGS)
            .ToArray();

        AssertInt(infos.Length).IsEqual(1);
        AssertString(infos[0].PropertyInfo.Name).IsEqual("MarkedProperty");
        AssertString(infos[0].Attributes[0].Name).IsEqual("property");
    }

    [TestCase]
    public void UnrelatedAttributesAreNotMatched() {
        AssertArray(typeof(Subject).GetFieldsWithAttribute<ObsoleteAttribute>(FLAGS)).IsEmpty();
    }
}