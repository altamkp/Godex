namespace GodotEx.Tests;

public enum PhysicsLayers3D : uint {
    None = 0,

    Player = 1 << 0,
    Melee = 1 << 1,
    Projectile = 1 << 2,
    Ground = 1 << 3,

    NonPlayer = ~Player
}
