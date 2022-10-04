using Godot;
using System;

public class Camera : Camera2D
{
    [Export] private int _amplitude = 17;
    [Export] private float _duration = 0.2f;
    [Export] private float _frequency = 15f;
    private Timer _frequencyTimer;
    private Timer _durationTimer;
    private Tween _tween;
    private Vector2 _randomVec;
    private Player _player;
    
    public override void _Ready()
    {
        _frequencyTimer = GetNode<Timer>("Frequency");
        _durationTimer = GetNode<Timer>("Duration");
        _tween = GetNode<Tween>("Tween");
        _player = GetParent().GetParent().GetNode<Player>("Player");
        _player.Connect("Moved", this, "StartShake");
    }

    private void StartShake()
    {
        _durationTimer.WaitTime = _duration;
        _frequencyTimer.WaitTime = 1 / _frequency;
        _frequencyTimer.Start();
        _durationTimer.Start();
        
        Shake();
    }

    private void Shake()
    {
        var random = new Random();
        _randomVec.x = random.Next(-_amplitude, _amplitude);
        _randomVec.y = random.Next(-_amplitude, _amplitude);
        _tween.InterpolateProperty(this, "offset", Offset, _randomVec, _frequencyTimer.WaitTime, Tween.TransitionType.Sine,
            Tween.EaseType.InOut);
        _tween.Start();
    }

    private void Reset()
    {
        _tween.InterpolateProperty(this, "offset", Offset, Vector2.Zero, _frequencyTimer.WaitTime, Tween.TransitionType.Sine,
            Tween.EaseType.InOut);
        _tween.Start();
    }

    private void OnFrequencyTimeout()
    {
        Shake();
    }

    private void OnDurationTimeout()
    {
        Reset();
        _frequencyTimer.Stop();
    }
}
