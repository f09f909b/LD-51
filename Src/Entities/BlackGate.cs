using Godot;
using System;

public class BlackGate : Area2D
{
    [Export] private PackedScene _spawnObject;
    private string _gateName;
    private bool _isKeyInserted;
    private Player _player;
    private TileMap _tileMap;

    public override void _Ready()
    {
        _player = GetParent().GetNode<Player>("Player");
        _tileMap = GetParent().GetNode<TileMap>("TileMap");
        _player.Connect("AttemptedGateOpened", this, "AttemptGateOpen");
    }

    private void AttemptGateOpen(string name)
    {
        _isKeyInserted = true;
    }

    private void OnEnterArea2D(Area2D body)
    {
        if (body.IsInGroup("Player") && _isKeyInserted)
        {
            OpenGate();
            QueueFree();
            _isKeyInserted = false;
        }
    }

    private void OpenGate()
    {
        //var spawnedObject = (Area2D)_spawnObject.Instance();
        //spawnedObject.Position += new Vector2(0, Position.y + 16);
        //GetParent().AddChild(spawnedObject);
        var worldPos = new Vector2(Position.x, Position.y + 32);
        var posMap = _tileMap.WorldToMap(worldPos);
        _tileMap.SetCellv(posMap, 3);
    }
}
