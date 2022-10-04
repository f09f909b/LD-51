using Godot;
using System;

public class Key : Area2D
{
    [Signal] public delegate void Activated();
    [Signal] public delegate void PickedUpKey();

    private void OnEnterArea2D(Area2D body)
    {
        if (body.IsInGroup("Player"))
        {
            EmitSignal("PickedUpKey");
        }
        if (body.IsInGroup("Black"))
        {
            EmitSignal("Activated");
        }
    }
}
