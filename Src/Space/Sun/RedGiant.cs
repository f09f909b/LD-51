using Godot;
using System;

public class RedGiant : Area2D
{
    private const int TranslationDistance = 64;
    private const int TimeToTranslate = 2;
    private Tween _tween;
    private StopWatch _stopWatch;

    public override void _Ready()
    {
        _tween = GetNode<Tween>("Tween");
        _stopWatch = GetParent().GetNode<StopWatch>("GUI/HUD/StopWatch");
        _stopWatch.Connect("OnTenSecondsPassed", this, "Translate");
    }

    private void Translate()
    {
        var targetPosition = Position; 
        targetPosition += new Vector2(0, TranslationDistance);
        _tween.InterpolateProperty(this, "position", Position, targetPosition, TimeToTranslate, 
            Tween.TransitionType.Sine, Tween.EaseType.InOut); 
        _tween.Start();
    }
}
