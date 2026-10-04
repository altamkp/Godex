namespace Godex.Hosting;

/// <summary>
/// Attribute that is recognized by <see cref="SingletonManager"/>, 
/// adding or removing the object labeled by this attribute to the scene tree
/// as singleton nodes as it is added to or removed from the tree.
/// Use with <see cref="Godex.Hosting.Host"/> is recommended.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class SingletonAttribute : Attribute { }
