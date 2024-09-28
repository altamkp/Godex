using Godot;

namespace Qkabi.GodotEx;

internal interface IInputHandler {
    string Id { get; }
    bool Pass { get; }
    bool Disabled { get; set; }

    bool Handle(InputEvent @event);
}
