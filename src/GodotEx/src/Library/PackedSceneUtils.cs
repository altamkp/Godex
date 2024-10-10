using System.Reflection;
using Godot;

namespace GodotEx;

/// <summary>
/// Utilities for <see cref="PackedScene"/>.
/// </summary>
public static class PackedSceneUtils {
    private const string SCENE_FILE_EXTENSION = ".tscn";

    /// <summary>
    /// Returns the <see cref="PackedScene"/> related to a custom node defined with the <see cref="PackedSceneAttribute"/>.
    /// </summary>
    /// <param name="nodeType">Custom node type.</param>
    /// <param name="cacheMode"><see cref="ResourceLoader"/> cache mode.</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static PackedScene GetPackedScene(Type nodeType,
                                             ResourceLoader.CacheMode cacheMode = ResourceLoader.CacheMode.Reuse) {
        var attribute = nodeType.GetCustomAttribute<PackedSceneAttribute>()
            ?? throw new InvalidOperationException($"[PackedScene] attribute is not defined on {nodeType.Name}.");
        var scenePath = attribute.ScenePath;

        if (scenePath == null) {
            var scriptPath = nodeType.GetCustomAttribute<ScriptPathAttribute>(false)?.Path
                ?? throw new InvalidOperationException($"[ScriptPath] attribute is not defined on {nodeType.Name}");
            scenePath = Path.ChangeExtension(scriptPath, SCENE_FILE_EXTENSION);
        }

        return ResourceLoader.Load<PackedScene>(scenePath, null, cacheMode);
    }

    /// <summary>
    /// Returns the PackedScene related to a custom node defined with the <see cref="PackedSceneAttribute"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="cacheMode"><see cref="ResourceLoader"/> cache mode.</param>
    /// <returns></returns>
    public static PackedScene GetPackedScene<T>(ResourceLoader.CacheMode cacheMode = ResourceLoader.CacheMode.Reuse) where T : Node {
        return GetPackedScene(typeof(T), cacheMode);
    }

}
