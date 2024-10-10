using Godot;

namespace GodotEx;

/// <summary>
/// Extensions for <see cref="Viewport"/> related to raycast.
/// </summary>
public static class RaycastViewportExtensions {
    /// <summary>
    /// Returns raycast result from the current mouse position with <paramref name="layers"/>.
    /// </summary>
    /// <param name="viewport">Viewport to use.</param>
    /// <param name="layers">Collision layers to check.</param>
    /// <param name="length">Raycast length.</param>
    /// <returns>Raycast result, null if not found.</returns>
    public static RaycastHit3D? GetMouseRaycast(this Viewport viewport, uint layers, float length = RaycastDef.DEFAULT_RAY_LENGTH) {
        var camera = viewport.GetCamera3D();
        var mousePosition = viewport.GetMousePosition();
        var query = BuildQuery(camera, mousePosition, length, layers);
        return camera.GetWorld3D().DirectSpaceState.IntersectRay(query).ToRaycastHit3D();
    }

    /// <summary>
    /// Returns raycast result from the center of the viewport with <paramref name="layers"/>.
    /// </summary>
    /// <param name="viewport">Viewport to use.</param>
    /// <param name="layers">Collision layers to check.</param>
    /// <param name="length">Raycast length.</param>
    /// <returns>Raycast result, null if not found.</returns>
    public static RaycastHit3D? GetCenterRaycast(this Viewport viewport, uint layers, float length = RaycastDef.DEFAULT_RAY_LENGTH) {
        var camera = viewport.GetCamera3D();
        var center = viewport.GetVisibleRect().Size / 2;
        var query = BuildQuery(camera, center, length, layers);
        return camera.GetWorld3D().DirectSpaceState.IntersectRay(query).ToRaycastHit3D();
    }

    private static PhysicsRayQueryParameters3D BuildQuery(Camera3D camera, Vector2 source, float length, uint layers) {
        var from = camera.ProjectRayOrigin(source);
        var to = from + camera.ProjectRayNormal(source) * length;
        return new PhysicsRayQueryParameters3D() {
            From = from,
            To = to,
            CollisionMask = layers
        };
    }
}
