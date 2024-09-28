using Godot;
using System.Runtime.CompilerServices;

namespace GodotEx;

/// <summary>
/// Attribute that is recognized by <see cref="GDx.New{T}(Action{T}?)"/> to instantiate custom 
/// node type labeled with this attribute by matching a tscn file with the same name 
/// under the same folder as the current script, or using the path if it is provided.
/// </summary>
/// <remarks>
/// <b>Example 1</b>: 
/// <br/>
/// [ScenePath]
/// <br/>
/// public class CustomLabel : Label { }
/// <br/>
/// <br/>
/// By using the default path, the attribute looks for a tscn file
/// with the same name and under the same folder as the current script. For example, if
/// CustomLabel.cs is located at res://src/UI/CustomLabel.cs, then it looks for
/// res://src/UI/CustomLabel.tscn to instantiate.
/// <br/>
/// <br/>
/// <b>Example 2</b>: 
/// <br/>
/// [ScenePath("res://src/UI/CustomImage.tscn")]
/// <br/>
/// public class CustomImage : TextureRect { }
/// <br/>
/// <br/>
/// By providing a path, the attribute looks for the tscn file at the given path. 
/// </remarks>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class PackedSceneAttribute : Attribute {
    /// <summary>
    /// Creates a new <see cref="PackedSceneAttribute"/> used with <see cref="GDx.New{T}(Action{T}?)"/>
    /// and similar overloads to instantiate scenes.
    /// </summary>
    /// <param name="scenePath">Scene path.</param>
    /// <exception cref="InvalidOperationException">Scene file not found.</exception>
    public PackedSceneAttribute(string? scenePath = null) {
        ScenePath = scenePath;
    }

    /// <summary>
    /// File path of the scene with the annotated script attached.
    /// </summary>
    public string? ScenePath { get; init; }
}
