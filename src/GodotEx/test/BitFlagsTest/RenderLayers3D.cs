namespace GodotEx.Tests;

public enum RenderLayers3D : uint {
    Player = 1 << 0,
    Enemy = 1 << 1,

    All = uint.MaxValue
}
