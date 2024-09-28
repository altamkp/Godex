using DotEx.Maths;
using Godot;

namespace GodotEx;

/// <summary>
/// Extensions for <see cref="Basis"/>.
/// </summary>
public static class BasisExtensions {
    /// <summary>
    /// Trims basis to a given <paramref name="precision"/>.
    /// </summary>
    /// <param name="basis">Basis to trim.</param>
    /// <param name="precision">Precision to use.</param>
    /// <returns>Trimmed basis.</returns>
    public static Basis Trim(this Basis basis, float precision = (float)MathDef.DEFAULT_PRECISION) {
        return new(basis.X.Trim(precision), basis.Y.Trim(precision), basis.Z.Trim(precision));
    }
}
