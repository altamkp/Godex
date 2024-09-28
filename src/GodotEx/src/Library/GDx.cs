using Godot;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using FileAccess = Godot.FileAccess;

namespace Qkabi.GodotEx;

/// <summary>
/// GodotEx's global functions.
/// </summary>
public static class GDx {
    static GDx() {
        const string PATH = "res://config.json";

        var exists = FileAccess.FileExists(PATH);
        if (exists) {
            var config = new MemoryStream(FileAccess.GetFileAsBytes(PATH));
            Config = new ConfigurationBuilder().AddJsonStream(config).Build();
        }
    }

    /// <summary>
    /// Global access to config.json.
    /// </summary>
    public static IConfiguration Config { get; private set; }

    /// <summary>
    /// Instantiates node from packed scene using its <paramref name="path"/>.
    /// Nodes are automatically resolved using this method, see <see cref="NodeExtensions.Resolve(Node)"/>.
    /// </summary>
    /// <param name="path">Packed scene path.</param>
    /// <param name="setup">Setup action.</param>
    /// <returns>Instantiated node.</returns>
    public static Node New(string path, Action<Node>? setup = null) {
        var node = GD.Load<PackedScene>(path).Instantiate();
        node.Resolve();
        setup?.Invoke(node);
        return node;
    }

    /// <summary>
    /// Instantiates node from packed scene using its <paramref name="path"/>.
    /// Nodes are automatically resolved using this method, see <see cref="NodeExtensions.Resolve(Node)"/>.
    /// </summary>
    /// <typeparam name="T">Node type.</typeparam>
    /// <param name="path">Packed scene path.</param>
    /// <param name="setup">Setup action.</param>
    /// <returns>Instantiated node of type <typeparamref name="T"/>.</returns>
    public static T New<T>(string path, Action<T>? setup = null) where T : Node {
        var node = GD.Load<PackedScene>(path).Instantiate<T>();
        node.Resolve();
        setup?.Invoke(node);
        return node;
    }

    /// <summary>
    /// Instantiates node from packed scene by matching a tscn file with the same name 
    /// under the same folder as the script defining the target node, or using the path 
    /// if it is provided. See <see cref="PackedSceneAttribute"/>.
    /// Nodes are automatically resolved using this method, see <see cref="NodeExtensions.Resolve(Node)"/>.
    /// </summary>
    /// <param name="type">Node type.</param>
    /// <param name="setup">Setup action.</param>
    /// <returns>Instantiated node of type <paramref name="type"/>.</returns>
    /// <exception cref="InvalidOperationException"><see cref="PackedSceneAttribute"/>
    /// not defined for type <paramref name="type"/>.</exception>
    public static Node New(Type type, Action<Node>? setup = null) {
        var attribute = type.GetCustomAttribute<PackedSceneAttribute>();
        var node = attribute == null ? (Node)Activator.CreateInstance(type)! : PackedSceneUtils.GetPackedScene(type).Instantiate();
        node.Resolve();
        setup?.Invoke(node);
        return node;
    }

    /// <summary>
    /// Instantiates node from packed scene by matching a tscn file with the same name 
    /// under the same folder as the script defining the target node, or using the path 
    /// if it is provided. See <see cref="PackedSceneAttribute"/>.
    /// Nodes are automatically resolved using this method, see <see cref="NodeExtensions.Resolve(Node)"/>.
    /// </summary>
    /// <typeparam name="T">Node type.</typeparam>
    /// <param name="setup">Setup action.</param>
    /// <returns>Instantiated node of type <typeparamref name="T"/>.</returns>
    /// <exception cref="InvalidOperationException"><see cref="PackedSceneAttribute"/>
    /// not defined for type <typeparamref name="T"/>.</exception>
    public static T New<T>(Action<T>? setup = null) where T : Node {
        var type = typeof(T);
        var attribute = type.GetCustomAttribute<PackedSceneAttribute>();
        var node = attribute == null ? Activator.CreateInstance<T>() : PackedSceneUtils.GetPackedScene(type).Instantiate<T>();
        node.Resolve();
        setup?.Invoke(node);
        return node;
    }
}
