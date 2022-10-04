using Godot;
using System;

public class StopWatch : Control
{
    [Signal] public delegate void OnTenSecondsPassed();
    private float _time;
    private const float TimeIncrements = 1f;
    private Label _stopWatchDisplay;
    private Timer _stopWatchTimer;
    private Player _player;
    
    public override void _Ready()
    {
        _stopWatchDisplay = GetNode<Label>("StopWatchDisplay");
        _stopWatchTimer = GetNode<Timer>("Timer");
        _player = GetParent().GetParent().GetParent().GetNode<Player>("Player");
        _player.Connect("LandedOnRegularPlanet", this, "ApplyRegularPlanetTime");
        _player.Connect("LandedOnHeavyPlanet", this, "ApplyHeavyPlanetTime");
        _player.Connect("LandedOnLightPlanet", this, "ApplyLightPlanetTime");
    }

    private void AddTimeToWatch()
    {
        _time += TimeIncrements;
        _stopWatchDisplay.Text = _time.ToString();
        if (_time % 10 == 0)
        {
            EmitSignal("OnTenSecondsPassed");
        }
    }

    private void ApplyRegularPlanetTime()
    {
        _stopWatchTimer.WaitTime = 0.7f;
    }

    private void ApplyHeavyPlanetTime()
    {
        _stopWatchTimer.WaitTime = 1.7f;
    }
    
    private void ApplyLightPlanetTime()
    {
        _stopWatchTimer.WaitTime = 0.1f;
    }
}
