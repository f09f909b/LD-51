using Godot;
using System;
using Object = System.Object;

public class Player : Area2D
{
    [Signal] public delegate void LandedOnRegularPlanet();
    [Signal] public delegate void LandedOnHeavyPlanet();
    [Signal] public delegate void LandedOnLightPlanet();
    [Signal] public delegate void LandedOnVoid();
    [Signal] public delegate void LandedOnExit();
    [Signal] public delegate void CollidedWithSun();
    [Signal] public delegate void Moved();
    [Signal] public delegate void AttemptedGateOpened(string gateType); 
    private Tween _tween;
    private TileMap _tileMap;
    private Sprite _sprite;
    private AudioStreamPlayer _boostSfx;
    private AudioStreamPlayer _winSfx;
    private bool _justMoved;
    private bool _isBlackKeyActive;
    private bool _isBlueKeyActive;
    private bool _isYellowKeyActive;
    private const int TileSize = 32;
    private const float TimeToTile = 0.2f;

    public override void _Ready()
    {
        _boostSfx = GetParent().GetNode<AudioStreamPlayer>("SfxManager/Boost");
        _winSfx = GetParent().GetNode<AudioStreamPlayer>("SfxManager/Win");
        _tileMap = GetParent().GetNode<TileMap>("TileMap");
        _sprite = GetNode<Sprite>("Sprite");
        _tween = GetNode<Tween>("Tween");
        _tween.Connect("tween_completed", this, "CompleteCharacterMovement");
    }

    public override void _PhysicsProcess(float delta)
    {
        if (!_justMoved)
        {
            MoveCharacter();
        } 
    }

    private void MoveCharacter()
    {
        var motion = Vector2.Zero;

        if (Input.IsActionJustPressed("ui_up"))
        { 
            motion = Vector2.Up;
            _sprite.RotationDegrees = 180;
        }

        if (Input.IsActionJustPressed("ui_down"))
        {
            motion = Vector2.Down;
            _sprite.RotationDegrees = 0;
        }

        if (Input.IsActionJustPressed("ui_right"))
        {
            motion = Vector2.Right;
            _sprite.RotationDegrees = -90;
        }

        if (Input.IsActionJustPressed("ui_left"))
        {
            motion = Vector2.Left;
            _sprite.RotationDegrees = 90;
        }
        
        if (motion != Vector2.Zero)
        {
            var targetPosition = Position; 
            targetPosition += motion * TileSize;
            _tween.InterpolateProperty(this, "position", Position, targetPosition, TimeToTile, 
        Tween.TransitionType.Sine, Tween.EaseType.InOut); 
            _tween.Start();
            _justMoved = true;
            EmitSignal("Moved");
        }
    }

    private void CompleteCharacterMovement(System.Object obj, NodePath path)
    {
        _justMoved = false;
        _boostSfx.Play();
        CheckMapCoordinates();
    }

    private void CheckMapCoordinates()
    {
        var currentLocationToMap = _tileMap.WorldToMap(Position);
        var tileId = _tileMap.GetCellv(currentLocationToMap);
        if (tileId == 0)
        {
            EmitSignal("LandedOnRegularPlanet");
        }
        else if (tileId == 1)
        {
            EmitSignal("LandedOnHeavyPlanet"); 
        }
        else if (tileId == 2)
        {
            EmitSignal("LandedOnLightPlanet"); 
        }
        else if (tileId == 3)
        {
            EmitSignal("LandedOnExit");
        }
        else if (tileId == 4)
        {
            EmitSignal("CollidedWithSun");
        }
    }

    private void CollectKey(string name)
    {
        switch (name)
        {
            case "BlackKey":
                var blackKey = GetNode<Sprite>("KeyPickUp/BlackKey");
                blackKey.Visible = true;
                _isBlackKeyActive = true;
                break;
            case "BlueKey":
                var blueKey = GetNode<Sprite>("KeyPickUp/BlueKey");
                blueKey.Visible = true;
                _isBlueKeyActive = true;
                break;
            case "YellowKey":
                var yellowKey = GetNode<Sprite>("KeyPickUp/YellowKey");
                yellowKey.Visible = true;
                _isYellowKeyActive = true;
                break;
        }
    }
    
    private void OnArea2DEnter(Area2D body)
    {
        if (body.IsInGroup("Sun"))
        {
            EmitSignal("CollidedWithSun");
        }
        if (body.IsInGroup("Key"))
        {
            CollectKey(body.Name);
            body.QueueFree();
        }
        if (body.IsInGroup("Black") && _isBlackKeyActive)
        {
            EmitSignal("AttemptedGateOpened", "Black");
            _isBlackKeyActive = false;
        }
        if (body.IsInGroup("Blue") && _isBlueKeyActive)
        {
            EmitSignal("AttemptedGateOpened", "Blue");
            _isBlackKeyActive = false;
        }
        if (body.IsInGroup("Yellow") && _isYellowKeyActive)
        {
            EmitSignal("AttemptedGateOpened", "Yellow");
            _isBlackKeyActive = false;
        }
    }
}
